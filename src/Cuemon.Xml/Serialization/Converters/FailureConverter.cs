using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Cuemon.Diagnostics;

namespace Cuemon.Xml.Serialization.Converters
{
    /// <summary>
    /// Converts a <see cref="Failure"/> object to XML.
    /// </summary>
    public class FailureConverter : XmlConverter<Failure>
    {
        /// <summary>
        /// Reads the XML representation of the <paramref name="objectType" />.
        /// </summary>
        /// <param name="objectType">The <seealso cref="Type" /> of the object.</param>
        /// <param name="reader">The <see cref="XmlReader" /> to read from.</param>
        /// <returns>An object of <see cref="Failure"/>.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public override Failure ReadXml(Type objectType, XmlReader reader)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes the XML representation of <see cref="Failure"/>.
        /// </summary>
        /// <param name="writer">The <see cref="XmlWriter" /> to write to.</param>
        /// <param name="value">The object to serialize.</param>
        /// <param name="elementName">The element name to encapsulate around <paramref name="value" />.</param>
        public override void WriteXml(XmlWriter writer, Failure value, XmlQualifiedEntity elementName = null)
        {
            writer.WriteStartElement(Decorator.Enclose(value.Type).SanitizeXmlElementName());
            if (value.Namespace != null) { writer.WriteAttributeString("namespace", value.Namespace); }

            WriteException(writer, value);

            writer.WriteEndElement();
        }

        private static void WriteException(XmlWriter writer, Failure value)
        {
            if (!string.IsNullOrEmpty(value.Source)) { writer.WriteElementString(nameof(value.Source), value.Source); }
            if (!string.IsNullOrEmpty(value.Message)) { writer.WriteElementString(nameof(value.Message), value.Message); }

            if (value.Stack.Any())
            {
                writer.WriteStartElement(nameof(value.Stack));
                foreach (var line in value.Stack)
                {
                    writer.WriteElementString("Frame", line);
                }
                writer.WriteEndElement();
            }

            if (value.Data.Count > 0)
            {
                writer.WriteStartElement(nameof(value.Data));
                foreach (var kvp in value.Data)
                {
                    writer.WriteStartElement(Decorator.Enclose(kvp.Key).SanitizeXmlElementName());
                    writer.WriteString(Decorator.Enclose(kvp.Value?.ToString()).SanitizeXmlElementText());
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }

            foreach (var kvp in value)
            {
                Decorator.Enclose(writer).WriteObject(kvp.Value, kvp.Value.GetType(), o => o.Settings.RootName = new XmlQualifiedEntity(kvp.Key));
            }

            WriteInnerExceptions(writer, value);
        }

        private static void WriteInnerExceptions(XmlWriter writer, Failure value)
        {
            var exception = value.GetUnderlyingException();
            var innerExceptions = new List<Exception>();
            if (exception is AggregateException aggregated)
            {
                innerExceptions.AddRange(aggregated.Flatten().InnerExceptions);
            }
            else
            {
                if (exception.InnerException != null) { innerExceptions.Add(exception.InnerException); }
            }
            if (innerExceptions.Count > 0)
            {
                var endElementsToWrite = 0;
                foreach (var inner in innerExceptions)
                {
                    var innerValue = new Failure(inner, value.GetUnderlyingSensitivity());
                    writer.WriteStartElement(Decorator.Enclose(innerValue.Type).SanitizeXmlElementName());
                    if (innerValue.Namespace != null) { writer.WriteAttributeString("namespace", innerValue.Namespace); }
                    WriteException(writer, innerValue);
                    endElementsToWrite++;
                }

                for (var i = 0; i < endElementsToWrite; i++) { writer.WriteEndElement(); }
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <seealso cref="XmlConverter" /> can read XML.
        /// </summary>
        /// <value><c>true</c> if this <seealso cref="XmlConverter" /> can read XML; otherwise, <c>false</c>.</value>
        public override bool CanRead => false;
    }
}
