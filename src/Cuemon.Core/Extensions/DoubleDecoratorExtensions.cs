using System;
using System.ComponentModel;

namespace Cuemon;
/// <summary>
/// Extension methods for the <see cref="double"/> struct hidden behind the <see cref="IDecorator{T}"/> interface.
/// </summary>
/// <seealso cref="IDecorator{T}"/>
/// <seealso cref="Decorator{T}"/>
public static class DoubleDecoratorExtensions
{
    /// <summary>
    /// Converts the enclosed <see cref="double"/> of the <paramref name="decorator"/> to its equivalent <see cref="TimeSpan"/> representation.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <param name="unitOfTime">One of the enumeration values that specifies the outcome of the conversion.</param>
    /// <returns>A <see cref="TimeSpan"/> that corresponds to the enclosed <see cref="double"/> of the <paramref name="decorator"/> and <paramref name="unitOfTime"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    /// <exception cref="OverflowException">
    /// The enclosed <see cref="double"/> of the <paramref name="decorator"/> paired with <paramref name="unitOfTime"/> is outside its valid range.
    /// </exception>
    /// <exception cref="InvalidEnumArgumentException">
    /// <paramref name="unitOfTime"/> was outside its valid range.
    /// </exception>
    public static TimeSpan ToTimeSpan(this IDecorator<double> decorator, TimeUnit unitOfTime)
    {
        var input = decorator.Inner;
        if (input == 0.0) { return TimeSpan.Zero; }
        switch (unitOfTime)
        {
            case TimeUnit.Days:
                return TimeSpan.FromDays(input);
            case TimeUnit.Hours:
                return TimeSpan.FromHours(input);
            case TimeUnit.Minutes:
                return TimeSpan.FromMinutes(input);
            case TimeUnit.Seconds:
                return TimeSpan.FromSeconds(input);
            case TimeUnit.Milliseconds:
                return TimeSpan.FromMilliseconds(input);
            case TimeUnit.Ticks:
                if (input < long.MinValue || input > long.MaxValue) { throw new OverflowException(FormattableString.Invariant($"The specified input, {input}, having a time unit specified as Ticks cannot be less than {long.MinValue} or be greater than {long.MaxValue}.")); }
                if (input == long.MaxValue) { return TimeSpan.MaxValue; }
                if (input == long.MinValue) { return TimeSpan.MinValue; }
                return TimeSpan.FromTicks((long)input);
            default:
                throw new InvalidEnumArgumentException(nameof(unitOfTime), (int)unitOfTime, typeof(TimeUnit));
        }
    }
}
