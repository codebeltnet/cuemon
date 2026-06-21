---
uid: Cuemon.Xml.HierarchyDecoratorExtensions
example:
- *content
---

`HierarchyDecoratorExtensions` provides extension methods on `Decorator.Enclose` for inspecting XML serialization metadata on `IHierarchy<object>` nodes. This example creates a `Hierarchy<object>` tree with a `Person` node (annotated with `[XmlRoot("person")]`) and an `Address` child node, then uses decorator methods to call `GetXmlQualifiedEntity`, `TryGetXmlRootAttribute`, `TryGetXmlElementAttribute`, `TryGetXmlTextAttribute`, `TryGetXmlAttributeAttribute`, `IsNodeEnumerable`, `HasXmlIgnoreAttribute`, and `OrderByXmlAttributes`. Console output displays the qualified entity local name, boolean results for attribute detection, and the count of ordered nodes.

```csharp
using System;
using Cuemon;
using Cuemon.Extensions.Runtime;
using Cuemon.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace MyApp.Examples;

public static class HierarchyDecoratorExtensionsExample
{
    public static void Demonstrate()
    {
        var hierarchy = new Hierarchy<object>();
        var root = hierarchy.Add(new Person { Name = "Alice" });
        var child = root.Add(new Address { City = "Paris" });

        var decorator = Decorator.Enclose((IHierarchy<object>)child);
        var rootDecorator = Decorator.Enclose((IHierarchy<object>)root);
        var qualifiedEntity = decorator.GetXmlQualifiedEntity();
        var hasRoot = rootDecorator.TryGetXmlRootAttribute(out _);
        var hasElement = decorator.TryGetXmlElementAttribute(out _);
        var hasText = decorator.TryGetXmlTextAttribute(out _);
        var hasAttribute = decorator.TryGetXmlAttributeAttribute(out _);
        var ordered = Decorator.Enclose((IEnumerable<IHierarchy<object>>)new[] { root, child }).OrderByXmlAttributes<object>().ToList();

        Console.WriteLine(qualifiedEntity.LocalName);
        Console.WriteLine(hasRoot);
        Console.WriteLine(hasElement || hasText || hasAttribute);
        Console.WriteLine(decorator.IsNodeEnumerable());
        Console.WriteLine(decorator.HasXmlIgnoreAttribute());
        Console.WriteLine(ordered.Count);
    }

    [XmlRoot("person")]
    private sealed class Person
    {
        public string Name { get; set; }
    }

    private sealed class Address
    {
        public string City { get; set; }
    }
}

```
