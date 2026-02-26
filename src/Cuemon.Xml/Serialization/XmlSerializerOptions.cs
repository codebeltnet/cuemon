using System.Collections.Generic;
using System.Xml;
using Cuemon.Xml.Serialization.Converters;

namespace Cuemon.Xml.Serialization
{
    /// <summary>
    /// Configuration options for <see cref="XmlSerializer"/>.
    /// </summary>
    public class XmlSerializerOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XmlSerializerOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="DefaultXmlConverter"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="Reader"/></term>
        ///         <description><c>new XmlWriterSettings() { IndentChars = Alphanumeric.Tab };</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="Writer"/></term>
        ///         <description><c>new XmlReaderSettings { DtdProcessing = DtdProcessing.Ignore };</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="Converters"/></term>
        ///         <description><see cref="List{XmlConverter}"/></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="RootName"/></term>
        ///         <description><c>null</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public XmlSerializerOptions()
        {
            Writer = new XmlWriterSettings() { IndentChars = Alphanumeric.Tab };
            Reader = new XmlReaderSettings { DtdProcessing = DtdProcessing.Ignore };
            Converters = new List<XmlConverter>();
        }

        /// <summary>
        /// Gets a <see cref="XmlConverter" /> collection that will be used during serialization.
        /// </summary>
        /// <value>The converters that will be used during serialization.</value>
        public IList<XmlConverter> Converters { get; }

        /// <summary>
        /// Gets or sets the <see cref="XmlReaderSettings"/> to support the <see cref="XmlSerializer"/>.
        /// </summary>
        /// <value>A <see cref="XmlReaderSettings"/> instance that specifies a set of features to support the <see cref="XmlSerializer"/> object.</value>
        public XmlReaderSettings Reader { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="XmlWriterSettings"/> to support the <see cref="XmlSerializer"/>.
        /// </summary>
        /// <value>A <see cref="XmlWriterSettings"/> instance that specifies a set of features to support the <see cref="XmlSerializer"/> object.</value>
        public XmlWriterSettings Writer { get; set; }

        /// <summary>
        /// Gets or sets the name of the XML root element.
        /// </summary>
        /// <value>The name of the XML root element.</value>
        public XmlQualifiedEntity RootName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether collection items should be serialized as repeated elements using the property name instead of being wrapped in a generic <c>Item</c> element.
        /// </summary>
        /// <value><c>true</c> to emit one repeated element per item named after the enclosing property; <c>false</c> to use the default <c>Item</c> wrapper. The default is <c>false</c>.</value>
        public bool FlattenCollectionItems { get; set; }
    }
}
