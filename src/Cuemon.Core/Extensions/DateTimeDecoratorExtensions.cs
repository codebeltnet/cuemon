using System;

namespace Cuemon;
/// <summary>
/// Extension methods for the <see cref="DateTime"/> struct hidden behind the <see cref="IDecorator{T}"/> interface.
/// </summary>
/// <seealso cref="IDecorator{T}"/>
/// <seealso cref="Decorator{T}"/>
public static class DateTimeDecoratorExtensions
{
    /// <summary>
    /// A <see cref="DateTime"/> initialized to midnight, January 1st, 1970 in Coordinated Universal Time (UTC).
    /// </summary>
    private static readonly DateTime UnixEpoch = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

    /// <summary>
    /// Gets a <see cref="DateTime"/> initialized to midnight, January 1st, 1970 in Coordinated Universal Time (UTC).
    /// </summary>
    /// <param name="_">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <returns>A a <see cref="DateTime"/> initialized to midnight, January 1st, 1970 in Coordinated Universal Time (UTC).</returns>
    public static DateTime GetUnixEpoch(this IDecorator<DateTime> _)
    {
        return UnixEpoch;
    }

    /// <summary>
    /// Converts the enclosed <see cref="DateTime"/> of the <paramref name="decorator"/> to an equivalent UNIX Epoch time representation.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <returns>A <see cref="double"/> value that is equivalent to the enclosed <see cref="DateTime"/> of the <paramref name="decorator"/>.</returns>
    /// <remarks>This implementation converts the enclosed <see cref="DateTime"/> of the <paramref name="decorator"/> to an UTC representation ONLY if the <see cref="DateTime.Kind"/> equals <see cref="DateTimeKind.Local"/>.</remarks>
    public static double ToUnixEpochTime(this IDecorator<DateTime> decorator)
    {
        Validator.ThrowIfNull(decorator);
        var value = decorator.Inner.Kind == DateTimeKind.Local ? decorator.Inner.ToUniversalTime() : decorator.Inner;
        return Math.Floor((value - UnixEpoch).TotalSeconds);
    }

    /// <summary>
    /// Converts the enclosed <see cref="DateTime"/> of the <paramref name="decorator"/> to a Coordinated Universal Time (UTC) representation.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <returns>A new <see cref="DateTime"/> value initialized to <see cref="DateTimeKind.Utc"/> that has the same number of ticks as the object represented by the enclosed <see cref="DateTime"/> of the <paramref name="decorator"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static DateTime ToUtcKind(this IDecorator<DateTime> decorator)
    {
        Validator.ThrowIfNull(decorator);
        return ToKind(decorator.Inner, DateTimeKind.Utc);
    }

    /// <summary>
    /// Converts the enclosed <see cref="DateTime"/> of the <paramref name="decorator"/> to a local time representation.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <returns>A new <see cref="DateTime"/> value initialized to <see cref="DateTimeKind.Local"/> that has the same number of ticks as the enclosed <see cref="DateTime"/> of the <paramref name="decorator"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static DateTime ToLocalKind(this IDecorator<DateTime> decorator)
    {
        Validator.ThrowIfNull(decorator);
        return ToKind(decorator.Inner, DateTimeKind.Local);
    }

    /// <summary>
    /// Converts the enclosed <see cref="DateTime"/> of the <paramref name="decorator"/> to a representation that is not specified as either local time or UTC.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <returns>A new <see cref="DateTime"/> value initialized to <see cref="DateTimeKind.Unspecified"/> that has the same number of ticks as the enclosed <see cref="DateTime"/> of the <paramref name="decorator"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static DateTime ToDefaultKind(this IDecorator<DateTime> decorator)
    {
        Validator.ThrowIfNull(decorator);
        return ToKind(decorator.Inner, DateTimeKind.Unspecified);
    }

    private static DateTime ToKind(DateTime value, DateTimeKind kind)
    {
        if (value.Kind != kind) { value = DateTime.SpecifyKind(value, kind); }
        return value;
    }
}
