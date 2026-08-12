using System.Threading.Tasks;
using Cuemon.AspNetCore.Configuration;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Cuemon.AspNetCore.Razor.TagHelpers;
/// <summary>
/// Provides a base-class for targeting &lt;script&gt; elements that supports <see cref="ICacheBusting"/> versioning.
/// </summary>
/// <seealso cref="CacheBustingTagHelper{TOptions}" />
public abstract class ScriptTagHelper<TOptions> : CacheBustingTagHelper<TOptions> where TOptions : TagHelperOptions, new()
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ScriptTagHelper{TOptions}"/> class.
    /// </summary>
    /// <param name="setup">The <typeparamref name="TOptions"/> which need to be configured.</param>
    /// <param name="cacheBusting">An optional object implementing the <see cref="ICacheBusting" /> interface.</param>
    protected ScriptTagHelper(IOptions<TOptions> setup, ICacheBusting cacheBusting = null) : base(setup, cacheBusting)
    {
    }

    /// <summary>
    /// Gets or sets the source of the script.
    /// </summary>
    /// <value>The source of the script.</value>
    public string Src { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the script is executed when the page has finished parsing.
    /// </summary>
    /// <value><c>true</c> if the script is executed when the page has finished parsing; otherwise, <c>false</c>.</value>
    public bool Defer { get; set; }

    /// <summary>
    /// Asynchronously executes the <see cref="TagHelper" /> with the given <paramref name="context" /> and <paramref name="output" />.
    /// </summary>
    /// <param name="context">Contains information associated with the current HTML tag.</param>
    /// <param name="output">A stateful HTML element used to generate an HTML tag.</param>
    /// <returns>A <see cref="Task" /> that on completion updates the <paramref name="output" />.</returns>
    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagMode = TagMode.StartTagAndEndTag;
        output.TagName = "script";
        output.Attributes.Add("type", "text/javascript");
        output.Attributes.Add("src", ResolveUrl(Src));
        if (Defer) { output.Attributes.Add("defer", "defer"); }
        return Task.CompletedTask;
    }
}
