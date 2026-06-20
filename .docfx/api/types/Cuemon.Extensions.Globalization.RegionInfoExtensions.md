---
uid: Cuemon.Extensions.Globalization.RegionInfoExtensions
example:
- *content
---

`RegionInfoExtensions` provides extension methods for `RegionInfo` to enumerate all cultures associated with a geographic region via the `GetCultures` method. This example creates `RegionInfo` instances for `"US"` and `"JP"`, then calls `GetCultures()` on each to retrieve their associated culture collections. Key steps include iterating the culture results and printing the culture name and English name for each. Console output lists cultures such as `en-US` for the US and `ja-JP` for Japan, along with their English names.

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
