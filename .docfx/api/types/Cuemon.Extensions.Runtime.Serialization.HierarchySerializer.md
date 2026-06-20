---
uid: Cuemon.Extensions.Runtime.Serialization.HierarchySerializer
example:
- *content
---

`HierarchySerializer` converts any object graph into a hierarchical node structure (`IHierarchy<DataPair>`) for inspection and path-based representation. This example creates a `ReportRoot` with a `Name` of `"alpha"` and a `ReportChild` with `Count = 7`, passes it to the `HierarchySerializer` constructor, and accesses the root node's instance type name, checks whether it has children, and prints the tree path representation via `ToString()`. Key steps include constructing a serializer from a plain object and reading the resulting node properties. Console output displays the `ReportRoot` type name, `True` for `HasChildren`, and a path-based tree showing `ReportRoot > ReportChild`.

```csharp
using System;
using Cuemon.Extensions.Runtime.Serialization;

namespace MyApp.Examples;

public static class HierarchySerializerExample
{
    private sealed class ReportRoot
    {
        public string Name { get; set; } = string.Empty;

        public ReportChild Child { get; set; } = new ReportChild();
    }

    private sealed class ReportChild
    {
        public int Count { get; set; }
    }

    public static void Demonstrate()
    {
        var serializer = new HierarchySerializer(new ReportRoot
        {
            Name = "alpha",
            Child = new ReportChild { Count = 7 }
        });

        Console.WriteLine(serializer.Nodes.InstanceType.Name);
        Console.WriteLine(serializer.Nodes.HasChildren);
        Console.WriteLine(serializer.ToString());
    }
}

```
