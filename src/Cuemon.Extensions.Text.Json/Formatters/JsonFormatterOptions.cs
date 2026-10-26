using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using Cuemon.Configuration;
using Cuemon.Diagnostics;
using Cuemon.Extensions.Text.Json.Converters;
using Cuemon.Net.Http;

namespace Cuemon.Extensions.Text.Json.Formatters;
/// <summary>
/// Specifies options that is related to <see cref="JsonFormatter"/> operations.
/// </summary>
public class JsonFormatterOptions : IContentNegotiation, IExceptionDescriptorOptions, IValidatableParameterObject
{
#if NET10_0_OR_GREATER
    private readonly System.Threading.Lock _lock = new();
#else
    private readonly object _lock = new();
#endif
    private bool _refreshed;

    /// <summary>
    /// Provides the default/fallback media type that the associated formatter should use when content negotiation either fails or is absent.
    /// </summary>
    /// <value>The media type that the associated formatter should use when content negotiation either fails or is absent.</value>
    public static MediaTypeHeaderValue DefaultMediaType { get; } = new("application/json");

    static JsonFormatterOptions()
    {
        DefaultConverters = list =>
        {
            list.AddDataPairConverter();
            list.AddStringEnumConverter();
            list.AddStringFlagsEnumConverter();
            list.AddTransientFaultExceptionConverter();
            list.AddFailureConverter();
        };
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonFormatterOptions"/> class.
    /// </summary>
    /// <remarks>
    /// The following table shows the initial property values for an instance of <see cref="JsonFormatterOptions"/>.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Property</term>
    ///         <description>Initial Value</description>
    ///     </listheader>
    ///     <item>
    ///         <term><see cref="Settings"/></term>
    ///         <description><see cref="JsonSerializerOptions"/></description>
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
    ///    new("application/json"),
    ///    new("text/json")
    ///};
    ///             </code>
    ///         </description>
    ///     </item>
    /// </list>
    /// </remarks>
    public JsonFormatterOptions()
    {
        Settings = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            ReadCommentHandling = JsonCommentHandling.Skip,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
        DefaultConverters?.Invoke(Settings.Converters);
        SensitivityDetails = FaultSensitivityDetails.None;
        SupportedMediaTypes = new List<MediaTypeHeaderValue>()
        {
            DefaultMediaType,
            new("text/json"),
            new("application/problem+json")
        };
    }

    /// <summary>
    /// Gets or sets a delegate that  is invoked when <see cref="JsonFormatterOptions"/> is initialized and propagates registered <see cref="JsonConverter"/> implementations.
    /// </summary>
    /// <value>The delegate which propagates registered <see cref="JsonConverter"/> implementations when <see cref="JsonFormatterOptions"/> is initialized.</value>
    public static Action<IList<JsonConverter>> DefaultConverters { get; set; }

    /// <summary>
    /// Gets or sets a bitwise combination of the enumeration values that specify which sensitive details to include in the serialized result.
    /// </summary>
    /// <value>The enumeration values that specify which sensitive details to include in the serialized result.</value>
    public FaultSensitivityDetails SensitivityDetails { get; set; }

    /// <summary>
    /// Gets or sets the settings to support the <see cref="JsonFormatter"/>.
    /// </summary>
    /// <returns>A <see cref="JsonSerializerOptions"/> instance that specifies a set of features to support the <see cref="JsonFormatter"/> object.</returns>
    public JsonSerializerOptions Settings { get; set; }

    /// <summary>
    /// Gets or sets the collection of <see cref="MediaTypeHeaderValue"/> elements supported by the <see cref="JsonFormatter"/>.
    /// </summary>
    /// <returns>A collection of <see cref="MediaTypeHeaderValue"/> elements supported by the <see cref="JsonFormatter"/>.</returns>
    public IReadOnlyCollection<MediaTypeHeaderValue> SupportedMediaTypes { get; set; }

    internal JsonSerializerOptions RefreshWithConverterDependencies()
    {
        lock (_lock)
        {
            if (!_refreshed)
            {
                _refreshed = true;
                Settings.Converters.AddExceptionConverter(SensitivityDetails.HasFlag(FaultSensitivityDetails.StackTrace), SensitivityDetails.HasFlag(FaultSensitivityDetails.Data));
                Settings.Converters.AddExceptionDescriptorConverterOf<ExceptionDescriptor>(o => o.SensitivityDetails = SensitivityDetails);
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
