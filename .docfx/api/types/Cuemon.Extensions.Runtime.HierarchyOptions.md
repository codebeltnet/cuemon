---
uid: Cuemon.Extensions.Runtime.HierarchyOptions
example:
- *content
---

The following example demonstrates how to configure <see cref="Cuemon.Extensions.Runtime.HierarchyOptions"/> to control the depth of object hierarchy traversal and skip specific property types.

```csharp
// Program.cs
using System;
using System.Linq;
using Cuemon.Extensions.Runtime;

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
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="de55cb16d8fff5f20a41a219"><a href="~/run-actions/de55cb16d8fff5f20a41a219/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/de55cb16d8fff5f20a41a219/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
