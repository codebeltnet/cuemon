namespace Cuemon.Globalization;
/// <summary>
/// Specifies the kind of a statistical region as defined by the UN M.49 standard.
/// </summary>
/// <remarks>
/// UN M.49 defines a hierarchical structure of geographic regions for statistical use.
/// This enum represents the different levels in that hierarchy, from World down to individual countries.
/// </remarks>
public enum StatisticalRegionKind
{
    /// <summary>
    /// The root node representing the entire World (code "001").
    /// </summary>
    World,

    /// <summary>
    /// A major geographic region or continent (e.g., Africa, Europe, Asia).
    /// </summary>
    Region,

    /// <summary>
    /// A subdivision of a region (e.g., Western Europe, Northern Africa).
    /// </summary>
    Subregion,

    /// <summary>
    /// An intermediate region that groups subregions (e.g., Latin America and the Caribbean).
    /// </summary>
    IntermediateRegion,

    /// <summary>
    /// A country or territory - a leaf node in the hierarchy with no children.
    /// </summary>
    CountryOrTerritory
}
