---
uid: Cuemon.Globalization.World
example:
- *content
---

The following example demonstrates how to query global regions, statistical regions, and cultures using the `World` class. It prints region counts, looks up the United States by M.49 code, and enumerates cultures for a specific region.

```csharp
using System;
using System.Globalization;
using System.Linq;
using Cuemon.Globalization;

namespace Cuemon.Globalization;

public class WorldExample
{
    public void Demonstrate()
    {
        var regions = World.Regions.ToList();
        Console.WriteLine($"Number of regions: {regions.Count}");

        var statisticalRegions = World.StatisticalRegions.ToList();
        Console.WriteLine($"Statistical regions: {statisticalRegions.Count}");

        var country = World.GetStatisticalRegion("840");
        Console.WriteLine($"United States M.49: {country?.Name}");

        var cultures = World.GetCultures(new RegionInfo("US"));
        foreach (var culture in cultures)
        {
            Console.WriteLine($"Culture: {culture.DisplayName}");
        }
    }
}
```
