using System;
using System.Data;
using Cuemon.Data;

namespace Cuemon.Extensions.Data;
/// <summary>
/// Extension methods for the <see cref="DbType"/> enumeration.
/// </summary>
public static class DbTypeExtensions
{
    /// <summary>
    /// Provides the equivalent <see cref="Type"/> of a <see cref="DbType"/> enumeration value.
    /// </summary>
    /// <param name="dbType">The <see cref="DbType"/> to extend.</param>
    /// <returns>The equivalent <see cref="Type"/> of a <see cref="DbType"/> enumeration value.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="dbType"/> value is not valid.
    /// </exception>
    public static Type ToType(this DbType dbType)
    {
        return Decorator.EncloseToExpose(dbType).ToType();
    }
}
