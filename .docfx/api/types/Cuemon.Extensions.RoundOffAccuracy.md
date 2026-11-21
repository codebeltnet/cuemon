---
uid: Cuemon.Extensions.RoundOffAccuracy
example:
- *content
---

The following example demonstrates how to use the <see cref="Cuemon.Extensions.RoundOffAccuracy"/> enum with the <see cref="M:Cuemon.Extensions.DoubleExtensions.RoundOff(System.Double,Cuemon.Extensions.RoundOffAccuracy)"/> extension method to round double values to the nearest specified accuracy.

```csharp
// Program.cs
using System;
using Cuemon.Extensions;

double value = 123456789.987654321d;

Console.WriteLine(value.RoundOff(RoundOffAccuracy.NearestTenth));
Console.WriteLine(value.RoundOff(RoundOffAccuracy.NearestHundredth));
Console.WriteLine(value.RoundOff(RoundOffAccuracy.NearestThousandth));
Console.WriteLine(value.RoundOff(RoundOffAccuracy.NearestMillion));
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="5abbe52bdee71d13d9595e91"><a href="~/run-actions/5abbe52bdee71d13d9595e91/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/5abbe52bdee71d13d9595e91/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
