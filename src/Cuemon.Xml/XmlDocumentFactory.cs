using System;
using System.IO;
using System.Xml;

namespace Cuemon.Xml;
/// <summary>
/// Provides access to factory methods for creating and configuring <see cref="XmlDocument"/> instances.
/// </summary>
public static class XmlDocumentFactory
{
    /// <summary>
    /// Creates and returns an instance of <see cref="XmlDocument"/> from the specified <paramref name="stream"/>.
    /// </summary>
    /// <param name="stream">The <see cref="Stream"/> to convert.</param>
    /// <param name="leaveOpen">if <c>true</c>, the <see cref="Stream"/> object is being left open; otherwise it is being closed and disposed.</param>
    /// <returns>An <see cref="XmlDocument"/> initialized with the XML provided by <paramref name="stream"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="stream"/> cannot be null.
    /// </exception>
    public static XmlDocument CreateDocument(Stream stream, bool leaveOpen = false)
    {
        Validator.ThrowIfNull(stream);
        long startPosition = -1;
        if (stream.CanSeek)
        {
            startPosition = stream.Position;
            stream.Position = 0;
        }

        var document = new XmlDocument();
        if (leaveOpen)
        {
            document.Load(stream);
            if (stream.CanSeek) { stream.Seek(startPosition, SeekOrigin.Begin); }
        }
        else
        {
            using (stream) { document.Load(stream); }
        }

        return document;
    }

    /// <summary>
    /// Creates and returns an instance of <see cref="XmlDocument"/> from the specified <paramref name="reader"/>.
    /// </summary>
    /// <param name="reader">The <see cref="XmlReader"/> to convert.</param>
    /// <param name="leaveOpen">if <c>true</c>, the <see cref="XmlReader"/> object is being left open; otherwise it is being closed and disposed.</param>
    /// <returns>An <see cref="XmlDocument"/> initialized with the XML provided by <paramref name="reader"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="reader"/> cannot be null.
    /// </exception>
    public static XmlDocument CreateDocument(XmlReader reader, bool leaveOpen = false)
    {
        var document = new XmlDocument();
        if (leaveOpen)
        {
            document.Load(reader);
        }
        else
        {
            using (reader) { document.Load(reader); }
        }
        return document;
    }

    /// <summary>
    /// Creates and returns an instance of <see cref="XmlDocument"/> from the specified <paramref name="uriLocation"/>.
    /// </summary>
    /// <param name="uriLocation">The <see cref="Uri"/> to convert.</param>
    /// <returns>An <see cref="XmlDocument"/> initialized with the XML provided by <paramref name="uriLocation"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="uriLocation"/> cannot be null.
    /// </exception>
    public static XmlDocument CreateDocument(Uri uriLocation)
    {
        Validator.ThrowIfNull(uriLocation);
        var document = new XmlDocument();
        document.Load(new StringReader(uriLocation.ToString()));
        return document;
    }

    /// <summary>
    /// Creates and returns an instance of <see cref="XmlDocument"/> from the specified <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The <see cref="string"/> to be convert.</param>
    /// <returns>An <see cref="XmlDocument"/> initialized with the XML provided by <paramref name="value"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> cannot be null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="value"/> cannot be empty or consist only of white-space characters.
    /// </exception>
    public static XmlDocument CreateDocument(string value)
    {
        Validator.ThrowIfNullOrWhitespace(value);
        var document = new XmlDocument();
        try
        {
            document.LoadXml(value);
        }
        catch (XmlException ex)
        {
            throw new ArgumentException("Unable to load XML - this is typical because you are trying to load a file. Use the overloaded method that takes a URI as parameter instead.", nameof(value), ex);
        }
        return document;
    }
}
