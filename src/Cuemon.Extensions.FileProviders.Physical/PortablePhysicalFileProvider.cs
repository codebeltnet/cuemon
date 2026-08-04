using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Physical;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Security;

namespace Cuemon.Extensions.FileProviders;

/// <summary>
/// Provides portable, case-insensitive path resolution for files and directories rooted in the physical file system.
/// </summary>
/// <remarks>
/// <para>
/// This provider decorates <see cref="PhysicalFileProvider"/> and resolves each path segment using ordinal, case-insensitive comparison before delegating file metadata, streams, directory contents, and change notifications to the underlying provider.
/// </para>
/// <para>
/// This provides consistent case-insensitive lookup behavior across case-sensitive and case-insensitive file systems while preserving the casing of files and directories maintained by the underlying physical file system.
/// </para>
/// <para>
/// A path segment is resolved when exactly one physical entry matches using <see cref="StringComparison.OrdinalIgnoreCase"/>. For example, when the physical file is named <c>logo.svg</c>, requests for <c>logo.svg</c>, <c>Logo.svg</c>, and <c>LOGO.SVG</c> all resolve to that file.
/// </para>
/// <para>
/// Path segments that differ only by casing have the same logical identity. If a case-sensitive file system contains multiple matching entries, such as <c>logo.svg</c> and <c>Logo.svg</c>, or a file and directory like <c>logo</c> and <c>Logo/</c>, the path segment is treated as a casing collision and none of the matching entries are selected, even when the requested casing exactly matches one of them.
/// </para>
/// <para>
/// A casing collision does not throw an exception. <see cref="GetFileInfo"/> returns a <see cref="NotFoundFileInfo"/>, <see cref="GetDirectoryContents"/> returns <see cref="NotFoundDirectoryContents.Singleton"/>, and <see cref="Watch"/> returns <see cref="NullChangeToken.Singleton"/> for a colliding literal filter.
/// </para>
/// <para>
/// Wildcard watch filters are delegated unchanged and are not inspected for casing collisions. Literal file filters, and literal directory filters that end with a trailing directory separator, are resolved to their physical casing when the corresponding entry exists without a collision. Literal directory filters without a trailing separator are delegated unchanged and follow the normal interpretation of <see cref="PhysicalFileProvider"/>. <see cref="PhysicalFileProvider"/> distinguishes wildcard watch patterns by the presence of <c>*</c>.
/// </para>
/// <para>
/// Successfully resolved paths are cached using ordinal, case-insensitive keys for the lifetime of this provider. Misses and collisions are not cached and are re-evaluated on each call. The root should therefore have a stable naming topology for previously successful logical paths. After a case-only rename, or after introducing or removing a casing collision for a previously successful logical path, create a new provider instance to guarantee that the path is resolved again.
/// </para>
/// </remarks>
/// <param name="root">The absolute directory to use as the provider root.</param>
/// <param name="filters">A bitwise combination of values that specifies which files or directories are excluded.</param>
public sealed class PortablePhysicalFileProvider(string root, ExclusionFilters filters = ExclusionFilters.Sensitive) : Disposable, IFileProvider
{
    private readonly PhysicalFileProvider _provider = new(root, filters);
    private readonly ConcurrentDictionary<string, string> _files = new(StringComparer.OrdinalIgnoreCase);
    private readonly ConcurrentDictionary<string, string> _directories = new(StringComparer.OrdinalIgnoreCase);
    private static readonly char[] PathSeparators = { '/' };

    /// <inheritdoc cref="PhysicalFileProvider.Root"/>
    public string Root => _provider.Root;

    /// <inheritdoc cref="PhysicalFileProvider.UsePollingFileWatcher"/>
    public bool UsePollingFileWatcher
    {
        get => _provider.UsePollingFileWatcher;
        set => _provider.UsePollingFileWatcher = value;
    }

    /// <inheritdoc cref="PhysicalFileProvider.UseActivePolling"/>
    public bool UseActivePolling
    {
        get => _provider.UseActivePolling;
        set => _provider.UseActivePolling = value;
    }

    /// <inheritdoc />
    public IFileInfo GetFileInfo(string subpath)
    {
        var resolvedPath = ResolvePath(subpath, finalSegmentIsDirectory: false, out var collision);
        return ResolveFileInfo(subpath, resolvedPath, collision);
    }

    /// <inheritdoc />
    public IDirectoryContents GetDirectoryContents(string subpath)
    {
        var resolvedPath = ResolvePath(subpath, finalSegmentIsDirectory: true, out var collision);
        return ResolveDirectoryContents(resolvedPath, collision);
    }

