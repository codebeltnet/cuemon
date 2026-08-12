using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using Cuemon.Text;

namespace Cuemon.Xml.XPath;
/// <summary>
/// Provides access to factory methods for creating and configuring <see cref="IXPathNavigable"/> instances.
/// </summary>
public static class XPathDocumentFactory
{
    /// <summary>
    /// Creates and returns an instance of <see cref="XPathDocument"/> from the specified <paramref name="value"/>.
    /// </summary>
	/// <param name="value">The <see cref="string"/> to convert.</param>
    /// <returns>An <see cref="XPathDocument"/> initialized with the XML provided by <paramref name="value"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> cannot be null.
    /// </exception>
	public static XPathDocument CreateDocument(string value)
    {
        return CreateDocument(value, Encoding.UTF8);
    }

    /// <summary>
    /// Creates and returns an instance of <see cref="XPathDocument"/> from the specified <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The <see cref="string"/> to convert.</param>
    /// <param name="encoding">The preferred <see cref="Encoding"/> to use in the conversion.</param>
    /// <returns>An <see cref="XPathDocument"/> initialized with the XML provided by <paramref name="value"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> cannot be null -or-
    /// <paramref name="encoding"/> cannot be null.
    /// </exception>
    public static XPathDocument CreateDocument(string value, Encoding encoding)
    {
        Validator.ThrowIfNull(value);
        Validator.ThrowIfNull(encoding);
        using (var stream = Decorator.Enclose(value).ToStream(options =>
        {
            options.Encoding = encoding;
            options.Preamble = PreambleSequence.Keep;
        }))
        {
            return CreateDocument(stream);
        }
    }

    /// <summary>
    /// Creates and returns an instance of <see cref="XPathDocument"/> from the specified <paramref name="stream"/>.
    /// </summary>
    /// <param name="stream">The <see cref="Stream"/> to convert.</param>
    /// <param name="leaveOpen">if <c>true</c>, the source <see cref="Stream"/> is being left open; otherwise it is being closed and disposed.</param>
    /// <returns>An <see cref="XPathDocument"/> initialized with the XML provided by <paramref name="stream"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="stream"/> cannot be null.
    /// </exception>
    public static XPathDocument CreateDocument(Stream stream, bool leaveOpen = false)
    {
        Validator.ThrowIfNull(stream);
        if (leaveOpen)
        {
            var reader = XmlReader.Create(stream);
            var document = new XPathDocument(reader);
            stream.Position = 0;
            return document;
        }
        using (stream)
        {
            return new XPathDocument(stream);
        }
    }

    /// <summary>
    /// Creates and returns an instance of <see cref="XPathDocument"/> from the specified <paramref name="reader"/>.
    /// </summary>
    /// <param name="reader">The <see cref="XmlReader"/> to convert.</param>
    /// <returns>An <see cref="XPathDocument"/> initialized with the XML provided by <paramref name="reader"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="reader"/> cannot be null.
    /// </exception>
    public static XPathDocument CreateDocument(XmlReader reader)
    {
        Validator.ThrowIfNull(reader);
        return new XPathDocument(reader);
    }

    /// <summary>
    /// Creates and returns an instance of <see cref="XPathDocument"/> from the specified <paramref name="uriLocation"/>.
    /// </summary>
    /// <param name="uriLocation">The <see cref="Uri"/> to convert.</param>
    /// <returns>An <see cref="XPathDocument"/> initialized with the XML provided by <paramref name="uriLocation"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="uriLocation"/> cannot be null.
    /// </exception>
    public static XPathDocument CreateDocument(Uri uriLocation)
    {
        Validator.ThrowIfNull(uriLocation);
        return new XPathDocument(uriLocation.ToString());
    }
}
