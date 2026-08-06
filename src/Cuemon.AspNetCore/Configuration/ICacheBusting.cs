namespace Cuemon.AspNetCore.Configuration;
/// <summary>
/// An interface to provide cache-busting capabilities.
/// </summary>
public interface ICacheBusting
{
    /// <summary>
    /// Gets the version to be a part of the link you need cache-busting compatible.
    /// </summary>
    /// <value>The version to be a part of the link you need cache-busting compatible.</value>
    string Version { get; }
}
