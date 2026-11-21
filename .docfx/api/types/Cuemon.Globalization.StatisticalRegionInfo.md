---
uid: Cuemon.Globalization.StatisticalRegionInfo
example:
- *content
---

The following example demonstrates how to use <see cref="Cuemon.Globalization.StatisticalRegionInfo"/> via the <see cref="World"/> class to retrieve UN M.49 region data.

```csharp
// Program.cs
using System;
using System.Linq;
using Cuemon.Globalization;

// Get United States by its UN M.49 code
StatisticalRegionInfo usa = World.GetCountry("840");

if (usa != null)
{
    Console.WriteLine($"Name: {usa.Name}");
    Console.WriteLine($"Code: {usa.Code}");
    Console.WriteLine($"ISO Alpha-2: {usa.IsoAlpha2}");
    Console.WriteLine($"ISO Alpha-3: {usa.IsoAlpha3}");
    Console.WriteLine($"Kind: {usa.Kind}");
    Console.WriteLine($"Parent: {usa.Parent?.Name}");

// List all European countries
var europe = World.GetStatisticalRegion("150");
if (europe != null)
{
    Console.WriteLine($"\nCountries in {europe.Name}:");
    foreach (var country in europe.Countries.Take(5))
    {
        Console.WriteLine($" - {country.Name} ({country.IsoAlpha2})");

}}}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="55d19b03dfe9fd6016b62ad0"><a href="~/run-actions/55d19b03dfe9fd6016b62ad0/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/55d19b03dfe9fd6016b62ad0/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
