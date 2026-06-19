---
uid: Cuemon.Extensions.Runtime.HierarchyDecoratorExtensions
example:
- *content
---

The following example demonstrates how to use the <xref:Cuemon.Extensions.Runtime.HierarchyDecoratorExtensions> to navigate a hierarchy, replace matching nodes, and materialize typed values from <xref:Cuemon.DataPair> nodes.

```csharp
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cuemon;
using Cuemon.Extensions.Runtime;

namespace MyApp.Examples;

public static class HierarchyDecoratorExtensionsExample
{
    public static void Demonstrate()
    {
        var root = BuildStringHierarchy(out var childOne, out var grandchild, out _);

        var rootNode = Decorator.Enclose(grandchild).Root();
        var ancestors = Decorator.Enclose(grandchild).AncestorsAndSelf().Select(node => node.Instance).ToArray();
        var descendants = Decorator.Enclose(root).DescendantsAndSelf().Select(node => node.Instance).ToArray();
        var siblings = Decorator.Enclose(childOne).SiblingsAndSelf().Select(node => node.Instance).ToArray();
        var nodesAtDepth = Decorator.Enclose(grandchild).SiblingsAndSelfAt(1).Select(node => node.Instance).ToArray();
        var flattened = Decorator.Enclose(childOne).FlattenAll().Select(node => node.Instance).ToArray();
        var firstChildName = Decorator.Enclose(root).FindFirstInstance(node => node.Instance.StartsWith("child", StringComparison.Ordinal));
        var grandchildName = Decorator.Enclose(root).FindSingleInstance(node => node.Instance == "grandchild");
        var firstChildNode = Decorator.Enclose(root).FindFirst(node => node.Depth == 1);
        var grandchildNode = Decorator.Enclose(root).FindSingle(node => node.Instance == "grandchild");
        var childNames = Decorator.Enclose(root).FindInstance(node => node.Depth == 1).OrderBy(name => name).ToArray();
        var childNodes = Decorator.Enclose(root).Find(node => node.Depth == 1).ToArray();
        var indexedNode = Decorator.Enclose(root).NodeAt(2);

        Decorator.Enclose(grandchild).Replace((node, value) => node.Replace(value.ToUpperInvariant()));
        Decorator.Enclose(Decorator.Enclose(root).Find(node => node.Depth == 1)).ReplaceAll((node, value) => node.Replace(value.ToUpperInvariant()));

        var integerNode = BuildDataPairHierarchy(new DataPair(typeof(int).Name, "42", typeof(string)));
        var timestamp = new DateTime(2024, 1, 2, 3, 4, 5, DateTimeKind.Utc);
        var dateTimeNode = BuildDataPairHierarchy(new DataPair("When", timestamp, typeof(DateTime)));
        var guid = Guid.Parse("11111111-2222-3333-4444-555555555555");
        var guidNode = BuildDataPairHierarchy(new DataPair("Value", guid.ToString("D"), typeof(string)));
        var stringNode = BuildDataPairHierarchy(new DataPair("Text", "hello", typeof(string)));
        var decimalNode = BuildDataPairHierarchy(new DataPair("Amount", "42.5", typeof(string)));
        var uri = new Uri("https://example.com/path?value=42", UriKind.Absolute);
        var uriNode = BuildDataPairHierarchy(new DataPair("OriginalString", uri.OriginalString, typeof(string)));

        var typedValues = new object[]
        {
            Decorator.Enclose(integerNode).UseConvertibleFormatter(),
            Decorator.Enclose(dateTimeNode).UseDateTimeFormatter(),
            Decorator.Enclose(guidNode).UseGuidFormatter(),
            Decorator.Enclose(stringNode).UseStringFormatter(),
            Decorator.Enclose(decimalNode).UseDecimalFormatter(),
            Decorator.Enclose(uriNode).UseUriFormatter()
        };

        ICollection prices = Decorator.Enclose(BuildCollectionHierarchy(typeof(decimal), "42.5", "84.0")).UseCollection(typeof(decimal));
        IDictionary milestones = Decorator.Enclose(BuildDictionaryHierarchy(
            typeof(DateTime),
            new KeyValuePair<string, object>("created", timestamp),
            new KeyValuePair<string, object>("updated", timestamp.AddHours(2))))
            .UseDictionary(new[] { typeof(string), typeof(DateTime) });

        Console.WriteLine(rootNode.Instance);
        Console.WriteLine(string.Join(" > ", ancestors));
        Console.WriteLine(string.Join(", ", descendants));
        Console.WriteLine(string.Join(", ", siblings));
        Console.WriteLine(string.Join(", ", nodesAtDepth));
        Console.WriteLine(string.Join(", ", flattened));
        Console.WriteLine(firstChildName);
        Console.WriteLine(grandchildName);
        Console.WriteLine(firstChildNode.Instance);
        Console.WriteLine(grandchildNode.Instance);
        Console.WriteLine(string.Join(", ", childNames));
        Console.WriteLine(childNodes.Length);
        Console.WriteLine(indexedNode.Instance);
        Console.WriteLine(grandchild.Instance);
        Console.WriteLine(string.Join(", ", root.GetChildren().Select(node => node.Instance)));
        Console.WriteLine(string.Join(", ", typedValues));
        Console.WriteLine(string.Join(", ", prices.Cast<decimal>()));
        Console.WriteLine(string.Join(", ", milestones.Keys.Cast<string>()));
    }

    private static Hierarchy<string> BuildStringHierarchy(out IHierarchy<string> childOne, out IHierarchy<string> grandchild, out IHierarchy<string> childTwo)
    {
        var root = new Hierarchy<string>();
        root.Add("root");
        childOne = root.Add("child-one");
        grandchild = childOne.Add("grandchild");
        childTwo = root.Add("child-two");
        return root;
    }

    private static IHierarchy<DataPair> BuildDataPairHierarchy(DataPair pair)
    {
        var hierarchy = new Hierarchy<DataPair>();
        hierarchy.Add(pair);
        return hierarchy;
    }

    private static IHierarchy<DataPair> BuildCollectionHierarchy(Type valueType, params object[] values)
    {
        var hierarchy = new Hierarchy<DataPair>();
        hierarchy.Add(new DataPair("Items", null, typeof(List<object>)));
        foreach (var value in values)
        {
            hierarchy.Add(CreateValuePair(valueType, value));
        }

        return hierarchy;
    }

    private static IHierarchy<DataPair> BuildDictionaryHierarchy(Type valueType, params KeyValuePair<string, object>[] values)
    {
        var hierarchy = new Hierarchy<DataPair>();
        hierarchy.Add(new DataPair("Entries", null, typeof(Dictionary<string, object>)));
        foreach (var value in values)
        {
            var keyNode = hierarchy.Add(new DataPair("Key", value.Key, typeof(string)));
            keyNode.Add(CreateValuePair(valueType, value.Value));
        }

        return hierarchy;
    }

    private static DataPair CreateValuePair(Type valueType, object value)
    {
        if (valueType.IsPrimitive)
        {
            return new DataPair(valueType.Name, value, value.GetType());
        }

        if (valueType == typeof(Uri))
        {
            return new DataPair("OriginalString", value, typeof(string));
        }

        if (valueType == typeof(DateTime))
        {
            return new DataPair("When", value, typeof(DateTime));
        }

        return new DataPair("Value", value, value?.GetType() ?? typeof(object));
    }
}

```
