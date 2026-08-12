using System.Xml.Linq;
using Cuemon.Xml.Linq;

namespace Cuemon.Extensions.Xml.Linq;
/// <summary>
/// Extension methods for the <see cref="string"/> class.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Tries to load an <see cref="XElement" /> from a <paramref name="value" /> that contains XML.
    /// </summary>
    /// <param name="value">The <see cref="string"/> to extend.</param>
    /// <param name="result">When this method returns, it contains the <see cref="XElement"/> populated from the <paramref name="value"/> that contains XML, if the conversion succeeded, or a null reference (Nothing in Visual Basic) if the conversion failed.</param>
    /// <returns><c>true</c> if the <paramref name="value"/> parameter was converted successfully; otherwise, <c>false</c>.</returns>
    public static bool TryParseXElement(this string value, out XElement result)
    {
        return TryParseXElement(value, LoadOptions.None, out result);
    }

    /// <summary>
    /// Tries to load an <see cref="XElement" /> from a <paramref name="value" /> that contains XML, optionally preserving white space and retaining line information.
    /// </summary>
    /// <param name="value">The <see cref="string"/> to extend.</param>
    /// <param name="options">A <see cref="LoadOptions"/> that specifies white space behavior, and whether to load base URI and line information.</param>
    /// <param name="result">When this method returns, it contains the <see cref="XElement"/> populated from the <paramref name="value"/> that contains XML, if the conversion succeeded, or a null reference (Nothing in Visual Basic) if the conversion failed.</param>
    /// <returns><c>true</c> if the <paramref name="value"/> parameter was converted successfully; otherwise, <c>false</c>.</returns>
    public static bool TryParseXElement(this string value, LoadOptions options, out XElement result)
    {
        return Decorator.Enclose(value).TryParseXElement(options, out result);
    }

    /// <summary>
    /// Determines whether the specified <paramref name="value"/> is a valid XML string.
    /// </summary>
    /// <param name="value">The <see cref="string"/> to extend.</param>
    /// <returns><c>true</c> if the specified <paramref name="value"/> is a valid XML string; otherwise, <c>false</c>.</returns>
    public static bool IsXmlString(this string value)
    {
        return TryParseXElement(value, out _);
    }
}
