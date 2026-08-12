using System;
using System.Data;

namespace Cuemon.Data;
/// <summary>
/// Extension methods for the <see cref="DbType"/> enumeration hidden behind the <see cref="IDecorator{T}"/> interface.
/// </summary>
/// <seealso cref="IDecorator{T}"/>
/// <seealso cref="Decorator{T}"/>
public static class DbTypeDecoratorExtensions
{
    /// <summary>
    /// Provides the equivalent <see cref="Type"/> of the underlying <see cref="DbType"/> enumeration value of the <paramref name="decorator"/>.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <returns>The equivalent <see cref="Type"/> of the underlying <see cref="DbType"/> enumeration value of the <paramref name="decorator"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> is null or its underlying value is null.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="decorator"/> underlying value is not valid.
    /// </exception>
    public static Type ToType(this IDecorator<DbType> decorator)
    {
        Validator.ThrowIfNull(decorator, out var dbType);
        switch (dbType)
        {
            case DbType.Byte:
                return typeof(byte);
            case DbType.SByte:
                return typeof(sbyte);
            case DbType.Binary:
                return typeof(byte[]);
            case DbType.Boolean:
                return typeof(bool);
            case DbType.Currency:
            case DbType.Double:
                return typeof(double);
            case DbType.Date:
            case DbType.DateTime:
            case DbType.Time:
            case DbType.DateTime2:
                return typeof(DateTime);
            case DbType.DateTimeOffset:
                return typeof(DateTimeOffset);
            case DbType.Guid:
                return typeof(Guid);
            case DbType.Int64:
                return typeof(long);
            case DbType.Int32:
                return typeof(int);
            case DbType.Int16:
                return typeof(short);
            case DbType.Object:
                return typeof(object);
            case DbType.Single:
                return typeof(float);
            case DbType.UInt64:
                return typeof(ulong);
            case DbType.UInt32:
                return typeof(uint);
            case DbType.UInt16:
                return typeof(ushort);
            case DbType.Decimal:
            case DbType.VarNumeric:
                return typeof(decimal);
            case DbType.AnsiString:
            case DbType.AnsiStringFixedLength:
            case DbType.StringFixedLength:
            case DbType.String:
            case DbType.Xml:
                return typeof(string);
            default:
                throw new ArgumentOutOfRangeException(decorator.ArgumentName ?? nameof(decorator), FormattableString.Invariant($"{nameof(DbType)}, '{dbType}', is not supported."));
        }
    }
}
