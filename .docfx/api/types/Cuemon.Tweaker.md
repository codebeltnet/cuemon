---
uid: Cuemon.Tweaker
example:
- *content
---

The following example demonstrates how to use `Tweaker` to apply inline transformations on values and objects: adjusting with a converter, altering in-place, and changing between types.

```csharp
// Program.cs
using System;
using System.Collections.Generic;
using Cuemon;

// Adjust: transform a value using a converter function
int doubled = Tweaker.Adjust(21, x => x * 2);
Console.WriteLine(doubled); // 42

// Alter: modify an object in-place via an action delegate
var list = new List<string> { "a", "b", "c" };
Tweaker.Alter(list, lst => lst.Add("d"));
Console.WriteLine(string.Join(", ", list)); // a, b, c, d

// Change: convert a value to a different type
string asString = Tweaker.Change(42, x => x.ToString());
Console.WriteLine(asString); // 42

// Adjust with null converter returns the original value unchanged
int same = Tweaker.Adjust(10, null as Func<int, int>);
Console.WriteLine(same); // 10
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="d339a86d1317c4cb71bbbdf1"><a href="~/run-actions/d339a86d1317c4cb71bbbdf1/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/d339a86d1317c4cb71bbbdf1/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
