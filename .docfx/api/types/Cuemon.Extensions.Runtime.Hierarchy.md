---
uid: Cuemon.Extensions.Runtime.Hierarchy
example:
- *content
---

```csharp
using System;
using System.Linq;
using Cuemon.Extensions.Runtime;

namespace Cuemon.Extensions.Runtime;

public class HierarchyExample
{
    public void Demonstrate()
    {
        var root = new Hierarchy<string>();
        root.Add("root");

        var child1 = root.Add("department");
        var child2 = root.Add("team");

        child1.Add("employee1");
        child1.Add("employee2");

        Console.WriteLine(root.GetPath());
        Console.WriteLine(child1.GetPath(n => n.Instance.ToString().ToUpper()));

        var matches = Hierarchy.Find(root, n => n.Instance.ToString().StartsWith("employee"));
        Console.WriteLine($"Found {matches.Count()} node(s)");

        var obj = new { Name = "Root", Items = new[] { new { Id = 1 }, new { Id = 2 } } };
        IHierarchy<object> tree = Hierarchy.GetObjectHierarchy(obj);
        Console.WriteLine(tree.GetPath(n => n.InstanceType.Name));
    }
}
```
