using System;
using System.Xml;
using System.Xml.Linq;

namespace Cuemon.Xml.Linq;
/// <summary>
/// Extension methods for the <see cref="string"/> class hidden behind the <see cref="IDecorator{T}"/> interface.
/// </summary>
/// <seealso cref="IDecorator{T}"/>
/// <seealso cref="Decorator{T}"/>
public static class StringDecoratorExtensions
{
    /// <summary>
    /// Tries to load an <see cref="XElement" /> from the enclosed <see cref="string"/> of the specified <paramref name="decorator"/> that contains XML.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{String}"/> to extend.</param>
    /// <param name="result">When this method returns, it contains the <see cref="XElement"/> populated from the enclosed <see cref="string"/> of the specified <paramref name="decorator"/> that contains XML, if the conversion succeeded, or a null reference if the conversion failed.</param>
    /// <returns><c>true</c> if the enclosed <see cref="string"/> of the specified <paramref name="decorator"/> was converted successfully; otherwise, <c>false</c>.</returns>
    public static bool TryParseXElement(this IDecorator<string> decorator, out XElement result)
    {
        return TryParseXElement(decorator, LoadOptions.None, out result);
    }

    /// <summary>
    /// Tries to load an <see cref="XElement" /> from the enclosed <see cref="string"/> of the specified <paramref name="decorator"/> that contains XML, optionally preserving white space and retaining line information.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{String}"/> to extend.</param>
    /// <param name="options">A <see cref="LoadOptions"/> that specifies white space behavior, and whether to load base URI and line information.</param>
    /// <param name="result">When this method returns, it contains the <see cref="XElement"/> populated from the enclosed <see cref="string"/> of the specified <paramref name="decorator"/> that contains XML, if the conversion succeeded, or a null reference if the conversion failed.</param>
    /// <returns><c>true</c> if the enclosed <see cref="string"/> of the specified <paramref name="decorator"/> was converted successfully; otherwise, <c>false</c>.</returns>
    public static bool TryParseXElement(this IDecorator<string> decorator, LoadOptions options, out XElement result)
    {
        result = null;
        if (string.IsNullOrWhiteSpace(decorator.Inner)) { return false; }
        if (decorator.Inner.StartsWith("<", StringComparison.Ordinal))
        {
            try
            {
                result = XElement.Parse(decorator.Inner, options);
                return true;
            }
            catch (XmlException)
            {
                // ignored as we are in a TryParse method
            }
        }
        return false;
    }

    /// <summary>
    /// Determines whether the enclosed <see cref="string"/> of the specified <paramref name="decorator"/> is a valid XML string.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{String}"/> to extend.</param>
    /// <returns><c>true</c> if the enclosed <see cref="string"/> of the specified <paramref name="decorator"/> is a valid XML string; otherwise, <c>false</c>.</returns>
    public static bool IsXmlString(this IDecorator<string> decorator)
    {
        return TryParseXElement(decorator, out _);
    }
}
