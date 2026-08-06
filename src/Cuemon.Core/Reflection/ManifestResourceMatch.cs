namespace Cuemon.Reflection;
/// <summary>
/// Specifies the way of finding and returning an embedded resource.
/// </summary>
public enum ManifestResourceMatch
{
    /// <summary>
    /// Specifies an exact match on the file name of the embedded resource.
    /// </summary>
    Name = 0,
    /// <summary>
    /// Specifies a partial match on the file name of the embedded resource.
    /// </summary>
    ContainsName = 1,
    /// <summary>
    /// Specifies an exact match on the file extension contained within the file name of the embedded resource.
    /// </summary>
    Extension = 2,
    /// <summary>
    /// Specifies a partial match on the file extension contained within the file name of the embedded resource.
    /// </summary>
    ContainsExtension = 3
}
