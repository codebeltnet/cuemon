using System;
using Cuemon.Xml;

namespace Cuemon.Extensions.Xml;
/// <summary>
/// Extension methods for the <see cref="string"/> class.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Escapes the given XML <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The <see cref="string"/> to extend.</param>
    /// <returns>The input <paramref name="value"/> with an escaped equivalent.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> cannot be null.
    /// </exception>
    public static string EscapeXml(this string value)
    {
        Validator.ThrowIfNull(value);
        return Decorator.Enclose(value).EscapeXml();
    }

    /// <summary>
    /// Unescapes the given XML <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The <see cref="string"/> to extend.</param>
    /// <returns>The input <paramref name="value"/> with an unescaped equivalent.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> cannot be null.
    /// </exception>
    public static string UnescapeXml(this string value)
    {
        Validator.ThrowIfNull(value);
        return Decorator.Enclose(value).UnescapeXml();
    }

    /// <summary>
    /// Sanitizes the <paramref name="value"/> for any invalid characters.
    /// </summary>
    /// <param name="value">The <see cref="string"/> to extend.</param>
    /// <returns>A sanitized <see cref="string"/> of <paramref name="value"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> cannot be null.
    /// </exception>
    /// <remarks>Sanitation rules are as follows:<br/>
    /// 1. Names can contain letters, numbers, and these 4 characters: _ | : | . | -<br/>
    /// 2. Names cannot start with a number or punctuation character<br/>
    /// 3. Names cannot contain spaces<br/>
    /// </remarks>
    public static string SanitizeXmlElementName(this string value)
    {
        Validator.ThrowIfNull(value);
        return Decorator.Enclose(value).SanitizeXmlElementName();
    }

    /// <summary>
    /// Sanitizes the <paramref name="value"/> for any invalid characters.
    /// </summary>
    /// <param name="value">The <see cref="string"/> to extend.</param>
    /// <param name="cdataSection">if set to <c>true</c> supplemental CDATA-section rules is applied to <paramref name="value"/>.</param>
    /// <returns>A sanitized <see cref="string"/> of <paramref name="value"/>.</returns>
    /// <remarks>Sanitation rules are as follows:<br/>
    /// 1. The <paramref name="value"/> cannot contain characters less or equal to a Unicode value of U+0019 (except U+0009, U+0010, U+0013)<br/>
    /// 2. The <paramref name="value"/> cannot contain the string "]]&lt;" if <paramref name="cdataSection"/> is <c>true</c>.<br/>
    /// </remarks>
    public static string SanitizeXmlElementText(this string value, bool cdataSection = false)
    {
        if (string.IsNullOrEmpty(value)) { return value; }
        return Decorator.Enclose(value).SanitizeXmlElementText(cdataSection);
    }
}
