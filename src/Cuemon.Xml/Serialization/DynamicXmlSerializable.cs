using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Cuemon.Xml.Serialization
{
    /// <summary>
    /// Provides a factory based way to create and wrap an <see cref="IXmlSerializable"/> implementation.
    /// </summary>
    public static class DynamicXmlSerializable
    {
        /// <summary>
        /// Creates a dynamic instance of an <see cref="IXmlSerializable"/> implementation wrapping <see cref="IXmlSerializable.WriteXml"/> through <paramref name="writer"/>, <see cref="IXmlSerializable.ReadXml"/> through <paramref name="reader"/> and <see cref="IXmlSerializable.GetSchema"/> through <paramref name="schema"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <paramref name="source"/> to implement an <see cref="IXmlSerializable"/>.</typeparam>
        /// <param name="source">The object that needs support for an <see cref="IXmlSerializable"/> implementation.</param>
        /// <param name="writer">The delegate that converts <paramref name="source"/> to its XML representation.</param>
        /// <param name="reader">The delegate that generates <paramref name="source"/> from its XML representation.</param>
        /// <param name="schema">The function delegate that can provide a schema of the <paramref name="source"/>.</param>
        /// <returns>An <see cref="IXmlSerializable"/> implementation of <paramref name="source"/>.</returns>
        public static IXmlSerializable Create<T>(T source, Action<XmlWriter, T> writer, Action<XmlReader> reader = null, Func<XmlSchema> schema = null)
        {
            Validator.ThrowIfNull(source);
            return new DynamicXmlSerializable<T>(source, writer, reader, schema);
        }
    }

    internal sealed class DynamicXmlSerializable<T> : IXmlSerializable
    {
        internal DynamicXmlSerializable(T source, Action<XmlWriter, T> writer, Action<XmlReader> reader, Func<XmlSchema> schema)
        {
            Writer = writer;
            Reader = reader;
            Schema = schema;
            Source = source;
        }

        private T Source { get; }

        private Func<XmlSchema> Schema { get; }

        private Action<XmlReader> Reader { get; }

        private Action<XmlWriter, T> Writer { get; }

        /// <summary>
        /// This method is reserved and should not be used. When implementing the IXmlSerializable interface, you should return null (Nothing in Visual Basic) from this method, and instead, if specifying a custom schema is required, apply the <see cref="XmlSchemaProviderAttribute" /> to the class.
        /// </summary>
        /// <returns>An <see cref="XmlSchema" /> that describes the XML representation of the object that is produced by the <see cref="IXmlSerializable.WriteXml(XmlWriter)" /> method and consumed by the <see cref="IXmlSerializable.ReadXml(XmlReader)" /> method.</returns>
        public XmlSchema GetSchema()
        {
            if (Schema == null) { throw new NotImplementedException(); }
            return Schema();
        }

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The <see cref="XmlReader" /> stream from which the object is deserialized.</param>
        public void ReadXml(XmlReader reader)
        {
            if (Reader == null) { throw new NotImplementedException(); }
            Reader(reader);
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="XmlWriter" /> stream to which the object is serialized.</param>
        public void WriteXml(XmlWriter writer)
        {
            if (Writer == null) { throw new NotImplementedException(); }
            Writer(writer, Source);
        }
    }
}
