---
uid: Cuemon.Extensions.Runtime.HierarchyOptions
example:
- *content
---

The following example demonstrates how to configure <see cref="Cuemon.Extensions.Runtime.HierarchyOptions"/> to control the depth of object hierarchy traversal and skip specific property types.

```csharp
using System;
using System.Linq;
using Cuemon.Extensions.Runtime;

namespace MyApp.Examples;

public class HierarchyOptionsExample
{
    public void Demonstrate()
    {
        // Direct instantiation of HierarchyOptions
        var hierarchyOptions = new HierarchyOptions
        {
            MaxDepth = 2,
            SkipPropertyType = t => t == typeof(string) || t.IsValueType
        };

        var source = new
        {
            Name = "Root",
            Value = 42,
            Nested = new
            {
                Deep = new
                {
                    Deeper = "found"
                }
            }
        };

        // Limit depth to 2 and skip string types
        var hierarchy = Hierarchy.GetObjectHierarchy(source, o =>
        {
            o.MaxDepth = 2;
            o.SkipPropertyType = t => t == typeof(string) || t.IsValueType;
        });

        var root = hierarchy;
        Console.WriteLine($"Root type: {root.InstanceType.Name}"); // Anonymous type

        var children = root.GetChildren().ToList();
        Console.WriteLine($"Children count: {children.Count}"); // 0 (all primitive types skipped)
    }
}
```
