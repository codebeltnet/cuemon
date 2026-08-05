using System;
using System.IO;
using Cuemon.Text;

namespace Cuemon
{
    /// <summary>
    /// Extension methods for the <see cref="byte"/> array hidden behind the <see cref="IDecorator{T}"/> interface.
    /// </summary>
    /// <seealso cref="IDecorator{T}"/>
    /// <seealso cref="Decorator{T}"/>
    public static class ByteArrayDecoratorExtensions
    {
        /// <summary>
        /// Converts the enclosed <see cref="byte"/> array of the specified <paramref name="decorator"/> to its equivalent <see cref="string"/> representation.
        /// </summary>
        /// <param name="decorator">The decorator that wraps the <see cref="byte"/> array to extend.</param>
        /// <param name="setup">The <see cref="EncodingOptions"/> which may be configured.</param>
        /// <returns>A <see cref="string"/> that is equivalent to the enclosed <see cref="byte"/> array of the specified <paramref name="decorator"/>.</returns>
        /// <remarks><see cref="EncodingOptions"/> will be initialized with <see cref="EncodingOptions.DefaultPreambleSequence"/> and <see cref="EncodingOptions.DefaultEncoding"/>.</remarks>
        public static string ToEncodedString(this IDecorator<byte[]> decorator, Action<EncodingOptions> setup = null)
        {
            Validator.ThrowIfNull(decorator);
            return Convertible.ToString(decorator.Inner, setup);
        }

        /// <summary>
        /// Converts the enclosed <see cref="byte"/> array of the specified <paramref name="decorator"/> to its equivalent <see cref="Stream"/> representation.
        /// </summary>
        /// <param name="decorator">The decorator that wraps the <see cref="byte"/> array to extend.</param>
        /// <returns>A <see cref="Stream"/> that is equivalent to the enclosed <see cref="byte"/> array of the specified <paramref name="decorator"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        public static Stream ToStream(this IDecorator<byte[]> decorator)
        {
            Validator.ThrowIfNull(decorator);
            return Patterns.SafeInvoke(() => new MemoryStream(decorator.Inner.Length), ms =>
            {
                ms.Write(decorator.Inner, 0, decorator.Inner.Length);
                ms.Position = 0;
                return ms;
            });
        }
    }
}
