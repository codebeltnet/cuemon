using System.Collections.Specialized;
using Cuemon.Net;
using Cuemon.Net.Collections.Specialized;

namespace Cuemon.Extensions.Net;
/// <summary>
/// Extension methods for the <see cref="NameValueCollection"/> class.
/// </summary>
public static class NameValueCollectionExtensions
{
    /// <summary>
    /// Converts the specified <paramref name="nvc"/> into its <see cref="string"/> equivalent.
    /// </summary>
    /// <param name="nvc">The <see cref="NameValueCollection"/> to extend.</param>
    /// <param name="urlEncode">Specify <c>true</c> to encode the <paramref name="nvc"/> into a URL-encoded string; otherwise, <c>false</c>. Default is <c>false</c>.</param>
    /// <returns>A <see cref="string"/> equivalent to the values in the <paramref name="nvc"/>.</returns>
    public static string ToQueryString(this NameValueCollection nvc, bool urlEncode = false)
    {
        Validator.ThrowIfNull(nvc);
        return Decorator.Enclose(nvc).ToString(FieldValueSeparator.Ampersand, urlEncode);
    }
}
