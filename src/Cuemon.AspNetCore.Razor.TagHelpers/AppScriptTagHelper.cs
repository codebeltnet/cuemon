using Cuemon.AspNetCore.Configuration;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Cuemon.AspNetCore.Razor.TagHelpers;
/// <summary>
/// Provides an implementation targeting &lt;script&gt; elements that supports <see cref="ICacheBusting"/> versioning of a static script placed on a location outside (but tied to) your application. This class cannot be inherited.
/// </summary>
/// <seealso cref="ScriptTagHelper{TOptions}" />
[HtmlTargetElement("app-script")]
public sealed class AppScriptTagHelper : ScriptTagHelper<AppTagHelperOptions>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AppScriptTagHelper"/> class.
    /// </summary>
    /// <param name="setup">The <see cref="CdnTagHelperOptions" /> which need to be configured.</param>
    /// <param name="cacheBusting">An optional object implementing the <see cref="ICacheBusting" /> interface.</param>
    public AppScriptTagHelper(IOptions<AppTagHelperOptions> setup, ICacheBusting cacheBusting = null) : base(setup, cacheBusting)
    {
    }
}
