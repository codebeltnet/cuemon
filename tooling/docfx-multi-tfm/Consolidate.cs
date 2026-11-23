#:package YamlDotNet@18.1.0
#:property PublishAot=false

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.Json;
using YamlDotNet.RepresentationModel;

namespace Cuemon.Tooling.Docfx;

internal static class Consolidate
{
    private const string ManagedReference = "### YamlMime:ManagedReference";
    private const string TableOfContent = "### YamlMime:TableOfContent";

    public static void Main(string[] args)
    {
        if (args.Length < 3 || args.Length % 2 == 0)
        {
            throw new ArgumentException("Usage: Consolidate.cs <new-output-directory> <tfm> <metadata-directory> [<tfm> <metadata-directory> ...]");
        }

        var inputs = args.Skip(1).Chunk(2).ToDictionary(pair => pair[0], pair => pair[1], StringComparer.Ordinal);
        var pages = new Dictionary<string, (string File, YamlMappingNode Page)>(StringComparer.Ordinal);
        var locations = new SortedDictionary<string, string>(StringComparer.Ordinal);
        var fileNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "toc.yml" };
        var variants = new Dictionary<string, Dictionary<string, YamlMappingNode>>(StringComparer.Ordinal);
        YamlMappingNode? toc = null;

        foreach (var (framework, directory) in inputs)
        {
            var manifest = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(Path.Combine(directory, ".manifest")))!;
            var inventory = new Dictionary<string, string>(StringComparer.Ordinal);
            foreach (var path in Directory.GetFiles(directory, "*.yml").Order(StringComparer.Ordinal))
            {
                var file = Path.GetFileName(path);
                if (file == "toc.yml") { continue; }
                var page = Read(path, ManagedReference);
                var items = Sequence(page, "items");
                var rootItem = (YamlMappingNode)items.First();
                var root = Uid(rootItem);
                var rootChildren = Strings(rootItem, "children").ToHashSet(StringComparer.Ordinal);
                foreach (YamlMappingNode item in items)
                {
                    var uid = Uid(item);
                    Require(uid == root || (Value(item, "parent") == root && rootChildren.Contains(uid)),
                        $"{framework}: {uid} has no owning child entry in {file}.");
                    inventory.Add(uid, file);
                    if (!variants.TryGetValue(uid, out var byFramework))
                    {
                        byFramework = new Dictionary<string, YamlMappingNode>(StringComparer.Ordinal);
                        variants.Add(uid, byFramework);
                        locations.Add(uid, file);
                    }
                    Require(locations[uid] == file, $"{uid}: page changed from {locations[uid]} to {file} ({framework}).");
                    byFramework.Add(framework, item);
                }

                if (!pages.TryGetValue(root, out var existing))
                {
                    Require(fileNames.Add(file), $"Page filename collision: {file} ({framework}).");
                    pages.Add(root, (file, page));
                    continue;
                }

                Compare(existing.Page, page, $"{root} page ({framework})", "items", "references");
                MergeSequence(existing.Page, page, "items", (left, right) =>
                    Compare(left, right, $"{Uid(left)} ({framework})", "children", "inheritedMembers", "derivedClasses"));
                MergeSequence(existing.Page, page, "references", (left, right) =>
                    Compare(left, right, $"{file}, reference {Uid(left)} ({framework})"));
            }

