using System;
using System.IO;
using System.Xml;
using Cuemon.Runtime.Serialization.Formatters;
using Cuemon.Xml.Serialization.Converters;

namespace Cuemon.Xml.Serialization.Formatters;
/// <summary>
/// Serializes and deserializes an object in XML format.
/// </summary>
/// <seealso cref="Formatter{TFormat}" />.
/// <seealso cref="DefaultXmlConverter"/>.
public class XmlFormatter : StreamFormatter<XmlFormatterOptions>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="XmlFormatter"/> class.
    /// </summary>
    public XmlFormatter() : this((Action<XmlFormatterOptions>)null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XmlFormatter"/> class.
    /// </summary>
    /// <param name="setup">The <see cref="XmlFormatterOptions"/> which need to be configured.</param>
    public XmlFormatter(Action<XmlFormatterOptions> setup) : this(Patterns.Configure(setup))
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XmlFormatter"/> class.
    /// </summary>
    /// <param name="options">The configured <see cref="XmlFormatterOptions"/>.</param>
    public XmlFormatter(XmlFormatterOptions options) : base(options)
    {
        Options.RefreshWithConverterDependencies();
        if (Options.SynchronizeWithXmlConvert) { Decorator.Enclose(Options.Settings).ApplyToDefaultSettings(); }
    }

    /// <summary>
    /// Serializes the specified <paramref name="source"/> to an object of <see cref="Stream"/>.
    /// </summary>
    /// <param name="source">The object to serialize to XML format.</param>
    /// <param name="objectType">The type of the object to serialize.</param>
    /// <returns>A stream of the serialized <paramref name="source"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source"/> cannot be null -or-
    /// <paramref name="objectType"/> cannot be null.
    /// </exception>
    public override Stream Serialize(object source, Type objectType)
    {
        Validator.ThrowIfNull(source);
        Validator.ThrowIfNull(objectType);
        var serializer = XmlSerializer.Create(Options.Settings);
        return serializer.Serialize(source, objectType);
    }

    /// <summary>
    /// Serializes the specified <paramref name="source"/>  into an XML format.
    /// </summary>
    /// <param name="writer">The writer used in the serialization process.</param>
    /// <param name="source">The object to serialize to XML format.</param>
    /// <param name="objectType">The type of the object to serialize.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="writer"/> cannot be null -or-
    /// <paramref name="source"/> cannot be null -or-
    /// <paramref name="objectType"/> cannot be null.
    /// </exception>
    public void SerializeToWriter(XmlWriter writer, object source, Type objectType)
    {
        Validator.ThrowIfNull(writer);
        Validator.ThrowIfNull(source);
        Validator.ThrowIfNull(objectType);
        var serializer = XmlSerializer.Create(Options.Settings);
        serializer.Serialize(writer, source, objectType);
    }

    /// <summary>
    /// Deserializes the specified <paramref name="value" /> into an object of <paramref name="objectType"/>.
    /// </summary>
    /// <param name="value">The stream from which to deserialize the object graph.</param>
    /// <param name="objectType">The type of the deserialized object.</param>
    /// <returns>An object of <paramref name="objectType"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> cannot be null -or-
    /// <paramref name="objectType"/> cannot be null.
    /// </exception>
    public override object Deserialize(Stream value, Type objectType)
    {
        Validator.ThrowIfNull(value);
        Validator.ThrowIfNull(objectType);
        var serializer = XmlSerializer.Create(Options.Settings);
        return serializer.Deserialize(value, objectType);
    }
}
