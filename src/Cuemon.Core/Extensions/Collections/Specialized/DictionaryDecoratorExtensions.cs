using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Cuemon.Collections.Specialized;
/// <summary>
/// Extension methods for the <see cref="IDictionary{TKey,TValue}"/> interface hidden behind the <see cref="IDecorator{T}"/> interface.
/// </summary>
/// <seealso cref="IDecorator{T}"/>
/// <seealso cref="Decorator{T}"/>
public static class DictionaryDecoratorExtensions
{
    /// <summary>
    /// Creates a <see cref="NameValueCollection"/> from the enclosed dictionary of <see cref="string"/> keys and <see cref="string"/> arrays in the specified <paramref name="decorator"/>.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <param name="setup">The <see cref="DelimitedStringOptions{String}"/> which may be configured.</param>
    /// <returns>A <see cref="NameValueCollection"/> that is equivalent to the enclosed dictionary of <see cref="string"/> keys and <see cref="string"/> arrays in the specified <paramref name="decorator"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static NameValueCollection ToNameValueCollection(this IDecorator<IDictionary<string, string[]>> decorator, Action<DelimitedStringOptions<string>> setup = null)
    {
        Validator.ThrowIfNull(decorator);
        var result = new NameValueCollection();
        foreach (var item in decorator.Inner)
        {
            result.Add(item.Key, DelimitedString.Create(item.Value, setup));
        }
        return result;
    }
}
