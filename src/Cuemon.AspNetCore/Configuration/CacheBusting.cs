namespace Cuemon.AspNetCore.Configuration;
/// <summary>
/// Represents a way to provide cache-busting capabilities.
/// </summary>
public abstract class CacheBusting : ICacheBusting
{
    /// <summary>
    /// Gets the version to be a part of the link you need cache-busting compatible.
    /// </summary>
    /// <value>The version to be a part of the link you need cache-busting compatible.</value>
    public abstract string Version { get; }
}
