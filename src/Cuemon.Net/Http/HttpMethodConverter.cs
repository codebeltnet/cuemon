using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Cuemon.Collections.Generic;
using Cuemon.Text;

namespace Cuemon.Net.Http;
/// <summary>
/// This utility class is designed to make <see cref="HttpMethod"/> related conversions easier to work with.
/// </summary>
public static class HttpMethodConverter
{
    private static readonly IDictionary<string, HttpMethods> StringToHttpMethodLookupTable = InitStringToHttpMethodLookupTable();

    private static IDictionary<string, HttpMethods> InitStringToHttpMethodLookupTable()
    {
        var result = new Dictionary<string, HttpMethods>(StringComparer.OrdinalIgnoreCase);
        foreach (var pair in new EnumReadOnlyDictionary<HttpMethods>().Select(pair => pair.Value))
        {
            result.Add(pair, ParserFactory.FromEnum().Parse<HttpMethods>(pair));
        }
        return result;
    }

    /// <summary>
    /// Converts the specified <paramref name="method"/> to its equivalent <see cref="HttpMethods"/> representation.
    /// </summary>
    /// <param name="method">The <see cref="HttpMethod"/> to be converted.</param>
    /// <returns>A <see cref="HttpMethods"/> representation of the specified <paramref name="method"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="method"/> cannot be null.
    /// </exception>
    public static HttpMethods ToHttpMethod(HttpMethod method)
    {
        Validator.ThrowIfNull(method);
        if (!StringToHttpMethodLookupTable.TryGetValue(method.Method, out var result))
        {
            result = HttpMethods.Get;
        }
        return result;
    }
}
