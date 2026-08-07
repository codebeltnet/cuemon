using Cuemon.AspNetCore.Configuration;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Cuemon.AspNetCore.Razor.TagHelpers;
/// <summary>
/// Provides an implementation targeting &lt;img&gt; elements that supports <see cref="ICacheBusting"/> versioning of a static image placed on a location with a CDN role. This class cannot be inherited.
/// </summary>
/// <seealso cref="ImageTagHelper{TOptions}" />
[HtmlTargetElement("cdn-img")]
public sealed class CdnImageTagHelper : ImageTagHelper<CdnTagHelperOptions>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CdnImageTagHelper"/> class.
    /// </summary>
    /// <param name="setup">The <see cref="CdnTagHelperOptions" /> which need to be configured.</param>
    /// <param name="cacheBusting">An optional object implementing the <see cref="ICacheBusting" /> interface.</param>
    public CdnImageTagHelper(IOptions<CdnTagHelperOptions> setup, ICacheBusting cacheBusting = null) : base(setup, cacheBusting)
    {
    }
}
