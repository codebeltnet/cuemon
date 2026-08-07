using System.Globalization;
using Cuemon.Configuration;
using Cuemon.Text;

namespace Cuemon.AspNetCore.Razor.TagHelpers;
/// <summary>
/// Configuration options for <see cref="CacheBustingTagHelper{TOptions}"/>.
/// </summary>
public abstract class TagHelperOptions : IParameterObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CdnTagHelperOptions"/> class.
    /// </summary>
    /// <remarks>
    /// The following table shows the initial property values for an instance of <see cref="CdnTagHelperOptions"/>.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Property</term>
    ///         <description>Initial Value</description>
    ///     </listheader>
    ///     <item>
    ///         <term><see cref="Scheme"/></term>
    ///         <description><see cref="ProtocolUriScheme.Relative"/></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="BaseUrl"/></term>
    ///         <description><c>null</c></description>
    ///     </item>
    /// </list>
    /// </remarks>
    protected TagHelperOptions()
    {
        Scheme = ProtocolUriScheme.Relative;
    }

    /// <summary>
    /// Gets or sets the <see cref="ProtocolUriScheme"/> of these options.
    /// </summary>
    /// <value>The <see cref="ProtocolUriScheme"/> of these options.</value>
    public ProtocolUriScheme Scheme { get; set; }

    /// <summary>
    /// Gets or sets the base URL of these options.
    /// </summary>
    /// <value>The base URL of these options.</value>
    public string BaseUrl { get; set; }

    /// <summary>
    /// Gets the base URL of this instance, formatted according to the defined <see cref="Scheme"/>.
    /// </summary>
    /// <returns>The base URL of this instance, formatted according to the defined <see cref="Scheme"/>.</returns>
    public string GetFormattedBaseUrl()
    {
        var baseUrlIsNullOrEmpty = string.IsNullOrWhiteSpace(BaseUrl);
        var baseUrlWithForwardingSlash = new Stem(BaseUrl).AttachSuffix("/");
        switch (Scheme)
        {
            case ProtocolUriScheme.None:
                return baseUrlIsNullOrEmpty ? "" : string.Create(CultureInfo.InvariantCulture, $"{baseUrlWithForwardingSlash}");
            case ProtocolUriScheme.Http:
                return baseUrlIsNullOrEmpty ? "" : string.Create(CultureInfo.InvariantCulture, $"{nameof(UriScheme.Http).ToLowerInvariant()}://{baseUrlWithForwardingSlash}");
            case ProtocolUriScheme.Https:
                return baseUrlIsNullOrEmpty ? "" : string.Create(CultureInfo.InvariantCulture, $"{nameof(UriScheme.Https).ToLowerInvariant()}://{baseUrlWithForwardingSlash}");
            case ProtocolUriScheme.Relative:
                return baseUrlIsNullOrEmpty ? "" : string.Create(CultureInfo.InvariantCulture, $"//{baseUrlWithForwardingSlash}");
            default:
                return "";
        }
    }
}
