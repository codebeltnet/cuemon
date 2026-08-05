using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Cuemon.Collections.Specialized;

namespace Cuemon.Extensions.Collections.Specialized
{
    /// <summary>
    /// Extension methods for the <see cref="IDictionary{TKey,TValue}"/> interface.
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Creates a <see cref="NameValueCollection"/> from the specified <paramref name="source"/>.
        /// </summary>
        /// <param name="source">An <see cref="IDictionary{TKey,TValue}"/> to extend.</param>
        /// <param name="setup">The <see cref="DelimitedStringOptions{String}"/> which may be configured.</param>
        /// <returns>A <see cref="NameValueCollection"/> that is equivalent to the specified <paramref name="source"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> cannot be null.
        /// </exception>
        public static NameValueCollection ToNameValueCollection(this IDictionary<string, string[]> source, Action<DelimitedStringOptions<string>> setup = null)
        {
            Validator.ThrowIfNull(source);
            return Decorator.Enclose(source).ToNameValueCollection(setup);
        }
    }
}