            Require(inventory.Count > 0 && inventory.Count == manifest.Count &&
                inventory.All(pair => manifest.TryGetValue(pair.Key, out var file) && file == pair.Value),
                $"{framework}: generated pages disagree with the DocFX manifest.");
            var sourceToc = Read(Path.Combine(directory, "toc.yml"), TableOfContent);
            if (toc is null) { toc = sourceToc; }
            else { MergeToc(toc, sourceToc, framework); }
            Console.WriteLine($"{framework}: {inventory.Count} UIDs");
        }

        var output = args[0];
        Require(!Directory.Exists(output), $"Output directory already exists: {output}");
        if (inputs.Count == 1)
        {
            Directory.CreateDirectory(output);
            foreach (var file in Directory.GetFiles(inputs.Values.Single()))
            {
                File.Copy(file, Path.Combine(output, Path.GetFileName(file)));
            }
            return;
        }

        var tocUids = new HashSet<string>(StringComparer.Ordinal);
        foreach (var uid in TocUids(toc!))
        {
            Require(tocUids.Add(uid), $"Duplicate UID in TOC: {uid}.");
        }
        foreach (var (file, page) in pages.Values)
        {
            var root = Uid((YamlMappingNode)Sequence(page, "items").First());
            Require(tocUids.Remove(root), $"Page missing from TOC: {root}.");
            var references = Sequence(page, "references").Cast<YamlMappingNode>().ToDictionary(Uid, StringComparer.Ordinal);
            foreach (YamlMappingNode item in Sequence(page, "items"))
            {
                var uid = Uid(item);
                var byFramework = variants[uid];
                var children = byFramework.Values.SelectMany(node => Strings(node, "children")).Distinct(StringComparer.Ordinal);
                SetSequence(item, "children", children);
                foreach (var child in Strings(item, "children"))
                {
                    Require(locations.ContainsKey(child) && (locations[child] == file || references.ContainsKey(child)),
                        $"{uid}: missing child or child reference {child}.");
                }
                var overload = Value(item, "overload");
                Require(overload is null || references.ContainsKey(overload), $"{uid}: missing overload reference {overload}.");
                // DocFX records a machine-relative path in addition to the canonical Git source URL.
                if (item.Children.TryGetValue("source", out var source) && source is YamlMappingNode sourceMap &&
                    sourceMap.Children.TryGetValue("remote", out var remote) && remote is YamlMappingNode remoteMap &&
                    Value(remoteMap, "path") is { } repositoryPath)
                {
                    sourceMap.Children["path"] = new YamlScalarNode(repositoryPath);
                }

                if (Value(item, "type") == "Namespace") { continue; }
                item.Children.Add("platform", new YamlSequenceNode(byFramework.Keys.Select(framework => new YamlScalarNode(framework))));
                var notes = new List<string> { $"**Available in documented targets:** {string.Join(", ", byFramework.Keys.Select(framework => $"`{framework}`"))}." };
                foreach (var (key, label) in new[] { ("inheritedMembers", "Inherited members"), ("derivedClasses", "Derived types") })
                {
                    var sets = byFramework.ToDictionary(pair => pair.Key, pair => Strings(pair.Value, key).ToArray(), StringComparer.Ordinal);
                    var first = sets.Values.First();
                    if (sets.Values.All(entries => entries.SequenceEqual(first, StringComparer.Ordinal))) { continue; }

                    var shared = first.Where(entry => sets.Values.All(entries => entries.Contains(entry, StringComparer.Ordinal))).ToArray();
                    SetSequence(item, key, shared);
                    foreach (var group in sets.GroupBy(pair => string.Join("\n", pair.Value.Except(shared, StringComparer.Ordinal))))
                    {
                        if (group.Key.Length == 0) { continue; }
                        var links = group.Key.Split('\n').Select(entry => $"<xref href=\"{SecurityElement.Escape(entry)}\" data-throw-if-not-resolved=\"false\"></xref>");
                        notes.Add($"**{label} in {string.Join(", ", group.Select(pair => pair.Key))}:** {string.Join(", ", links)}.");
                    }
                    Console.WriteLine($"{uid}: framework-specific {key}");
                }
                var remarks = Value(item, "remarks");
                if (!string.IsNullOrEmpty(remarks)) { notes.Add(remarks); }
                item.Children["remarks"] = new YamlScalarNode(string.Join("\n\n", notes)) { Style = YamlDotNet.Core.ScalarStyle.Literal };
            }
        }
        Require(tocUids.Count == 0, $"TOC UIDs without pages: {string.Join(", ", tocUids.Order(StringComparer.Ordinal))}");

        Directory.CreateDirectory(output);
        foreach (var (file, page) in pages.Values)
        {
            Write(Path.Combine(output, file), ManagedReference, page);
        }
        Write(Path.Combine(output, "toc.yml"), TableOfContent, toc!);
        File.WriteAllText(Path.Combine(output, ".manifest"), JsonSerializer.Serialize(locations, new JsonSerializerOptions { WriteIndented = true, NewLine = "\n" }));
        Console.WriteLine($"Union: {locations.Count} UIDs in {pages.Count} managed-reference pages.");
    }

    private static void MergeToc(YamlMappingNode left, YamlMappingNode right, string framework)
    {
        Compare(left, right, $"TOC {Value(left, "uid")} ({framework})", "items");
        MergeSequence(left, right, "items", (a, b) => MergeToc(a, b, framework));
    }

    private static IEnumerable<string> TocUids(YamlMappingNode node)
    {
        foreach (YamlMappingNode child in Sequence(node, "items"))
        {
            yield return Uid(child);
            foreach (var uid in TocUids(child)) { yield return uid; }
        }
    }

    private static void MergeSequence(YamlMappingNode left, YamlMappingNode right, string key, Action<YamlMappingNode, YamlMappingNode> compare)
    {
        var sequence = Sequence(left, key);
        var index = sequence.Cast<YamlMappingNode>().ToDictionary(Uid, StringComparer.Ordinal);
        foreach (var (uid, node) in Sequence(right, key).Cast<YamlMappingNode>().ToDictionary(Uid, StringComparer.Ordinal))
        {
            if (index.TryGetValue(uid, out var existing)) { compare(existing, node); }
            else { sequence.Add(node); }
        }
        if (sequence.Children.Count > 0) { left.Children[key] = sequence; }
    }

    private static void Compare(YamlMappingNode left, YamlMappingNode right, string context, params string[] handledKeys)
    {
        foreach (var key in left.Children.Keys.Concat(right.Children.Keys).Distinct())
        {
            if (handledKeys.Contains(((YamlScalarNode)key).Value, StringComparer.Ordinal)) { continue; }
            left.Children.TryGetValue(key, out var a);
            right.Children.TryGetValue(key, out var b);
            Require(Equals(a, b), $"{context}: conflicting '{key}'.\nPrimary: {a}\nAlternate: {b}");
        }
    }

    private static YamlMappingNode Read(string path, string mime)
    {
        using (var reader = File.OpenText(path))
        {
            Require(reader.ReadLine() == mime, $"Unexpected DocFX MIME: {path}");
            var yaml = new YamlStream();
            yaml.Load(reader);
            return (YamlMappingNode)yaml.Documents.Single().RootNode;
        }
    }

    private static void Write(string path, string mime, YamlMappingNode node)
    {
        using (var writer = new StringWriter { NewLine = "\n" })
        {
            writer.WriteLine(mime);
            new YamlStream(new YamlDocument(node)).Save(writer, assignAnchors: false);
            File.WriteAllText(path, writer.ToString().Replace("\r\n", "\n", StringComparison.Ordinal), new UTF8Encoding(false));
        }
    }

    private static string Uid(YamlMappingNode node) => Value(node, "uid") ?? throw new InvalidDataException("Missing DocFX UID.");
    private static string? Value(YamlMappingNode node, string key) => node.Children.TryGetValue(key, out var value) ? ((YamlScalarNode)value).Value : null;
    private static YamlSequenceNode Sequence(YamlMappingNode node, string key) => node.Children.TryGetValue(key, out var value) ? (YamlSequenceNode)value : new YamlSequenceNode();
    private static IEnumerable<string> Strings(YamlMappingNode node, string key) => Sequence(node, key).Cast<YamlScalarNode>().Select(value => value.Value!);

    private static void SetSequence(YamlMappingNode node, string key, IEnumerable<string> entries)
    {
        var sequence = new YamlSequenceNode(entries.Select(entry => new YamlScalarNode(entry)));
        if (sequence.Children.Count == 0) { node.Children.Remove(key); }
        else { node.Children[key] = sequence; }
    }

    private static void Require(bool condition, string message)
    {
        if (!condition) { throw new InvalidDataException(message); }
    }
}
