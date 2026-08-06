using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace Cuemon.Net.Collections.Specialized;
/// <summary>
/// Extension methods for the <see cref="NameValueCollection"/> class hidden behind the <see cref="IDecorator{T}"/> interface.
/// </summary>
/// <seealso cref="IDecorator{T}"/>
/// <seealso cref="Decorator{T}"/>
public static class NameValueCollectionDecoratorExtensions
{
    /// <summary>
    /// Returns a <see cref="string" /> that represents the enclosed <see cref="NameValueCollection"/> of the <paramref name="decorator"/>.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{NameValueCollection}"/> to extend.</param>
    /// <param name="separator">The separator used to form the key-value pairs.</param>
    /// <param name="urlEncode">Specify <c>true</c> to encode the values of the enclosed <see cref="NameValueCollection"/> of the <paramref name="decorator"/> into a URL-encoded string; otherwise, <c>false</c>. Default is <c>false</c>.</param>
    /// <returns>A <see cref="string" /> that represents the enclosed <see cref="NameValueCollection"/> of the <paramref name="decorator"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static string ToString(this IDecorator<NameValueCollection> decorator, FieldValueSeparator separator, bool urlEncode)
    {
        Validator.ThrowIfNull(decorator, out var fieldValuePairs);
        var characterSeparator = GetSeparator(separator);
        var builder = new StringBuilder(separator == FieldValueSeparator.Ampersand ? "?" : "");
        foreach (string item in fieldValuePairs)
        {
            var values = fieldValuePairs[item].Split(',');
            foreach (var value in values)
            {
                builder.AppendFormat(CultureInfo.InvariantCulture, "{0}={1}", item, urlEncode ? Decorator.Enclose(Decorator.Enclose(value).UrlDecode()).UrlEncode() : value);
                builder.Append(characterSeparator);
            }
        }
        if (builder.Length > 0 && separator == FieldValueSeparator.Ampersand) { builder.Remove(builder.Length - 1, 1); }
        return builder.ToString();
    }

    internal static char GetSeparator(FieldValueSeparator separator)
    {
        switch (separator)
        {
            case FieldValueSeparator.Ampersand:
                return '&';
            case FieldValueSeparator.Semicolon:
                return ';';
        }
        throw new InvalidEnumArgumentException(nameof(separator), (int)separator, typeof(FieldValueSeparator));
    }
}
