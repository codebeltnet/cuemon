using System;

namespace Cuemon;
/// <summary>
/// Extension methods for the <see cref="short"/>, <see cref="int"/> and <see cref="long"/> structs hidden behind the <see cref="IDecorator{T}"/> interface.
/// </summary>
/// <seealso cref="IDecorator{T}"/>
/// <seealso cref="Decorator{T}"/>
public static class IntegerDecoratorExtensions
{
    /// <summary>
    /// Returns the larger of two 32-bit signed integers.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{Int32}"/> to extend.</param>
    /// <param name="minimum">The second of two 32-bit signed integers to compare.</param>
    /// <returns>Parameter the enclosed <see cref="int"/> of the specified <paramref name="decorator"/> or <paramref name="minimum"/>, whichever is larger.</returns>
    public static int Max(this IDecorator<int> decorator, int minimum) => Math.Max(decorator.Inner, minimum);
}
