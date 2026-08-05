using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using Cuemon.Text;
using Cuemon.Collections.Generic;
using Cuemon.Extensions.Runtime;
using Cuemon.Xml.Serialization;

namespace Cuemon.Xml
{
    /// <summary>
    /// Extension methods for the <see cref="XmlReader"/> class hidden behind the <see cref="IDecorator{T}"/> interface.
    /// </summary>
    /// <seealso cref="IDecorator{T}"/>
    /// <seealso cref="Decorator{T}"/>
    public static class XmlReaderDecoratorExtensions
    {
        /// <summary>
        /// Creates and returns a sequence of chunked <see cref="XmlReader"/> instances from the enclosed <see cref="XmlReader"/> of the specified <paramref name="decorator"/> with a maximum of the specified <paramref name="size"/> of XML node elements located on a depth of 1.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{XmlReader}"/> to extend.</param>
        /// <param name="size">The amount of XML node elements allowed per <see cref="XmlReader"/> object. Default is 128 XML node element.</param>
        /// <param name="setup">The <see cref="XmlWriterSettings"/> which may be configured.</param>
        /// <returns>An sequence of <see cref="XmlReader"/> instances that contains no more than the specified <paramref name="size"/> of XML node elements from the enclosed <see cref="XmlReader"/> of the specified <paramref name="decorator"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The <see cref="XmlReader.Read"/> method of the enclosed <see cref="XmlReader"/> of the specified <paramref name="decorator"/> object has already been called.
        /// </exception>
        public static IEnumerable<XmlReader> Chunk(this IDecorator<XmlReader> decorator, int size = 128, Action<XmlWriterSettings> setup = null)
        {
            Validator.ThrowIfNull(decorator);
            Validator.ThrowIfTrue(decorator.Inner.ReadState != ReadState.Initial, nameof(decorator), "The Read method of the XmlReader object has already been called.");
            var reader = decorator.Inner;
            var outerReaders = new List<XmlReader>();
            var readerSettings = reader.Settings;
            if (MoveToFirstElement(Decorator.Enclose(reader)))
            {
                var rootElement = new XmlQualifiedEntity(reader.Prefix, reader.LocalName, reader.NamespaceURI);
                var innerReaders = new List<XmlReader>();
                Stream result;
                while (reader.Read())
                {
                    if (reader.Depth > 1) { continue; }
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            var document = new XPathDocument(reader.ReadSubtree());
                            var navigator = document.CreateNavigator();
                            innerReaders.Add(navigator.ReadSubtree());
                            break;
                    }

                    if (innerReaders.Count != size) { continue; }

                    result = XmlStreamFactory.CreateStream(writer => ChunkCore(writer, innerReaders, rootElement), setup);
                    outerReaders.Add(XmlReader.Create(result, readerSettings));
                    innerReaders.Clear();
                }

                if (innerReaders.Count > 0)
                {
                    result = XmlStreamFactory.CreateStream(writer => ChunkCore(writer, innerReaders, rootElement), setup);
                    outerReaders.Add(XmlReader.Create(result, readerSettings));
                    innerReaders.Clear();
                }
            }
            return outerReaders;
        }

        private static void ChunkCore(XmlWriter writer, IEnumerable<XmlReader> readers, XmlQualifiedEntity rootElement)
        {
            Validator.ThrowIfNull(writer);
            Validator.ThrowIfNull(readers);
            writer.WriteStartElement(rootElement.Prefix, rootElement.LocalName, rootElement.Namespace);
            foreach (var reader in readers)
            {
                try
                {
                    writer.WriteNode(reader, true);
                }
                finally
                {
                    reader.Dispose();
                }
            }
            writer.WriteEndDocument();
        }

        /// <summary>
        /// Moves the enclosed <see cref="XmlReader"/> of the specified <paramref name="decorator"/> to the first element.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{XmlReader}"/> to extend.</param>
        /// <returns><c>true</c> if an element exists (the reader moves to the first element), otherwise, <c>false</c> (the reader has reached <see cref="XmlReader.EOF"/>).</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The <see cref="XmlReader.Read"/> method of the enclosed <see cref="XmlReader"/> of the specified <paramref name="decorator"/> object has already been called.
        /// </exception>
        public static bool MoveToFirstElement(this IDecorator<XmlReader> decorator)
        {
            Validator.ThrowIfNull(decorator);
            var reader = decorator.Inner;
            if (reader.ReadState != ReadState.Initial) { throw new ArgumentException("The Read method of the XmlReader object has already been called.", nameof(decorator)); }
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Converts the XML hierarchy of the enclosed <see cref="XmlReader"/> of the specified <paramref name="decorator"/> into an <see cref="IHierarchy{T}"/>.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{XmlReader}" /> to extend.</param>
        /// <returns>An <see cref="IHierarchy{DataPair}"/> implementation.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        public static IHierarchy<DataPair> ToHierarchy(this IDecorator<XmlReader> decorator)
        {
            Validator.ThrowIfNull(decorator);
            var reader = decorator.Inner;
            return BuildHierarchy(reader);
        }

        private static IHierarchy<DataPair> BuildHierarchy(XmlReader reader)
        {
            var hierarchy = new Hierarchy<DataPair>();
            var attributes = new List<DataPair>();
            var stack = new Stack<IHierarchy<DataPair>>();
            var depthIndexes = new Dictionary<int, Dictionary<int, int>>();
            var index = 0;
            var dimension = 0;

            while (reader.Read())
            {
                object typeStrongValue;
                IHierarchy<DataPair> node = null;
                switch (reader.NodeType)
                {
                    case XmlNodeType.Attribute:
                        typeStrongValue = ParserFactory.FromValueType().Parse(reader.Value);
                        attributes.Add(new DataPair(reader.Name, typeStrongValue, typeStrongValue.GetType()));

                        while (reader.MoveToNextAttribute())
                        {
                            goto case XmlNodeType.Attribute;
                        }

                        node = stack.Pop();

                        foreach (var attribute in attributes)
                        {
                            node.Add(attribute);
                        }

                        attributes.Clear();

                        reader.MoveToElement();
                        break;
                    case XmlNodeType.Element:

                        if (reader.Depth == 0)
                        {
                            var root = hierarchy.Add(new DataPair(reader.Name, null, typeof(string)));
                            stack.Push(root);
                        }
                        else
                        {
                            var next = hierarchy[Decorator.Enclose(depthIndexes).GetDepthIndex(reader.Depth, index, dimension)].Add(new DataPair(reader.Name, null, typeof(string)));
                            index = next.Index;
                            stack.Push(next);
                        }

                        if (reader.HasAttributes && reader.MoveToFirstAttribute()) { goto case XmlNodeType.Attribute; }
                        break;
                    case XmlNodeType.EndElement:
                        if (reader.Depth == 1) { dimension++; }
                        break;
                    case XmlNodeType.CDATA:
                    case XmlNodeType.Text:
                        typeStrongValue = ParserFactory.FromValueType().Parse(reader.Value);

                        node = stack.Pop();

                        node.Replace(new DataPair(node.Instance.Name, typeStrongValue, typeStrongValue.GetType()));
                        break;
                }
            }
            return hierarchy;
        }
    }
}