using System;
using System.Collections.Generic;
using System.Linq;

namespace Cuemon
{
    /// <summary>
    /// Extension methods for the <see cref="char"/> struct hidden behind the <see cref="IDecorator{T}"/> interface.
    /// </summary>
    /// <seealso cref="IDecorator{T}"/>
    /// <seealso cref="Decorator{T}"/>
    public static class CharDecoratorExtensions
    {
        /// <summary>
        /// Converts the enclosed <see cref="IEnumerable{Char}"/> of the <paramref name="decorator"/> to its equivalent <see cref="IEnumerable{String}"/>.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
        /// <returns>An <see cref="IEnumerable{String}"/> equivalent to the enclosed <see cref="IEnumerable{Char}"/> of the <paramref name="decorator"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        public static IEnumerable<string> ToEnumerable(this IDecorator<IEnumerable<char>> decorator)
        {
            Validator.ThrowIfNull(decorator);
            return decorator.Inner.Select(c => new string(c, 1));
        }

        /// <summary>
        /// Converts the enclosed <see cref="IEnumerable{Char}"/> of the <paramref name="decorator"/> to its equivalent <see cref="string"/> representation.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
        /// <returns>A <see cref="string"/> equivalent to the enclosed <see cref="IEnumerable{Char}"/> of the <paramref name="decorator"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        public static string ToStringEquivalent(this IDecorator<IEnumerable<char>> decorator)
        {
            Validator.ThrowIfNull(decorator);
            return string.Concat(decorator.Inner);
        }
    }
}