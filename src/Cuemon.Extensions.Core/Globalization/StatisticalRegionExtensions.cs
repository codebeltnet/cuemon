using System;
using Cuemon.Globalization;

namespace Cuemon.Extensions.Globalization;
/// <summary>
/// Provides extension methods for <see cref="StatisticalRegionInfo"/>.
/// </summary>
public static class StatisticalRegionExtensions
{
    /// <summary>
    /// Determines whether the specified region is the World region.
    /// </summary>
    /// <param name="region">The region to check.</param>
    /// <returns><c>true</c> if the region is the World region; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="region"/> is <c>null</c>.</exception>
    public static bool IsWorld(this StatisticalRegionInfo region)
    {
        Validator.ThrowIfNull(region);
        return region.Kind == StatisticalRegionKind.World;
    }

    /// <summary>
    /// Determines whether the specified region is a geographic region (continent or major area).
    /// </summary>
    /// <param name="region">The region to check.</param>
    /// <returns><c>true</c> if the region is a Region; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="region"/> is <c>null</c>.</exception>
    public static bool IsRegion(this StatisticalRegionInfo region)
    {
        Validator.ThrowIfNull(region);
        return region.Kind == StatisticalRegionKind.Region;
    }

    /// <summary>
    /// Determines whether the specified region is a subregion.
    /// </summary>
    /// <param name="region">The region to check.</param>
    /// <returns><c>true</c> if the region is a Subregion; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="region"/> is <c>null</c>.</exception>
    public static bool IsSubregion(this StatisticalRegionInfo region)
    {
        Validator.ThrowIfNull(region);
        return region.Kind == StatisticalRegionKind.Subregion;
    }

    /// <summary>
    /// Determines whether the specified region is an intermediate region.
    /// </summary>
    /// <param name="region">The region to check.</param>
    /// <returns><c>true</c> if the region is an IntermediateRegion; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="region"/> is <c>null</c>.</exception>
    public static bool IsIntermediateRegion(this StatisticalRegionInfo region)
    {
        Validator.ThrowIfNull(region);
        return region.Kind == StatisticalRegionKind.IntermediateRegion;
    }

    /// <summary>
    /// Determines whether the specified region is a country or territory.
    /// </summary>
    /// <param name="region">The region to check.</param>
    /// <returns><c>true</c> if the region is a CountryOrTerritory; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="region"/> is <c>null</c>.</exception>
    public static bool IsCountryOrTerritory(this StatisticalRegionInfo region)
    {
        Validator.ThrowIfNull(region);
        return region.Kind == StatisticalRegionKind.CountryOrTerritory;
    }

    /// <summary>
    /// Determines whether the specified region is a geographic area (not a country).
    /// </summary>
    /// <param name="region">The region to check.</param>
    /// <returns><c>true</c> if the region is not a CountryOrTerritory; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="region"/> is <c>null</c>.</exception>
    public static bool IsArea(this StatisticalRegionInfo region)
    {
        Validator.ThrowIfNull(region);
        return region.Kind != StatisticalRegionKind.CountryOrTerritory;
    }

    /// <summary>
    /// Determines whether the specified country has ISO code information available.
    /// </summary>
    /// <param name="region">The region to check.</param>
    /// <returns><c>true</c> if the region is a country and has ISO codes; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="region"/> is <c>null</c>.</exception>
    public static bool HasIsoCodes(this StatisticalRegionInfo region)
    {
        Validator.ThrowIfNull(region);
        return region.Kind == StatisticalRegionKind.CountryOrTerritory &&
               !string.IsNullOrEmpty(region.IsoAlpha2) &&
               !string.IsNullOrEmpty(region.IsoAlpha3);
    }

    /// <summary>
    /// Determines whether the specified country has a .NET <see cref="System.Globalization.RegionInfo"/> available.
    /// </summary>
    /// <param name="region">The region to check.</param>
    /// <returns><c>true</c> if the region is a country and has OS-level RegionInfo support; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="region"/> is <c>null</c>.</exception>
    public static bool HasRegionInfo(this StatisticalRegionInfo region)
    {
        Validator.ThrowIfNull(region);
        return region.Kind == StatisticalRegionKind.CountryOrTerritory && region.Region != null;
    }
}
