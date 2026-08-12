using System;
using Cuemon.Net;
using Cuemon.Text;

namespace Cuemon.Extensions.Net;
/// <summary>
/// Extension methods for the <see cref="string"/> class.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Encodes a URL string.
    /// </summary>
    /// <param name="value">The <see cref="string"/> to extend.</param>
    /// <param name="setup">The <see cref="EncodingOptions"/> which may be configured.</param>
    /// <returns>An URL encoded string.</returns>
    public static string UrlEncode(this string value, Action<EncodingOptions> setup = null)
    {
        return Decorator.Enclose(value, false).UrlEncode(setup);
    }

    /// <summary>
    /// Converts a string that has been encoded for transmission in a URL into a decoded string.
    /// </summary>
    /// <param name="value">The <see cref="string"/> to extend.</param>
    /// <param name="setup">The <see cref="EncodingOptions"/> which may be configured.</param>
    /// <returns>An URL decoded string.</returns>
    public static string UrlDecode(this string value, Action<EncodingOptions> setup = null)
    {
        return Decorator.Enclose(value, false).UrlDecode(setup);
    }
}
