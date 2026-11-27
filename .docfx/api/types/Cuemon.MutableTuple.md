---
uid: Cuemon.MutableTuple
example:
- *content
---

The following example demonstrates how to create and use <xref:Cuemon.MutableTuple> and its generic variants to store and pass multiple values without creating a custom class.

```csharp
// Program.cs
using System;
using Cuemon;

MutableTuple empty = new MutableTuple();
var pair = new MutableTuple<string, int>("Alice", 30);

pair.Arg2 = 31;

object[] values = pair.ToArray("verified");
var clone = (MutableTuple<string, int>)pair.Clone();

Console.WriteLine($"Empty: {empty.IsEmpty}");
Console.WriteLine($"{clone.Arg1}:{clone.Arg2}");
Console.WriteLine($"Array length: {values.Length}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="d1ba84fca57818397fad30ce"><a href="~/run-actions/d1ba84fca57818397fad30ce/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/d1ba84fca57818397fad30ce/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
