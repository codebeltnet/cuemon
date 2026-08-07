using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using Cuemon.Xml;
using Cuemon.Xml.XPath;

namespace Cuemon.Extensions.Xml;
/// <summary>
/// Extension methods for the <see cref="Stream"/> class.
/// </summary>
public static class StreamExtensions
{
    /// <summary>
    /// Converts the given <paramref name="value"/> to an <see cref="XmlReader"/>.
    /// </summary>
    /// <param name="value">The XML <see cref="Stream"/> to extend.</param>
    /// <param name="encoding">The text encoding to use.</param>
    /// <param name="setup">The <see cref="XmlReaderSettings"/> which may be configured.</param>
    /// <returns>An <see cref="XmlReader"/> representation of <paramref name="value"/>.</returns>
    /// <remarks>If <paramref name="encoding"/> is null, an <see cref="Encoding"/> object will be attempted resolved by <see cref="TryDetectXmlEncoding"/>.</remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> cannot be null.
    /// </exception>
    public static XmlReader ToXmlReader(this Stream value, Encoding encoding = null, Action<XmlReaderSettings> setup = null)
    {
        Validator.ThrowIfNull(value);
        return Decorator.Enclose(value).ToXmlReader(encoding, setup);
    }

    /// <summary>
    /// Copies the entire XML <see cref="Stream"/> following the output format of <paramref name="setup"/>.
    /// </summary>
    /// <param name="value">The XML <see cref="Stream"/> to extend.</param>
    /// <param name="setup">The <see cref="XmlWriterSettings"/> which may be configured.</param>
    /// <returns>A <see cref="Stream"/> that is equivalent to <paramref name="value"/> following the output format of <paramref name="setup"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> cannot be null.
    /// </exception>
    public static Stream CopyXmlStream(this Stream value, Action<XmlWriterSettings> setup = null)
    {
        Validator.ThrowIfNull(value);

        long startingPosition = -1;
        if (value.CanSeek)
        {
            startingPosition = value.Position;
            value.Position = 0;
        }

        var options = Patterns.CreateInstance(setup);
        return Patterns.SafeInvoke(() => new MemoryStream(), ms =>
        {
            var document = new XmlDocument();
            document.Load(value);
            using (var writer = XmlWriter.Create(ms, options))
            {
                document.Save(writer);
                writer.Flush();
            }

            if (value.CanSeek) { value.Seek(startingPosition, SeekOrigin.Begin); } // reset to original position
            ms.Position = 0;
            return ms;
        });
    }

    /// <summary>
    /// Tries to resolve the <see cref="Encoding"/> level of the XML document from the <see cref="Stream"/>.
    /// </summary>
    /// <param name="value">The XML <see cref="Stream"/> to extend.</param>
    /// <param name="result">When this method returns, it contains the <see cref="Encoding"/> value equivalent to the encoding level of the XML document contained in <paramref name="value"/>, if the conversion succeeded, or a null reference (Nothing in Visual Basic) if the conversion failed. The conversion fails if the <paramref name="value"/> parameter is null, does not contain BOM information or does not contain an <see cref="XmlDeclaration"/>.</param>
    /// <returns><c>true</c> if the <paramref name="value"/> parameter was converted successfully; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> cannot be null.
    /// </exception>
    public static bool TryDetectXmlEncoding(this Stream value, out Encoding result)
    {
        Validator.ThrowIfNull(value);
        return Decorator.Enclose(value).TryDetectXmlEncoding(out result);
    }

    /// <summary>
    /// Remove the XML namespace declarations from the specified <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The XML <see cref="Stream"/> to extend.</param>
    /// <param name="setup">The <see cref="XmlWriterSettings"/> which may be configured.</param>
    /// <returns>A <see cref="Stream"/> object representing the specified <paramref name="value"/>, but with no namespace declarations.</returns>
    public static Stream RemoveXmlNamespaceDeclarations(this Stream value, Action<XmlWriterSettings> setup = null)
    {
        Validator.ThrowIfNull(value);
        var options = Patterns.CreateInstance(setup);
        var navigable = XPathDocumentFactory.CreateDocument(value, true);
        var navigator = navigable.CreateNavigator();
        return Patterns.SafeInvoke(() => new MemoryStream(), ms =>
        {
            using (var writer = XmlWriter.Create(ms, options))
            {
                WriteElements(navigator, writer);
                writer.Flush();
            }
            ms.Position = 0;
            return ms;
        });
    }

    private static void WriteAttributes(XPathNavigator navigator, XmlWriter writer)
    {
        var attributeIterator = navigator.Select("@*");
        while (attributeIterator.MoveNext())
        {
            writer.WriteAttributeString(attributeIterator.Current.Prefix, attributeIterator.Current.LocalName, null, attributeIterator.Current.Value);
        }
    }

    private static void WriteElements(XPathNavigator navigator, XmlWriter writer)
    {
        var childrenIterator = navigator.Select("*");
        while (childrenIterator.MoveNext())
        {
            writer.WriteStartElement(childrenIterator.Current.LocalName);
            WriteAttributes(childrenIterator.Current, writer);
            if (childrenIterator.Current.SelectSingleNode("text()") != null) { writer.WriteString(childrenIterator.Current.SelectSingleNode("text()").Value); }
            WriteElements(childrenIterator.Current, writer);
            writer.WriteEndElement();
        }
    }
}
