---
uid: Cuemon.Globalization.StatisticalRegionInfo
example:
- *content
---

The following example demonstrates how to use <see cref="Cuemon.Globalization.StatisticalRegionInfo"/> via the <see cref="World"/> class to retrieve UN M.49 region data.

```csharp
using System;
using System.Linq;
using Cuemon.Globalization;

namespace MyApp.Examples;

public class StatisticalRegionInfoExample
{
    public void Demonstrate()
    {
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

}}}}
}

```
