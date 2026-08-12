using System.Globalization;
using Cuemon.Configuration;
using Cuemon.Text;
using Microsoft.AspNetCore.Http;

namespace Cuemon.AspNetCore.Razor.TagHelpers;
/// <summary>
/// Configuration options for <see cref="CacheBustingTagHelper{TOptions}"/>.
/// </summary>
public abstract class TagHelperOptions : IParameterObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TagHelperOptions"/> class.
    /// </summary>
    /// <remarks>
    /// The following table shows the initial property values for an instance of <see cref="TagHelperOptions"/>.
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
    ///         <term><see cref="BaseUrlMode"/></term>
    ///         <description><see cref="TagHelperBaseUrlMode.Configured"/></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="BaseUrl"/></term>
    ///         <description><c>null</c></description>
    ///     </item>
    /// </list>
    /// </remarks>
    protected TagHelperOptions()
    {
        BaseUrlMode = TagHelperBaseUrlMode.Configured;
        Scheme = ProtocolUriScheme.Relative;
    }

    /// <summary>
    /// Gets or sets the <see cref="ProtocolUriScheme"/> of these options.
    /// </summary>
    /// <value>The <see cref="ProtocolUriScheme"/> of these options.</value>
    public ProtocolUriScheme Scheme { get; set; }

    /// <summary>
    /// Gets or sets the base URL resolution mode of these options.
    /// </summary>
    /// <value>The base URL resolution mode of these options.</value>
    public TagHelperBaseUrlMode BaseUrlMode { get; set; }

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
        if (string.IsNullOrWhiteSpace(BaseUrl)) { return ""; }
        var baseUrlWithForwardingSlash = new Stem(BaseUrl).AttachSuffix("/");
        switch (Scheme)
        {
            case ProtocolUriScheme.None:
                return string.Create(CultureInfo.InvariantCulture, $"{baseUrlWithForwardingSlash}");
            case ProtocolUriScheme.Http:
                return string.Create(CultureInfo.InvariantCulture, $"{nameof(UriScheme.Http).ToLowerInvariant()}://{baseUrlWithForwardingSlash}");
            case ProtocolUriScheme.Https:
                return string.Create(CultureInfo.InvariantCulture, $"{nameof(UriScheme.Https).ToLowerInvariant()}://{baseUrlWithForwardingSlash}");
            case ProtocolUriScheme.Relative:
                return string.Create(CultureInfo.InvariantCulture, $"//{baseUrlWithForwardingSlash}");
            default:
                return "";
        }
    }

    /// <summary>
    /// Gets the base URL of this instance, resolved from the configured values of this instance or the specified <paramref name="request"/>.
    /// </summary>
    /// <param name="request">The current HTTP request that may provide the effective scheme, host and application base path when <see cref="BaseUrlMode"/> is <see cref="TagHelperBaseUrlMode.Automatic"/> and <see cref="BaseUrl"/> is not configured.</param>
    /// <returns>The base URL of this instance, resolved from the configured values of this instance or the specified <paramref name="request"/>.</returns>
    public string GetFormattedBaseUrl(HttpRequest request)
    {
        if (BaseUrlMode == TagHelperBaseUrlMode.Automatic && string.IsNullOrWhiteSpace(BaseUrl) && request != null)
        {
            return string.Create(CultureInfo.InvariantCulture, $"{request.Scheme}://{request.Host.ToUriComponent()}{request.PathBase.ToUriComponent()}/");
        }
        return GetFormattedBaseUrl();
    }
}
