using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Cuemon.Configuration;
using Cuemon.Diagnostics;
using Cuemon.Net.Http;
using Cuemon.Xml.Serialization.Converters;

namespace Cuemon.Xml.Serialization.Formatters
{
    /// <summary>
    /// Configuration options for <see cref="XmlFormatter"/>.
    /// </summary>
    public class XmlFormatterOptions : IExceptionDescriptorOptions, IContentNegotiation, IValidatableParameterObject
    {
#if NET9_0_OR_GREATER
        private readonly System.Threading.Lock _lock = new();
#else
        private readonly object _lock = new();
#endif
        private bool _refreshed;

        /// <summary>
        /// Provides the default/fallback media type that the associated formatter should use when content negotiation either fails or is absent.
        /// </summary>
        /// <value>The media type that the associated formatter should use when content negotiation either fails or is absent.</value>
        public static MediaTypeHeaderValue DefaultMediaType { get; } = new("application/xml");

        static XmlFormatterOptions()
        {
            DefaultConverters = list =>
            {
                Decorator.Enclose(list)
                    .AddFailureConverter()
                    .AddEnumerableConverter()
                    .AddUriConverter()
                    .AddDateTimeConverter()
                    .AddTimeSpanConverter()
                    .AddStringConverter();
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlFormatterOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="XmlFormatterOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="Settings"/></term>
        ///         <description><see cref="XmlSerializerOptions"/></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="SynchronizeWithXmlConvert"/></term>
        ///         <description><c>true</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="SensitivityDetails"/></term>
        ///         <description><see cref="FaultSensitivityDetails.None"/></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="SupportedMediaTypes"/></term>
        ///         <description>
        ///             <code>
        ///new List&lt;MediaTypeHeaderValue&gt;()
        ///{
        ///    new("application/xml"),
        ///    new("text/xml")
        ///};
        ///             </code>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        public XmlFormatterOptions()
        {
            Settings = new XmlSerializerOptions();
            DefaultConverters?.Invoke(Settings.Converters);
            SensitivityDetails = FaultSensitivityDetails.None;
            SupportedMediaTypes = new List<MediaTypeHeaderValue>()
            {
                DefaultMediaType,
                new("text/xml"),
                new("application/problem+xml")
            };
        }

        /// <summary>
        /// Gets or sets a delegate that  is invoked when <see cref="XmlFormatterOptions"/> is initialized and propagates registered <see cref="XmlConverter"/> implementations.
        /// </summary>
        /// <value>The delegate which propagates registered <see cref="XmlConverter"/> implementations when <see cref="XmlFormatterOptions"/> is initialized.</value>
        public static Action<IList<XmlConverter>> DefaultConverters { get; set; }

        /// <summary>
        /// Gets or sets a bitwise combination of the enumeration values that specify which sensitive details to include in the serialized result.
        /// </summary>
        /// <value>The enumeration values that specify which sensitive details to include in the serialized result.</value>
        public FaultSensitivityDetails SensitivityDetails { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Settings"/> should be synchronized on <see cref="XmlConvert.DefaultSettings"/>.
        /// </summary>
        /// <value><c>true</c> if <see cref="Settings"/> should be synchronized on <see cref="XmlConvert.DefaultSettings"/>; otherwise, <c>false</c>.</value>
        public bool SynchronizeWithXmlConvert { get; set; }

        /// <summary>
        /// Gets or sets the settings to support the <see cref="XmlFormatter"/>.
        /// </summary>
        /// <returns>A <see cref="XmlSerializerOptions"/> instance that specifies a set of features to support the <see cref="XmlFormatter"/> object.</returns>
        public XmlSerializerOptions Settings { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="MediaTypeHeaderValue"/> elements supported by the <see cref="XmlFormatter"/>.
        /// </summary>
        /// <returns>A collection of <see cref="MediaTypeHeaderValue"/> elements supported by the <see cref="XmlFormatter"/>.</returns>
        public IReadOnlyCollection<MediaTypeHeaderValue> SupportedMediaTypes { get; set; }

        internal XmlSerializerOptions RefreshWithConverterDependencies()
        {
            lock (_lock)
            {
                if (!_refreshed)
                {
                    _refreshed = true;
                    if (Settings.FlattenCollectionItems)
                    {
                        var existing = Decorator.Enclose(Settings.Converters).FirstOrDefaultWriterConverter(typeof(IEnumerable));
                        if (existing != null) { Settings.Converters.Remove(existing); }
                        Decorator.Enclose(Settings.Converters).AddEnumerableConverter(flattenItems: true);
                    }
                    Decorator.Enclose(Settings.Converters)
                        .AddExceptionConverter(SensitivityDetails.HasFlag(FaultSensitivityDetails.StackTrace), SensitivityDetails.HasFlag(FaultSensitivityDetails.Data))
                        .AddExceptionDescriptorConverter(o => o.SensitivityDetails = SensitivityDetails);
                }
                return Settings;
            }
        }

        /// <summary>
        /// Determines whether the public read-write properties of this instance are in a valid state.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// <see cref="Settings"/> cannot be null.
        /// </exception>
        /// <remarks>This method is expected to throw exceptions when one or more conditions fails to be in a valid state.</remarks>
        public void ValidateOptions()
        {
            Validator.ThrowIfInvalidState(Settings == null);
            Validator.ThrowIfInvalidState(SupportedMediaTypes == null);
        }
    }
}
