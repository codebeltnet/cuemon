using System;
using System.Diagnostics;
using Cuemon.Reflection;

namespace Cuemon.Extensions.Diagnostics;
/// <summary>
/// Extension methods for the <see cref="FileVersionInfo"/> class.
/// </summary>
public static class FileVersionInfoExtensions
{
    /// <summary>
    /// Returns a <see cref="VersionResult"/> from the specified <paramref name="fvi"/>.
    /// </summary>
    /// <param name="fvi">An instance of <see cref="FileVersionInfo"/>.</param>
    /// <returns>A <see cref="VersionResult"/> that represents the product version that the <see cref="FileVersionInfo"/> is distributed with.</returns>
    public static VersionResult ToProductVersion(this FileVersionInfo fvi)
    {
        return ToVersion(fvi, info => info.ProductVersion);
    }

    /// <summary>
    /// Returns a <see cref="VersionResult"/> from the specified <paramref name="fvi"/>.
    /// </summary>
    /// <param name="fvi">An instance of <see cref="FileVersionInfo"/>.</param>
    /// <returns>A <see cref="VersionResult"/> that represents the file version that the <see cref="FileVersionInfo"/> is distributed with.</returns>
    public static VersionResult ToFileVersion(this FileVersionInfo fvi)
    {
        return ToVersion(fvi, info => info.FileVersion);
    }

    private static VersionResult ToVersion(this FileVersionInfo fvi, Func<FileVersionInfo, string> propertySelector)
    {
        Validator.ThrowIfNull(fvi);
        var version = propertySelector(fvi);
        return new VersionResult(version);
    }
}
