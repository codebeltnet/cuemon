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

// Limit depth to 1 and skip the Value property
var options = new HierarchyOptions
{
    MaxDepth = 1,
    SkipProperty = p => p.Name == "Value"
};

var hierarchy = Hierarchy.GetObjectHierarchy(source, o =>
{
    o.MaxDepth = options.MaxDepth;
    o.SkipProperty = options.SkipProperty;
});

var root = hierarchy;
Console.WriteLine($"Root type: {root.InstanceType.Name}"); // Anonymous type

var children = root.GetChildren().ToList();
Console.WriteLine($"Children count: {children.Count}"); // 2 (Name and Nested; Value skipped)
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="41cbeb89779372c477fdc3b0"><a href="~/run-actions/41cbeb89779372c477fdc3b0/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/41cbeb89779372c477fdc3b0/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
