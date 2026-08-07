using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Cuemon.Globalization;
/// <summary>
/// Represents a geographic region or country as defined by the UN M.49 standard.
/// </summary>
/// <remarks>
/// UN M.49 is a standard for representing country and area codes for statistical use.
/// This class provides a unified view of the hierarchy, treating countries as leaf nodes
/// in the region tree. Each instance has a <see cref="Kind"/> that identifies its level
/// in the hierarchy (World, Region, Subregion, IntermediateRegion, or CountryOrTerritory).
/// Source: https://unstats.un.org/unsd/methodology/m49/
/// </remarks>
public sealed class StatisticalRegionInfo
{
    private readonly List<StatisticalRegionInfo> _children = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="StatisticalRegionInfo"/> class for a region.
    /// </summary>
    /// <param name="code">The UN M.49 numeric code.</param>
    /// <param name="name">The region name.</param>
    /// <param name="kind">The kind of region.</param>
    /// <param name="parent">The parent region, or <c>null</c> for World.</param>
    /// <exception cref="ArgumentNullException"><paramref name="code"/> or <paramref name="name"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException"><paramref name="code"/> or <paramref name="name"/> is empty.</exception>
    internal StatisticalRegionInfo(string code, string name, StatisticalRegionKind kind, StatisticalRegionInfo parent)
    {
        Validator.ThrowIfNullOrEmpty(code);
        Validator.ThrowIfNullOrEmpty(name);

        Code = code;
        Name = name;
        Kind = kind;
        Parent = parent;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="StatisticalRegionInfo"/> class for a country.
    /// </summary>
    /// <param name="code">The UN M.49 numeric code.</param>
    /// <param name="name">The country name.</param>
    /// <param name="isoAlpha2">The ISO 3166-1 alpha-2 code.</param>
    /// <param name="isoAlpha3">The ISO 3166-1 alpha-3 code.</param>
    /// <param name="parent">The parent region.</param>
    /// <param name="isLeastDevelopedCountry">Whether this is a Least Developed Country.</param>
    /// <param name="isLandLockedDevelopingCountry">Whether this is a Land Locked Developing Country.</param>
    /// <param name="isSmallIslandDevelopingState">Whether this is a Small Island Developing State.</param>
    /// <param name="region">The .NET RegionInfo, or <c>null</c> if not available.</param>
    /// <exception cref="ArgumentNullException"><paramref name="code"/> or <paramref name="name"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException"><paramref name="code"/> or <paramref name="name"/> is empty.</exception>
    internal StatisticalRegionInfo(
        string code,
        string name,
        string isoAlpha2,
        string isoAlpha3,
        StatisticalRegionInfo parent,
        bool isLeastDevelopedCountry,
        bool isLandLockedDevelopingCountry,
        bool isSmallIslandDevelopingState,
        RegionInfo region)
    {
        Validator.ThrowIfNullOrEmpty(code);
        Validator.ThrowIfNullOrEmpty(name);

        Code = code;
        Name = name;
        Kind = StatisticalRegionKind.CountryOrTerritory;
        Parent = parent;
        IsoAlpha2 = isoAlpha2;
        IsoAlpha3 = isoAlpha3;
        IsLeastDevelopedCountry = isLeastDevelopedCountry;
        IsLandLockedDevelopingCountry = isLandLockedDevelopingCountry;
        IsSmallIslandDevelopingState = isSmallIslandDevelopingState;
        Region = region;
    }

    /// <summary>
    /// Gets the UN M.49 numeric code for this region or country.
    /// </summary>
    /// <value>The three-digit UN M.49 code (e.g., "001" for World, "840" for United States).</value>
    public string Code { get; }

    /// <summary>
    /// Gets the official UN name of this region or country.
    /// </summary>
    /// <value>The name (e.g., "World", "Europe", "United States of America").</value>
    public string Name { get; }

    /// <summary>
    /// Gets the kind of this statistical region.
    /// </summary>
    /// <value>A <see cref="StatisticalRegionKind"/> value indicating the hierarchy level.</value>
    public StatisticalRegionKind Kind { get; }

    /// <summary>
    /// Gets the parent region in the UN M.49 hierarchy.
    /// </summary>
    /// <value>The parent region, or <c>null</c> if this is the World region (code "001").</value>
    public StatisticalRegionInfo Parent { get; internal set; }

    /// <summary>
    /// Gets the direct child regions or countries of this region.
    /// </summary>
    /// <value>An enumerable list of children. Empty list if this is a leaf node (country).</value>
    public IEnumerable<StatisticalRegionInfo> Children => _children;

    /// <summary>
    /// Gets all countries in this geographic region.
    /// </summary>
    /// <value>An enumerable list of child regions where <see cref="Kind"/> is <see cref="StatisticalRegionKind.CountryOrTerritory"/>.</value>
    /// <remarks>
    /// This is a convenience property that filters <see cref="Children"/> to return only countries.
    /// It recursively includes all descendant countries, not just immediate children.
    /// </remarks>
    public IEnumerable<StatisticalRegionInfo> Countries => GetAllDescendants()
        .Where(r => r.Kind == StatisticalRegionKind.CountryOrTerritory)
        .ToList();

    /// <summary>
    /// Gets the ISO 3166-1 alpha-2 code for this country.
    /// </summary>
    /// <value>The two-letter ISO code (e.g., "US", "DE"), or <c>null</c> if this is not a country.</value>
    public string IsoAlpha2 { get; }

    /// <summary>
    /// Gets the ISO 3166-1 alpha-3 code for this country.
    /// </summary>
    /// <value>The three-letter ISO code (e.g., "USA", "DEU"), or <c>null</c> if this is not a country.</value>
    public string IsoAlpha3 { get; }

    /// <summary>
    /// Gets the .NET <see cref="RegionInfo"/> for this country if available.
    /// </summary>
    /// <value>The RegionInfo instance, or <c>null</c> if not a country or not supported by the OS.</value>
    /// <remarks>
    /// Some territories (e.g., "British Indian Ocean Territory") may not have OS-level support.
    /// </remarks>
    public RegionInfo Region { get; }

    /// <summary>
    /// Gets a value indicating whether this country is classified as a Least Developed Country (LDC).
    /// </summary>
    /// <value><c>true</c> if this is an LDC; otherwise, <c>false</c>. Always <c>false</c> for non-countries.</value>
    public bool IsLeastDevelopedCountry { get; }

    /// <summary>
    /// Gets a value indicating whether this country is classified as a Land Locked Developing Country (LLDC).
    /// </summary>
    /// <value><c>true</c> if this is an LLDC; otherwise, <c>false</c>. Always <c>false</c> for non-countries.</value>
    public bool IsLandLockedDevelopingCountry { get; }

    /// <summary>
    /// Gets a value indicating whether this country is classified as a Small Island Developing State (SIDS).
    /// </summary>
    /// <value><c>true</c> if this is a SIDS; otherwise, <c>false</c>. Always <c>false</c> for non-countries.</value>
    public bool IsSmallIslandDevelopingState { get; }

    /// <summary>
    /// Gets all ancestor regions in the hierarchy up to and including World.
    /// </summary>
    /// <returns>An enumerable of ancestor regions, ordered from immediate parent to World.</returns>
    public IEnumerable<StatisticalRegionInfo> GetAncestors()
    {
        var current = Parent;
        while (current != null)
        {
            yield return current;
            current = current.Parent;
        }
    }

    /// <summary>
    /// Gets all descendant regions and countries recursively.
    /// </summary>
    /// <returns>An enumerable of all descendants in the hierarchy.</returns>
    public IEnumerable<StatisticalRegionInfo> GetAllDescendants()
    {
        foreach (var child in Children)
        {
            yield return child;
            foreach (var descendant in child.GetAllDescendants())
            {
                yield return descendant;
            }
        }
    }

    /// <summary>
    /// Adds a child region to this region.
    /// </summary>
    /// <param name="child">The child region to add.</param>
    /// <exception cref="InvalidOperationException">This region is a country and cannot have children.</exception>
    internal void AddChild(StatisticalRegionInfo child)
    {
        if (Kind == StatisticalRegionKind.CountryOrTerritory)
        {
            throw new InvalidOperationException("Countries cannot have child regions.");
        }

        _children.Add(child);
    }

    /// <summary>
    /// Returns a string that represents the current object.
    /// </summary>
    /// <returns>A string that represents the current object.</returns>
    public override string ToString()
    {
        return $"{Name} ({Code})";
    }
}
