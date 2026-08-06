using System;
using System.IO;
using System.Xml;
using Cuemon.Xml.Serialization.Converters;

namespace Cuemon.Xml.Serialization;
/// <summary>
/// Serializes and deserializes objects into and from the XML format.
/// </summary>
public class XmlSerializer
{
    /// <summary>
    /// Creates a new <see cref="XmlSerializer"/> instance using the specified <see cref="XmlSerializerOptions"/>.
    /// </summary>
    /// <param name="settings">The settings to be applied to the <see cref="XmlSerializer"/>.</param>
    /// <returns>
    /// A new <see cref="XmlSerializer"/> instance using the specified <see cref="XmlSerializerOptions"/>.
    /// </returns>
    /// <remarks>If <paramref name="settings"/> is <c>null</c>, <see cref="XmlConvert.DefaultSettings"/> is tried invoked. Otherwise, as a fallback, a default instance of <seealso cref="XmlSerializerOptions"/> is created.</remarks>
    public static XmlSerializer Create(XmlSerializerOptions settings)
    {
        var defaultSetup = settings ?? XmlConvert.DefaultSettings?.Invoke();
        return new XmlSerializer(defaultSetup ?? new XmlSerializerOptions());
    }

    /// <summary>
    /// Serializes the specified <paramref name="value"/> to a <see cref="Stream" />.
    /// </summary>
    /// <param name="value">The object to serialize to XML format.</param>
    /// <param name="objectType">The type of the object to serialize.</param>
    /// <returns>A stream of the serialized object.</returns>
    public Stream Serialize(object value, Type objectType)
    {
        return XmlStreamFactory.CreateStream(writer =>
        {
            Serialize(writer, value, objectType);
        }, settings =>
        {
            settings.Encoding = Settings.Writer.Encoding;
            settings.OmitXmlDeclaration = Settings.Writer.OmitXmlDeclaration;
            settings.CheckCharacters = Settings.Writer.CheckCharacters;
            settings.CloseOutput = Settings.Writer.CloseOutput;
            settings.ConformanceLevel = Settings.Writer.ConformanceLevel;
            settings.Indent = Settings.Writer.Indent;
            settings.IndentChars = Settings.Writer.IndentChars;
            settings.NamespaceHandling = Settings.Writer.NamespaceHandling;
            settings.NewLineChars = Settings.Writer.NewLineChars;
            settings.NewLineHandling = Settings.Writer.NewLineHandling;
            settings.NewLineOnAttributes = Settings.Writer.NewLineOnAttributes;
            settings.WriteEndDocumentOnClose = Settings.Writer.WriteEndDocumentOnClose;
            settings.Async = Settings.Writer.Async;
        });
    }

    internal void Serialize(XmlWriter writer, object value, Type objectType)
    {
        GetWriterConverter(objectType).WriteXml(writer, value);
    }

    /// <summary>
    /// Deserializes the specified <paramref name="value"/> into an object of <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the object to deserialize.</typeparam>
    /// <param name="value">The object to deserialize from XML format.</param>
    /// <returns>An object of <typeparamref name="T"/>.</returns>
    public T Deserialize<T>(Stream value)
    {
        return (T)Deserialize(value, typeof(T));
    }

    /// <summary>
    /// Deserializes the specified <paramref name="value" /> into an object of <paramref name="objectType"/>.
    /// </summary>
    /// <param name="value">The object to deserialize from XML format.</param>
    /// <param name="objectType">The type of the object to deserialize.</param>
    /// <returns>An object of <paramref name="objectType"/>.</returns>
    public object Deserialize(Stream value, Type objectType)
    {
        using (var reader = Decorator.Enclose(value).ToXmlReader(null, settings =>
        {
            settings.ConformanceLevel = Settings.Reader.ConformanceLevel;
            settings.IgnoreComments = Settings.Reader.IgnoreComments;
            settings.IgnoreProcessingInstructions = Settings.Reader.IgnoreProcessingInstructions;
            settings.IgnoreWhitespace = Settings.Reader.IgnoreWhitespace;
            settings.LineNumberOffset = Settings.Reader.LineNumberOffset;
            settings.LinePositionOffset = Settings.Reader.LinePositionOffset;
            settings.MaxCharactersFromEntities = Settings.Reader.MaxCharactersFromEntities;
            settings.MaxCharactersInDocument = Settings.Reader.MaxCharactersInDocument;
            settings.NameTable = Settings.Reader.NameTable;
            settings.Async = Settings.Reader.Async;
            settings.DtdProcessing = Settings.Reader.DtdProcessing;
            settings.CloseInput = Settings.Reader.CloseInput;
            settings.CheckCharacters = Settings.Reader.CheckCharacters;
        }))
        {
            return Deserialize(reader, objectType);
        }
    }

    private object Deserialize(XmlReader reader, Type objectType)
    {
        return GetReaderConverter(objectType).ReadXml(reader, objectType);
    }

    private XmlSerializer(XmlSerializerOptions settings)
    {
        Settings = settings ?? new XmlSerializerOptions();
    }

    internal XmlSerializerOptions Settings { get; }

    internal XmlConverter GetReaderConverter(Type objectType)
    {
        var converter = Decorator.Enclose(Settings.Converters).FirstOrDefaultReaderConverter(objectType);
        return converter ?? new DefaultXmlConverter(Settings.RootName, Settings.Converters);
    }

    internal XmlConverter GetWriterConverter(Type objectType)
    {
        var converter = Decorator.Enclose(Settings.Converters).FirstOrDefaultWriterConverter(objectType);
        if (converter is DynamicXmlConverterCore dc)
        {
            if (Settings.RootName != null && dc.RootName == null) { dc.RootName = Settings.RootName; }
            return dc;
        }
        return converter ?? new DefaultXmlConverter(Settings.RootName, Settings.Converters);
    }
}
