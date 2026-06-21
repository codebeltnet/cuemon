---
uid: Cuemon.Globalization.StatisticalRegionKind
example:
- *content
---

The following example demonstrates how to use <see cref="Cuemon.Globalization.StatisticalRegionKind"/> to categorize geographic regions according to the UN M.49 standard.

```csharp
using System;
using Cuemon.Globalization;

namespace MyApp.Examples;

public class StatisticalRegionKindExample
{
    public void Demonstrate()
    {
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

}}
}

```
