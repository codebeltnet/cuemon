---
uid: Cuemon.Extensions.VerticalDirection
example:
- *content
---

The following example demonstrates how to use the <xref:Cuemon.Extensions.VerticalDirection> enum to indicate vertical positioning.

```csharp
// Program.cs
using System;
using Cuemon.Extensions;

var upward = VerticalDirection.Up;
var downward = (VerticalDirection)Enum.Parse(typeof(VerticalDirection), "Down");

Console.WriteLine($"{upward} = {(int)upward}");
Console.WriteLine($"{downward} = {(int)downward}");
Console.WriteLine(default(VerticalDirection));
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="30451220407512250e37b6a5"><a href="~/run-actions/30451220407512250e37b6a5/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/30451220407512250e37b6a5/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