    /// <inheritdoc />
    public IChangeToken Watch(string filter)
    {
        // Preserve PhysicalFileProvider behavior for null, empty, and wildcard filters.
        // PhysicalFileProvider uses '*' to distinguish wildcard watch patterns.
        if (string.IsNullOrEmpty(filter) || filter.IndexOf('*') >= 0)
        {
            return _provider.Watch(filter);
        }

#if NETSTANDARD2_0
        var finalSegmentIsDirectory = filter[filter.Length - 1] is '/' or '\\';
#else
        var finalSegmentIsDirectory = filter[^1] is '/' or '\\';
#endif
        var resolvedFilter = ResolvePath(filter, finalSegmentIsDirectory, out var collision);
        return ResolveWatchToken(resolvedFilter, finalSegmentIsDirectory, collision);
    }

    /// <inheritdoc />
    protected override void OnDisposeManagedResources()
    {
        _provider.Dispose();
    }

    private string ResolvePath(string subpath, bool finalSegmentIsDirectory, out bool collision)
    {
        return ResolvePath(subpath, finalSegmentIsDirectory, path => _provider.GetDirectoryContents(path), out collision);
    }

    private IFileInfo ResolveFileInfo(string subpath, string resolvedPath, bool collision)
    {
        return collision ? new NotFoundFileInfo(subpath) : _provider.GetFileInfo(resolvedPath);
    }

    private IDirectoryContents ResolveDirectoryContents(string resolvedPath, bool collision)
    {
        return collision ? NotFoundDirectoryContents.Singleton : _provider.GetDirectoryContents(resolvedPath);
    }

    private IChangeToken ResolveWatchToken(string resolvedFilter, bool finalSegmentIsDirectory, bool collision)
    {
        if (collision)
        {
            return NullChangeToken.Singleton;
        }

        if (finalSegmentIsDirectory && resolvedFilter.Length > 0 &&
#if NETSTANDARD2_0
            resolvedFilter[resolvedFilter.Length - 1] is not ('/' or '\\'))
#else
            resolvedFilter[^1] is not ('/' or '\\'))
#endif
        {
            resolvedFilter += Path.DirectorySeparatorChar;
        }

        return _provider.Watch(resolvedFilter);
    }

    private string ResolvePath(string subpath, bool finalSegmentIsDirectory, Func<string, IEnumerable<IFileInfo>> entriesFactory, out bool collision)
    {
        collision = false;

        if (string.IsNullOrEmpty(subpath))
        {
            return subpath;
        }

        var cache = finalSegmentIsDirectory ? _directories : _files;

        if (cache.TryGetValue(subpath, out var cachedPath))
        {
            return cachedPath;
        }

        var segments = subpath.Split(PathSeparators, StringSplitOptions.RemoveEmptyEntries);

        if (segments.Length == 0)
        {
            return subpath;
        }

        var currentPath = string.Empty;

        for (var i = 0; i < segments.Length; i++)
        {
            var requestedName = segments[i];
            var isDirectory = i < segments.Length - 1 || finalSegmentIsDirectory;
            var entry = FindMatchingEntry(requestedName, () => entriesFactory(currentPath), out collision);

            if (collision || entry is null)
            {
                return subpath;
            }

            if (entry.IsDirectory != isDirectory)
            {
                return subpath;
            }

            segments[i] = entry.Name;
            currentPath = currentPath.Length == 0 ? entry.Name : $"{currentPath}/{entry.Name}";
        }

        var resolvedPath = string.Join(Path.DirectorySeparatorChar.ToString(), segments);
        cache.TryAdd(subpath, resolvedPath);
        return resolvedPath;
    }

    private static IFileInfo FindMatchingEntry(string requestedName, Func<IEnumerable<IFileInfo>> entriesFactory, out bool collision)
    {
        collision = false;

        try
        {
            IFileInfo caseInsensitiveMatch = null;

            foreach (var entry in entriesFactory())
            {
                if (!string.Equals(entry.Name, requestedName, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (caseInsensitiveMatch is not null)
                {
                    collision = true;
                    return null;
                }

                caseInsensitiveMatch = entry;
            }

            return caseInsensitiveMatch;
        }
        catch (Exception ex) when (ex is ArgumentException or DirectoryNotFoundException or IOException or SecurityException or UnauthorizedAccessException)
        {
            return null;
        }
    }
}
