using System;
using System.Xml;

namespace Cuemon.Xml.Serialization.Converters;
/// <summary>
/// Converts an object to or from XML.
/// </summary>
public abstract class XmlConverter
{
    /// <summary>
    /// Initializes a new instance of the <see cref="XmlConverter"/> class.
    /// </summary>
    protected XmlConverter()
    {
    }

    /// <summary>
    /// Writes the XML representation of the <paramref name="value" />.
    /// </summary>
    /// <param name="writer">The <see cref="XmlWriter" /> to write to.</param>
    /// <param name="value">The object to serialize.</param>
    /// <param name="elementName">The element name to encapsulate around <paramref name="value"/>.</param>
    public abstract void WriteXml(XmlWriter writer, object value, XmlQualifiedEntity elementName = null);

    /// <summary>
    /// Reads the XML representation of the <paramref name="objectType"/>.
    /// </summary>
    /// <param name="reader">The <see cref="XmlReader"/> to read from.</param>
    /// <param name="objectType">The <seealso cref="Type"/> of the object.</param>
    /// <returns>An object of <paramref name="objectType"/>.</returns>
    public abstract object ReadXml(XmlReader reader, Type objectType);

    /// <summary>
    /// Determines whether this instance can convert the specified object type.
    /// </summary>
    /// <param name="objectType">The <seealso cref="Type"/> of the object.</param>
    /// <returns><c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.</returns>
    public abstract bool CanConvert(Type objectType);

    /// <summary>
    /// Gets a value indicating whether this <seealso cref="XmlConverter"/> can read XML.
    /// </summary>
    /// <value><c>true</c> if this <seealso cref="XmlConverter"/> can read XML; otherwise, <c>false</c>.</value>
    public virtual bool CanRead => true;

    /// <summary>
    /// Gets a value indicating whether this <seealso cref="XmlConverter"/> can write XML.
    /// </summary>
    /// <value><c>true</c> if this <seealso cref="XmlConverter"/> can write XML; otherwise, <c>false</c>.</value>
    public virtual bool CanWrite => true;
}

/// <summary>
/// Converts an object to or from XML.
/// </summary>
public abstract class XmlConverter<T> : XmlConverter where T : class
{
    /// <summary>
    /// Initializes a new instance of the <see cref="XmlConverter{T}"/> class.
    /// </summary>
    protected XmlConverter()
    {
    }


    /// <summary>
    /// Reads the XML representation of the <paramref name="objectType" />.
    /// </summary>
    /// <param name="objectType">The <seealso cref="Type" /> of the object.</param>
    /// <param name="reader">The <see cref="XmlReader" /> to read from.</param>
    /// <returns>An object of <typeparamref name="T"/>.</returns>
    public abstract T ReadXml(Type objectType, XmlReader reader);

    /// <summary>
    /// Writes the XML representation of <typeparamref name="T"/>.
    /// </summary>
    /// <param name="writer">The <see cref="XmlWriter" /> to write to.</param>
    /// <param name="value">The object to serialize.</param>
    /// <param name="elementName">The element name to encapsulate around <paramref name="value" />.</param>
    public abstract void WriteXml(XmlWriter writer, T value, XmlQualifiedEntity elementName = null);

    /// <summary>
    /// Writes the XML representation of the <paramref name="value" />.
    /// </summary>
    /// <param name="writer">The <see cref="XmlWriter" /> to write to.</param>
    /// <param name="value">The object to serialize.</param>
    /// <param name="elementName">The element name to encapsulate around <paramref name="value" />.</param>
    public sealed override void WriteXml(XmlWriter writer, object value, XmlQualifiedEntity elementName = null)
    {
        WriteXml(writer, value as T, elementName);
    }

    /// <summary>
    /// Reads the XML representation of the <paramref name="objectType" />.
    /// </summary>
    /// <param name="reader">The <see cref="XmlReader" /> to read from.</param>
    /// <param name="objectType">The <seealso cref="Type" /> of the object.</param>
    /// <returns>An object of <paramref name="objectType" />.</returns>
    public sealed override object ReadXml(XmlReader reader, Type objectType)
    {
        return ReadXml(objectType, reader);
    }

    /// <summary>
    /// Determines whether this instance can convert the specified object type.
    /// </summary>
    /// <param name="objectType">The <seealso cref="Type" /> of the object.</param>
    /// <returns><c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.</returns>
    public override bool CanConvert(Type objectType)
    {
        return typeof(T).IsAssignableFrom(objectType);
    }
}
