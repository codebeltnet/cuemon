using Cuemon.AspNetCore.Configuration;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Cuemon.AspNetCore.Razor.TagHelpers;
/// <summary>
/// Provides an implementation targeting &lt;link&gt; elements that supports <see cref="ICacheBusting"/> versioning of a static resource placed on a location outside (but tied to) your application. This class cannot be inherited.
/// </summary>
/// <seealso cref="LinkTagHelper{TOptions}" />
[HtmlTargetElement("app-link")]
public sealed class AppLinkTagHelper : LinkTagHelper<AppTagHelperOptions>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AppLinkTagHelper"/> class.
    /// </summary>
    /// <param name="setup">The <see cref="AppTagHelperOptions" /> which need to be configured.</param>
    /// <param name="cacheBusting">An optional object implementing the <see cref="ICacheBusting" /> interface.</param>
    public AppLinkTagHelper(IOptions<AppTagHelperOptions> setup, ICacheBusting cacheBusting = null) : base(setup, cacheBusting)
    {
    }
}
