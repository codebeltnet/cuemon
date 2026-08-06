using System.Collections.Generic;
using Cuemon.Collections.Specialized;

namespace Cuemon.Extensions.Net;
/// <summary>
/// Extension methods for the <see cref="IDictionary{TKey,TValue}"/> interface.
/// </summary>
public static class DictionaryExtensions
{
    /// <summary>
    /// Converts the specified <paramref name="query"/> into its <see cref="string"/> equivalent.
    /// </summary>
    /// <param name="query">The <see cref="IDictionary{TKey,TValue}"/> to extend.</param>
    /// <param name="urlEncode">Specify <c>true</c> to encode the <paramref name="query"/> into a URL-encoded string; otherwise, <c>false</c>. Default is <c>false</c>.</param>
    /// <returns>A <see cref="string"/> equivalent to the values in the <paramref name="query"/>.</returns>
    public static string ToQueryString(this IDictionary<string, string[]> query, bool urlEncode = false)
    {
        Validator.ThrowIfNull(query);
        return Decorator.Enclose(query).ToNameValueCollection().ToQueryString(urlEncode);
    }
}
