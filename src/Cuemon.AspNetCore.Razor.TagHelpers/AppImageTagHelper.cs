using Cuemon.AspNetCore.Configuration;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Cuemon.AspNetCore.Razor.TagHelpers;
/// <summary>
/// Provides an implementation targeting &lt;img&gt; elements that supports <see cref="ICacheBusting"/> versioning of a static image placed on a location outside (but tied to) your application. This class cannot be inherited.
/// </summary>
/// <seealso cref="ImageTagHelper{TOptions}" />
[HtmlTargetElement("app-img")]
public sealed class AppImageTagHelper : ImageTagHelper<AppTagHelperOptions>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AppImageTagHelper"/> class.
    /// </summary>
    /// <param name="setup">The <see cref="AppTagHelperOptions" /> which need to be configured.</param>
    /// <param name="cacheBusting">An optional object implementing the <see cref="ICacheBusting" /> interface.</param>
    public AppImageTagHelper(IOptions<AppTagHelperOptions> setup, ICacheBusting cacheBusting = null) : base(setup, cacheBusting)
    {
    }
}
