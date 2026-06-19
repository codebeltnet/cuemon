---
uid: Cuemon.Extensions.Runtime.Serialization.HierarchySerializer
example:
- *content
---

The following example demonstrates how to use <see cref="Cuemon.Extensions.Runtime.Serialization.HierarchySerializer"/> to convert any object graph into a hierarchical node structure and display its path-based tree representation.

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
