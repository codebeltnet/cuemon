using System.Collections.Generic;
using System.Globalization;
using Cuemon.Globalization;

namespace Cuemon.Extensions.Globalization;
/// <summary>
/// Extension methods for the <see cref="RegionInfo"/> class.
/// </summary>
public static class RegionInfoExtensions
{
    /// <summary>
    /// Resolves a sequence of related <see cref="CultureInfo"/> objects for the specified <paramref name="region"/>.
    /// </summary>
    /// <param name="region">The <see cref="RegionInfo"/> to extend.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> sequence of <see cref="CultureInfo"/> objects.</returns>
    public static IEnumerable<CultureInfo> GetCultures(this RegionInfo region)
    {
        Validator.ThrowIfNull(region);
        return World.GetCultures(region);
    }
}
