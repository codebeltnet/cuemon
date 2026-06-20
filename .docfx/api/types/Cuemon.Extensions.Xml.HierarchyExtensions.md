---
uid: Cuemon.Extensions.Xml.HierarchyExtensions
example:
- *content
---

`HierarchyExtensions` provides extension methods for inspecting XML serialization metadata on hierarchy nodes, including qualified entity names, enumerable detection, and XML ignore and ordering attributes. This example creates `HierarchySerializer` instances for `CatalogDocument` (with `[XmlAttribute]` on `Id` and a `List<string> Tags` property) and `IgnoredDocument` (with `[XmlIgnore]` on `Hidden`), then calls `GetXmlQualifiedEntity`, `IsNodeEnumerable`, `HasXmlIgnoreAttribute`, and `OrderByXmlAttributes` on individual nodes. Console output shows the qualified entity local name, whether the node is enumerable, whether it has `[XmlIgnore]`, and the reordered element order.

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Cuemon.Extensions.Runtime.Serialization;
using Cuemon.Extensions.Xml;
using Cuemon.Xml.Serialization;

namespace DocExamples;

public static class HierarchyExtensionsExample
{
    public static void Demonstrate()
    {
        var documentNodes = new HierarchySerializer(new CatalogDocument()).Nodes.GetChildren().ToList();
        var idNode = documentNodes.Single(node => node.MemberReference?.Name == nameof(CatalogDocument.Id));
        var tagsNode = documentNodes.Single(node => node.MemberReference?.Name == nameof(CatalogDocument.Tags));

        var ignoredNodes = new HierarchySerializer(new IgnoredDocument()).Nodes.GetChildren().ToList();
        var hiddenNode = ignoredNodes.Single(node => node.MemberReference?.Name == nameof(IgnoredDocument.Hidden));
        var overrideEntity = new XmlQualifiedEntity("Override");

        Console.WriteLine(idNode.GetXmlQualifiedEntity().LocalName);
        Console.WriteLine(idNode.GetXmlQualifiedEntity(overrideEntity).LocalName);
        Console.WriteLine(tagsNode.IsNodeEnumerable());
        Console.WriteLine(hiddenNode.HasXmlIgnoreAttribute());

        var reordered = new[] { tagsNode, idNode }.OrderByXmlAttributes().ToList();
        Console.WriteLine(reordered[0].MemberReference?.Name);
    }

    private sealed class CatalogDocument
    {
        [XmlAttribute]
        public Guid Id { get; } = Guid.Empty;

        public List<string> Tags { get; } = new() { "xml", "docfx" };
    }

    private sealed class IgnoredDocument
    {
        [XmlIgnore]
        public string Hidden => "internal";

        public string Visible => "public";
    }
}
```
