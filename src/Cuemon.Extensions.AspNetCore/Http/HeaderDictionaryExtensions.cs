using System.Net.Http.Headers;
using Cuemon.AspNetCore.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Cuemon.Extensions.AspNetCore.Http;
/// <summary>
/// Extension methods for the <see cref="IHeaderDictionary"/> interface.
/// </summary>
public static class HeaderDictionaryExtensions
{
    /// <summary>
    /// Attempts to add or update an existing element with the provided key and value to the <see cref="IHeaderDictionary"/>.
    /// </summary>
    /// <param name="dictionary">The <see cref="IHeaderDictionary"/> to extend.</param>
    /// <param name="key">The string to use as the key of the element to add.</param>
    /// <param name="value">The string to use as the value of the element to add.</param>
    /// <param name="useAsciiEncodingConversion">if set to <c>true</c> an ASCII encoding conversion is applied to the <paramref name="value"/>.</param>
    public static void AddOrUpdateHeader(this IHeaderDictionary dictionary, string key, StringValues value, bool useAsciiEncodingConversion = true)
    {
        Validator.ThrowIfNull(dictionary);
        Decorator.Enclose(dictionary).AddOrUpdateHeader(key, value, useAsciiEncodingConversion);
    }

    /// <summary>
    /// Attempts to add or update one or more elements from the provided collection of <paramref name="responseHeaders"/> to the <see cref="IHeaderDictionary"/>.
    /// </summary>
    /// <param name="dictionary">The <see cref="IHeaderDictionary"/> to extend.</param>
    /// <param name="responseHeaders">The <see cref="HttpResponseHeaders"/> to copy.</param>
    public static void AddOrUpdateHeaders(this IHeaderDictionary dictionary, HttpResponseHeaders responseHeaders)
    {
        Decorator.Enclose(dictionary).AddOrUpdateHeaders(responseHeaders);
    }
}
