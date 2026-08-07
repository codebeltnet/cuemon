using System;
using System.Collections.Generic;

namespace Cuemon.Extensions;
/// <summary>
/// Extension methods for the <see cref="char"/> struct.
/// </summary>
public static class CharExtensions
{
    /// <summary>
    /// Converts the specified <paramref name="values"/> to its equivalent <see cref="IEnumerable{String}"/>.
    /// </summary>
    /// <param name="values">The <see cref="IEnumerable{Char}"/> to extend.</param>
    /// <returns>An <see cref="IEnumerable{String}"/> equivalent to the specified <paramref name="values"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="values"/> cannot be null.
    /// </exception>
    public static IEnumerable<string> ToEnumerable(this IEnumerable<char> values)
    {
        Validator.ThrowIfNull(values);
        return Decorator.Enclose(values).ToEnumerable();
    }

    /// <summary>
    /// Converts the specified <paramref name="values"/> to its equivalent <see cref="string"/> representation.
    /// </summary>
    /// <param name="values">The <see cref="IEnumerable{Char}"/> to extend.</param>
    /// <returns>A <see cref="string"/> equivalent to the specified <paramref name="values"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="values"/> cannot be null.
    /// </exception>
    public static string FromChars(this IEnumerable<char> values)
    {
        Validator.ThrowIfNull(values);
        return Decorator.Enclose(values).ToStringEquivalent();
    }
}
