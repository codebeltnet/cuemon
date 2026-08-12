using System.Threading.Tasks;
using Cuemon.AspNetCore.Configuration;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Cuemon.AspNetCore.Razor.TagHelpers;
/// <summary>
/// Provides a base-class for targeting &lt;link&gt; elements that supports <see cref="ICacheBusting"/> versioning.
/// </summary>
/// <seealso cref="CacheBustingTagHelper{TOptions}" />
public abstract class LinkTagHelper<TOptions> : CacheBustingTagHelper<TOptions> where TOptions : TagHelperOptions, new()
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LinkTagHelper{TOptions}"/> class.
    /// </summary>
    /// <param name="setup">The <typeparamref name="TOptions"/> which need to be configured.</param>
    /// <param name="cacheBusting">An optional object implementing the <see cref="ICacheBusting" /> interface.</param>
    protected LinkTagHelper(IOptions<TOptions> setup, ICacheBusting cacheBusting = null) : base(setup, cacheBusting)
    {
        Type = "text/css";
        Rel = "stylesheet";
    }

    /// <summary>
    /// Gets or sets the location of the link.
    /// </summary>
    /// <value>The location of the link.</value>
    public string Href { get; set; }

    /// <summary>
    /// Gets or sets the type of the link.
    /// </summary>
    /// <value>The type of the link.</value>
    public string Type { get; set; }

    /// <summary>
    /// Gets or sets the relation of the link.
    /// </summary>
    /// <value>The relation of the link.</value>
    public string Rel { get; set; }

    /// <summary>
    /// Asynchronously executes the <see cref="TagHelper" /> with the given <paramref name="context" /> and <paramref name="output" />.
    /// </summary>
    /// <param name="context">Contains information associated with the current HTML tag.</param>
    /// <param name="output">A stateful HTML element used to generate an HTML tag.</param>
    /// <returns>A <see cref="Task" /> that on completion updates the <paramref name="output" />.</returns>
    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagMode = TagMode.StartTagOnly;
        output.TagName = "link";
        output.Attributes.Add("rel", Rel);
        output.Attributes.Add("href", ResolveUrl(Href));
        if (!string.IsNullOrWhiteSpace(Type)) { output.Attributes.Add("type", new HtmlString(Type)); }
        return Task.CompletedTask;
    }
}
