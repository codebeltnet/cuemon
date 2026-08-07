using System;
using System.IO;
using System.Reflection;
using Cuemon.Data.Integrity;
using Cuemon.Security;

namespace Cuemon.Extensions.Data.Integrity;
/// <summary>
/// Extension methods for the <see cref="Assembly"/> class.
/// </summary>
public static class AssemblyExtensions
{
    /// <summary>
    /// Returns a <see cref="CacheValidator"/> from the specified <paramref name="assembly" />.
    /// </summary>
    /// <param name="assembly">The assembly to resolve a <see cref="CacheValidator" /> from.</param>
    /// <param name="hashFactory">The function delegate that is invoked to produce the <see cref="HashResult"/>.</param>
    /// <param name="setup">The <see cref="FileChecksumOptions" /> which may be configured.</param>
    /// <returns>A <see cref="CacheValidator" /> that represents the integrity of the specified <paramref name="assembly" />.</returns>
    public static CacheValidator GetCacheValidator(this Assembly assembly, Func<Hash> hashFactory = null, Action<FileChecksumOptions> setup = null)
    {
        if (assembly == null || assembly.IsDynamic) { return CacheValidator.Default; }
        var assemblyHashCode64 = Generate.HashCode64(assembly.FullName);
        var assemblyLocation = assembly.Location;
        return string.IsNullOrEmpty(assemblyLocation)
            ? new CacheValidator(new EntityInfo(DateTime.MinValue, DateTime.MaxValue, Convertible.GetBytes(assemblyHashCode64)), hashFactory)
            : new FileInfo(assemblyLocation).GetCacheValidator(hashFactory, setup).CombineWith(assemblyHashCode64);
    }
}
