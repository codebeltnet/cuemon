---
uid: Cuemon.Extensions.Globalization.RegionInfoExtensions
example:
- *content
---

The following example demonstrates how to use RegionInfoExtensions to retrieve the cultures associated with a specific geographic region.

```csharp
using System;
using System.Globalization;
using System.Linq;
using Cuemon.Extensions.Globalization;

namespace MyApp.Globalization
{
    public class RegionInfoExtensionsExample
    {
        public void Demonstrate()
        {
            // Get cultures associated with a specific region
            var region = new RegionInfo("US");

            var cultures = region.GetCultures().ToList();
            Console.WriteLine($"Cultures for {region.EnglishName} ({region.TwoLetterISORegionName}):");
            foreach (var culture in cultures)
            {
                Console.WriteLine($"  {culture.Name} - {culture.EnglishName}");
            }

            // Try another region
            var japan = new RegionInfo("JP");
            var jpCultures = japan.GetCultures().ToList();
            Console.WriteLine($"\nCultures for {japan.EnglishName}:");
            foreach (var culture in jpCultures)
            {
                Console.WriteLine($"  {culture.Name} - {culture.EnglishName}");
            }
        }
    }
}
```
