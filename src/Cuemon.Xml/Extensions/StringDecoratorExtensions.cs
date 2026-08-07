using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cuemon.Xml;
/// <summary>
/// Extension methods for the <see cref="string"/> class hidden behind the <see cref="IDecorator{T}"/> interface.
/// </summary>
/// <seealso cref="IDecorator{T}"/>
/// <seealso cref="Decorator{T}"/>
public static class StringDecoratorExtensions
{
    private static readonly string[][] EscapeStringPairs = new[] { new[] { "&lt;", "&gt;", "&quot;", "&apos;", "&amp;" }, new[] { "<", ">", "\"", "'", "&" } };
    private static readonly char[] AdditionalInclusiveChars = new[] { '_', ':', '.', '-' };
    private static readonly char[] DotExclusiveChar = new[] { '.' };
    private static readonly string[] DotExclusiveString = new[] { "." };

    /// <summary>
    /// Escapes the given XML of the enclosed <see cref="string"/> of the specified <paramref name="decorator"/>.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{String}"/> to extend.</param>
    /// <returns>An escaped equivalent of the enclosed <see cref="string"/> of the specified <paramref name="decorator"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static string EscapeXml(this IDecorator<string> decorator)
    {
        Validator.ThrowIfNull(decorator);
        var replacePairs = new List<StringReplacePair>();
        for (byte b = 0; b < EscapeStringPairs[0].Length; b++)
        {
            replacePairs.Add(new StringReplacePair(EscapeStringPairs[1][b], EscapeStringPairs[0][b]));
        }
        return StringReplacePair.ReplaceAll(decorator.Inner, replacePairs, StringComparison.Ordinal);
    }

    /// <summary>
    /// Unescapes the given XML of the enclosed <see cref="string"/> of the specified <paramref name="decorator"/>.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{String}"/> to extend.</param>
    /// <returns>An unescaped equivalent of the enclosed <see cref="string"/> of the specified <paramref name="decorator"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static string UnescapeXml(this IDecorator<string> decorator)
    {
        Validator.ThrowIfNull(decorator);
        var builder = new StringBuilder(decorator.Inner);
        for (byte b = 0; b < EscapeStringPairs[0].Length; b++)
        {
            builder.Replace(EscapeStringPairs[0][b], EscapeStringPairs[1][b]);
        }
        return builder.ToString();
    }

    /// <summary>
    /// Sanitizes the enclosed <see cref="string"/> of the specified <paramref name="decorator"/> for any invalid characters.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{String}"/> to extend.</param>
    /// <returns>A sanitized <see cref="string"/> of the enclosed <see cref="string"/> of the specified <paramref name="decorator"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    /// <remarks>Sanitation rules are as follows:<br/>
    /// 1. Names can contain letters, numbers, and these 4 characters: _ | : | . | -<br/>
    /// 2. Names cannot start with a number or punctuation character<br/>
    /// 3. Names cannot contain spaces<br/>
    /// </remarks>
    public static string SanitizeXmlElementName(this IDecorator<string> decorator)
    {
        Validator.ThrowIfNull(decorator);
        var value = decorator.Inner;
        if (Decorator.Enclose(value).StartsWith(StringComparison.OrdinalIgnoreCase, Decorator.Enclose(Alphanumeric.Numbers).ToEnumerable().Concat(DotExclusiveString)))
        {
            var startIndex = 0;
            var numericsAndPunctual = new List<char>(Alphanumeric.Numbers.ToCharArray().Concat(DotExclusiveChar));
            foreach (var c in value)
            {
                if (numericsAndPunctual.Contains(c))
                {
                    startIndex++;
                    continue;
                }
                break;
            }
            return SanitizeXmlElementName(Decorator.Enclose(value.Substring(startIndex)));
        }

        var validElementName = new StringBuilder();
        foreach (var c in value)
        {
            var validCharacters = new List<char>(Alphanumeric.LettersAndNumbers.ToCharArray().Concat(AdditionalInclusiveChars));
            if (validCharacters.Contains(c)) { validElementName.Append(c); }
        }
        return validElementName.ToString();
    }

    /// <summary>
    /// Sanitizes the enclosed <see cref="string"/> of the specified <paramref name="decorator"/> for any invalid characters.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{String}"/> to extend.</param>
    /// <param name="cdataSection">if set to <c>true</c> supplemental CDATA-section rules is applied to the enclosed <see cref="string"/> of the specified <paramref name="decorator"/>.</param>
    /// <returns>A sanitized <see cref="string"/> of the enclosed <see cref="string"/> of the specified <paramref name="decorator"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    /// <remarks>Sanitation rules are as follows:<br/>
    /// 1. The enclosed <see cref="string"/> of the specified <paramref name="decorator"/> cannot contain characters less or equal to a Unicode value of U+0019 (except U+0009, U+0010, U+0013)<br/>
    /// 2. The enclosed <see cref="string"/> of the specified <paramref name="decorator"/> cannot contain the string "]]&lt;" if <paramref name="cdataSection"/> is <c>true</c>.<br/>
    /// </remarks>
    public static string SanitizeXmlElementText(this IDecorator<string> decorator, bool cdataSection = false)
    {
        Validator.ThrowIfNull(decorator);
        var value = decorator.Inner;
        if (string.IsNullOrEmpty(value)) { return value; }
        value = StringReplacePair.RemoveAll(decorator.Inner, '\x0001', '\x0002', '\x0003', '\x0004', '\x0005', '\x0006', '\x0007', '\x0008', '\x0011', '\x0012', '\x0014', '\x0015', '\x0016', '\x0017', '\x0018', '\x0019');
        return cdataSection ? StringReplacePair.RemoveAll(value, "]]>") : value;
    }
}
