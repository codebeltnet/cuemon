---
uid: Cuemon.Extensions.Globalization.StatisticalRegionExtensions
example:
- *content
---

The following example demonstrates how to use the <xref:Cuemon.Extensions.Globalization.StatisticalRegionExtensions> to classify geographic regions using the UN M.49 standard.

```csharp
using System;
using System.Globalization;
using System.Linq;
using Cuemon.Extensions.Globalization;
using Cuemon.Globalization;

namespace MyApp.Examples;

public class Example
{
    public void Run()
    {

        // Access regions via the World class
        var world = World.GetStatisticalRegion("001");
        var europe = World.GetStatisticalRegion("150");
        var westernEurope = World.GetStatisticalRegion("155");
        var denmark = World.GetCountry("208");     // Denmark M.49 code
        var usa = World.GetCountry("840");         // United States M.49 code

        // Classify by kind using the extension methods
        bool isWorld = world.IsWorld();                       // true
        bool isRegion = europe.IsRegion();                    // true (continent)
        bool isSubregion = westernEurope.IsSubregion();       // true
        bool isCountry = denmark.IsCountryOrTerritory();      // true
        bool isArea = europe.IsArea();                        // true (not a country)
        bool isCountryArea = denmark.IsArea();                // false (is a country)

        // Check intermediate regions (sub-Saharan Africa, Latin America)
        var subSaharanAfrica = World.GetStatisticalRegion("202");
        bool isIntermediate = subSaharanAfrica.IsIntermediateRegion(); // true

        // Verify hierarchy
        bool denmarkIsInEurope = denmark.Parent.IsSubregion(); // true (Northern Europe, M.49: 154)

        // Check if a country has associated .NET RegionInfo
        bool hasRegionInfo = usa.HasRegionInfo();  // true (US has RegionInfo)
        bool noRegionInfo = world.HasRegionInfo(); // false (World is not a country)

        // Check if a region has ISO codes
        bool hasIso = denmark.HasIsoCodes(); // true

        // Iterate all countries
        int countryCount = world.Countries.Count();
        Console.WriteLine($"Total countries/territories: {countryCount}");

        // Find a country by RegionInfo
        var regionInfo = new RegionInfo("US");
        var usaByRegion = World.GetCountry(regionInfo);
        Console.WriteLine(usaByRegion.IsCountryOrTerritory()); // True

        // Traverse the hierarchy
        foreach (var ancestor in denmark.GetAncestors())
        {
            Console.WriteLine($"{ancestor.Name} ({ancestor.Kind})");
        // "Northern Europe (Subregion)"
        // "Europe (Region)"
        // "World (World)"

}}
}

```
