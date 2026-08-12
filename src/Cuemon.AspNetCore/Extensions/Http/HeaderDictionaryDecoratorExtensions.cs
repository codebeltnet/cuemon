using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using Cuemon.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Cuemon.AspNetCore.Http;
/// <summary>
/// Extension methods for the <see cref="IHeaderDictionary"/> interface hidden behind the <see cref="IDecorator{T}"/> interface.
/// </summary>
/// <seealso cref="IDecorator{T}"/>
/// <seealso cref="Decorator{T}"/>
public static class HeaderDictionaryDecoratorExtensions
{
    /// <summary>
    /// Adds a range of <paramref name="headers"/> to the enclosed <see cref="IHeaderDictionary"/>.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <param name="headers">The <see cref="IHeaderDictionary"/> to populate.</param>
    /// <param name="predicate">The function delegate that specifies what elements to populate from <paramref name="headers"/>. Default is only non-existing headers.</param>
    /// <returns>A reference to <paramref name="decorator.Inner" /> so that additional calls can be chained.</returns>
    /// <remarks>When <paramref name="predicate"/> is <c>null</c>, only new headers are added to the enclosed <see cref="IHeaderDictionary"/>.</remarks>
    public static IHeaderDictionary AddRange(this IDecorator<IHeaderDictionary> decorator, IHeaderDictionary headers, Func<KeyValuePair<string, StringValues>, IHeaderDictionary, bool> predicate = null)
    {
        Validator.ThrowIfNull(decorator);
        Validator.ThrowIfNull(headers);
        predicate ??= (kvp, hd) => !hd.Contains(kvp);
        foreach (var header in headers.Where(pair => predicate(pair, decorator.Inner)))
        {
            decorator.Inner.Add(header);
        }
        return decorator.Inner;
    }

    /// <summary>
    /// Attempts to add or update an existing element with the provided key and value to the enclosed <see cref="IHeaderDictionary"/> of the <paramref name="decorator"/>.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <param name="key">The string to use as the key of the element to add.</param>
    /// <param name="value">The string to use as the value of the element to add.</param>
    /// <param name="useAsciiEncodingConversion">if set to <c>true</c> an ASCII encoding conversion is applied to the <paramref name="value"/>.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static void AddOrUpdateHeader(this IDecorator<IHeaderDictionary> decorator, string key, StringValues value, bool useAsciiEncodingConversion = true)
    {
        var headerValue = useAsciiEncodingConversion ? new StringValues(Decorator.Enclose<string>(value).ToAsciiEncodedString()) : value;
        if (headerValue != StringValues.Empty)
        {
            decorator.AddOrUpdate(key, Decorator.Enclose(headerValue.ToString().Where(c => !char.IsControl(c))).ToStringEquivalent());
        }
    }

    /// <summary>
    /// Attempts to add or update one or more elements from the provided collection of <paramref name="responseHeaders"/> to the enclosed <see cref="IHeaderDictionary"/> of the <paramref name="decorator"/>.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <param name="responseHeaders">The <see cref="HttpResponseHeaders"/> to copy.</param>
    public static void AddOrUpdateHeaders(this IDecorator<IHeaderDictionary> decorator, HttpResponseHeaders responseHeaders)
    {
        if (decorator == null || responseHeaders == null) { return; }
        foreach (var header in responseHeaders)
        {
            decorator.AddOrUpdate(header.Key, header.Value != null ? DelimitedString.Create(header.Value) : "");
        }
    }
}
