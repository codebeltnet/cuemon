using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Cuemon.Extensions.Runtime;
using Cuemon.Xml;

namespace Cuemon.Extensions.Xml
{
    /// <summary>
    /// Extension methods for the <see cref="XmlReader"/> class.
    /// </summary>
    public static class XmlReaderExtensions
    {
        /// <summary>
        /// Converts the XML hierarchy of the <paramref name="reader"/> into an <see cref="IHierarchy{T}"/>.
        /// </summary>
        /// <param name="reader">The <see cref="XmlReader"/> to extend.</param>
        /// <returns>An <see cref="IHierarchy{DataPair}"/> implementation.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="reader"/> cannot be null.
        /// </exception>
        public static IHierarchy<DataPair> ToHierarchy(this XmlReader reader)
        {
            Validator.ThrowIfNull(reader);
            return Decorator.Enclose(reader).ToHierarchy();
        }

        /// <summary>
        /// Creates and returns a sequence of chunked <see cref="XmlReader"/> instances with a maximum of the specified <paramref name="size"/> of XML node elements located on a depth of 1.
        /// </summary>
        /// <param name="reader">The <see cref="XmlReader"/> to extend.</param>
        /// <param name="size">The amount of XML node elements allowed per <see cref="XmlReader"/> object. Default is 128 XML node element.</param>
        /// <param name="setup">The <see cref="XmlWriterSettings"/> which may be configured.</param>
        /// <returns>An sequence of <see cref="XmlReader"/> instances that contains no more than the specified <paramref name="size"/> of XML node elements from the <paramref name="reader" /> object.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="reader"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The <see cref="XmlReader.Read"/> method of the <paramref name="reader"/> object has already been called.
        /// </exception>
        public static IEnumerable<XmlReader> Chunk(this XmlReader reader, int size = 128, Action<XmlWriterSettings> setup = null)
        {
            Validator.ThrowIfNull(reader);
            Validator.ThrowIfTrue(reader.ReadState != ReadState.Initial, nameof(reader), "The Read method of the XmlReader object has already been called.");
            return Decorator.Enclose(reader).Chunk(size, setup);
        }

        /// <summary>
        /// Copies everything from the specified <paramref name="reader"/> and returns the result as an XML stream.
        /// </summary>
        /// <param name="reader">The <see cref="XmlReader"/> to extend.</param>
        /// <param name="setup">The <see cref="XmlCopyOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Stream"/> holding an exact copy of the source <paramref name="reader"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="reader"/> is null.
        /// </exception>
        public static Stream ToStream(this XmlReader reader, Action<XmlCopyOptions> setup = null)
        {
            return ToStream(reader, (w, r, o) =>
            {
                try
                {
                    while (r.Read())
                    {
                        switch (r.NodeType)
                        {
                            case XmlNodeType.CDATA:
                                w.WriteCData(r.Value);
                                break;
                            case XmlNodeType.Comment:
                                w.WriteComment(r.Value);
                                break;
                            case XmlNodeType.DocumentType:
                                w.WriteDocType(r.Name, r.GetAttribute("PUBLIC"), r.GetAttribute("SYSTEM"), r.Value);
                                break;
                            case XmlNodeType.Element:
                                w.WriteStartElement(r.Prefix, r.LocalName, r.NamespaceURI);
                                w.WriteAttributes(r, true);
                                if (r.IsEmptyElement) { w.WriteEndElement(); }
                                break;
                            case XmlNodeType.EndElement:
                                w.WriteFullEndElement();
                                break;
                            case XmlNodeType.Attribute:
                            case XmlNodeType.Document:
                            case XmlNodeType.DocumentFragment:
                            case XmlNodeType.EndEntity:
                            case XmlNodeType.None:
                            case XmlNodeType.Notation:
                            case XmlNodeType.Entity:
                                break;
                            case XmlNodeType.EntityReference:
                                w.WriteEntityRef(r.Name);
                                break;
                            case XmlNodeType.Whitespace:
                            case XmlNodeType.SignificantWhitespace:
                                w.WriteWhitespace(r.Value);
                                break;
                            case XmlNodeType.Text:
                                w.WriteString(r.Value);
                                break;
                            case XmlNodeType.ProcessingInstruction:
                            case XmlNodeType.XmlDeclaration:
                                w.WriteProcessingInstruction(r.Name, r.Value);
                                break;
                        }
                    }
                }
                finally
                {
                    if (!o.LeaveOpen) { r?.Dispose(); }
                }

            }, setup);
        }

        /// <summary>
        /// Copies the specified <paramref name="reader"/> using the specified delegate <paramref name="copier"/> and returns the result as an XML stream.
        /// </summary>
        /// <param name="reader">The <see cref="XmlReader"/> to extend.</param>
        /// <param name="copier">The delegate that will create an in-memory copy of <paramref name="reader"/> as a XML stream.</param>
        /// <param name="setup">The <see cref="XmlCopyOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Stream"/> holding the XML copied by the delegate <paramref name="copier"/> from the source <paramref name="reader"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="reader"/> is null - or - <paramref name="copier"/> is null.
        /// </exception>
        public static Stream ToStream(this XmlReader reader, Action<XmlWriter, XmlReader, DisposableOptions> copier, Action<XmlCopyOptions> setup = null)
        {
            Validator.ThrowIfNull(reader);
            Validator.ThrowIfNull(copier);
            var options = Patterns.Configure(setup);
            return XmlStreamFactory.CreateStream(writer => copier(writer, reader, options), options.WriterSettings);
        }

        /// <summary>
        /// Moves the <paramref name="reader"/> to the first element.
        /// </summary>
        /// <param name="reader">The <see cref="XmlReader"/> to extend.</param>
        /// <returns><c>true</c> if an element exists (the reader moves to the first element), otherwise, <c>false</c> (the reader has reached <see cref="XmlReader.EOF"/>).</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="reader"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The <see cref="XmlReader.Read"/> method of the <paramref name="reader"/> object has already been called.
        /// </exception>
        public static bool MoveToFirstElement(this XmlReader reader)
        {
            Validator.ThrowIfNull(reader);
            return Decorator.Enclose(reader).MoveToFirstElement();
        }
    }
}