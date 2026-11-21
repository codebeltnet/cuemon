---
uid: Cuemon.Extensions.Runtime.Hierarchy
example:
- *content
---

The following example demonstrates how to build and traverse a hierarchical tree structure using `Hierarchy`. It shows adding nodes, printing paths, searching for nodes with `Find`, and generating an object hierarchy from an anonymous type.

```csharp
// Program.cs
using System;
using System.Linq;
using Cuemon.Extensions.Runtime;

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
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="0935da97633b9761817c0697"><a href="~/run-actions/0935da97633b9761817c0697/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/0935da97633b9761817c0697/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
