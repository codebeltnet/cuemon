---
uid: Cuemon.Globalization.StatisticalRegionKind
example:
- *content
---

The following example demonstrates how to use <see cref="Cuemon.Globalization.StatisticalRegionKind"/> to categorize geographic regions according to the UN M.49 standard.

```csharp
// Program.cs
using System;
using Cuemon.Globalization;

var regionKind = StatisticalRegionKind.CountryOrTerritory;

switch (regionKind)
{
    case StatisticalRegionKind.World:
        Console.WriteLine("The entire world (code 001).");
        break;
    case StatisticalRegionKind.Region:
        Console.WriteLine("A major geographic region or continent.");
        break;
    case StatisticalRegionKind.Subregion:
        Console.WriteLine("A subdivision of a region (e.g., Western Europe).");
        break;
    case StatisticalRegionKind.IntermediateRegion:
        Console.WriteLine("An intermediate grouping of subregions.");
        break;
    case StatisticalRegionKind.CountryOrTerritory:
        Console.WriteLine("An individual country or territory (leaf node).");
        break;

}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="4d6879e5532e04fd034ba23e"><a href="~/run-actions/4d6879e5532e04fd034ba23e/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/4d6879e5532e04fd034ba23e/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
