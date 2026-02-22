using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Cuemon.Collections.Generic;

namespace Cuemon.Globalization
{
    /// <summary>
    /// This static class is designed to make <see cref="System.Globalization"/> operations easier to work with.
    /// </summary>
    public static class World
    {
        internal static readonly Lazy<IEnumerable<CultureInfo>> SpecificCultures = new(() =>
        {
            var cultures = new SortedList<string, CultureInfo>();
            var specificCultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            foreach (var c in specificCultures.Where(ci => ci.LCID != 127)) // *should* not happen for specific cultures, but on some linux based systems there are some cultures with LCID 127 (invariant culture) that are incorrectly categorized as specific cultures, so we need to filter those out.
            {
                Decorator.Enclose(cultures).TryAdd(c.Name, c);
            }
            return cultures.Values;
        });

        private static readonly Lazy<IEnumerable<RegionInfo>> SpecificRegions = new(() =>
        {
            var regions = new SortedList<string, RegionInfo>(StringComparer.OrdinalIgnoreCase);
            foreach (var c in SpecificCultures.Value)
            {
                var region = new RegionInfo(c.Name);
                if (int.TryParse(region.Name, out _)) { continue; } // Skip statistical regions (why would Microsoft even consider having these as part of RegionInfo? No valid ISO 3166-1 alpha-2/3 code can be all digits, so these are not actual regions or countries.)
                Decorator.Enclose(regions).TryAdd(c.Name, region);
            }
            return regions.Values.OrderBy(info => info.Name);
        });

        private static readonly Lazy<UnM49DataContainer> UnM49Data = new(() => new UnM49DataContainer());

        /// <summary>
        /// Gets the by .NET specific regions of the world.
        /// </summary>
        /// <value>The .NET specific regions of the world.</value>
        public static IEnumerable<RegionInfo> Regions { get; } = SpecificRegions.Value;

        /// <summary>
        /// Gets all UN M.49 geographic regions.
        /// </summary>
        /// <value>A sequence of all <see cref="StatisticalRegionInfo"/> instances.</value>
        /// <remarks>
        /// The sequence includes the World region (code "001") and all geographic regions.
        /// </remarks>
        public static IEnumerable<StatisticalRegionInfo> StatisticalRegions { get; } = UnM49Data.Value.Regions;

        /// <summary>
        /// Gets a region or country by its UN M.49 code.
        /// </summary>
        /// <param name="code">The three-digit UN M.49 code (e.g., "001" for World, "840" for United States).</param>
        /// <returns>A <see cref="StatisticalRegionInfo"/> instance, or <c>null</c> if the code is not found.</returns>
        public static StatisticalRegionInfo GetStatisticalRegion(string code)
        {
            if (string.IsNullOrEmpty(code)) return null;
            UnM49Data.Value.RegionsByCode.TryGetValue(code, out var region);
            return region;
        }

        /// <summary>
        /// Gets a country by its UN M.49 code.
        /// </summary>
        /// <param name="m49Code">The three-digit UN M.49 country code (e.g., "840" for United States).</param>
        /// <returns>A <see cref="StatisticalRegionInfo"/> instance with <see cref="StatisticalRegionKind.CountryOrTerritory"/>, or <c>null</c> if the code is not found.</returns>
        public static StatisticalRegionInfo GetCountry(string m49Code)
        {
            if (string.IsNullOrEmpty(m49Code)) return null;
            UnM49Data.Value.CountriesByCode.TryGetValue(m49Code, out var country);
            return country;
        }

        /// <summary>
        /// Gets the UN M.49 country information for the specified <see cref="RegionInfo"/>.
        /// </summary>
        /// <param name="region">The .NET region information.</param>
        /// <returns>A <see cref="StatisticalRegionInfo"/> instance, or <c>null</c> if no mapping exists.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="region"/> is <c>null</c>.</exception>
        public static StatisticalRegionInfo GetCountry(RegionInfo region)
        {
            Validator.ThrowIfNull(region);
            UnM49Data.Value.CountriesByIsoAlpha2.TryGetValue(region.TwoLetterISORegionName, out var country);
            return country;
        }

        /// <summary>
        /// Resolves a sequence of related <see cref="CultureInfo"/> objects for the specified <paramref name="region"/>.
        /// </summary>
        /// <param name="region">The region to resolve a sequence of <see cref="CultureInfo"/> objects from.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> sequence of <see cref="CultureInfo"/> objects.</returns>
        public static IEnumerable<CultureInfo> GetCultures(RegionInfo region)
        {
            Validator.ThrowIfNull(region);
            return SpecificCultures.Value.Where(c => c.Name.EndsWith(region.TwoLetterISORegionName, StringComparison.Ordinal));
        }
    }
}
