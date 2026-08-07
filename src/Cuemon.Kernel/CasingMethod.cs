namespace Cuemon;
/// <summary>
/// Specifies ways that a string must be converted in terms of casing.
/// </summary>
public enum CasingMethod
{
    /// <summary>
    /// Indicates default behavior which is leaving the casing unaltered, hence allowing mixed casing.
    /// </summary>
    Default,
    /// <summary>
    /// Indicates that all characters will be converted to lowercase.
    /// </summary>
    LowerCase,
    /// <summary>
    /// Indicates that all characters will be converted to UPPERCASE.
    /// </summary>
    UpperCase,
    /// <summary>
    /// Indicates that characters will be converted to Title Case.
    /// </summary>
    TitleCase
}
