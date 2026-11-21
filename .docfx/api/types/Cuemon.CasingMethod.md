---
uid: Cuemon.CasingMethod
example:
- *content
---

The following example demonstrates how to use `CasingMethod` to control string casing transformations.

```csharp
// Program.cs
using Cuemon;
using System;

var lower = CasingMethod.LowerCase;
var upper = CasingMethod.UpperCase;
var title = CasingMethod.TitleCase;

Console.WriteLine(lower); // outputs: LowerCase
Console.WriteLine(upper); // outputs: UpperCase
Console.WriteLine(title); // outputs: TitleCase
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="77cfc4d2b1b9055ef3b01218"><a href="~/run-actions/77cfc4d2b1b9055ef3b01218/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/77cfc4d2b1b9055ef3b01218/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
