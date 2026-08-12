using System;
using System.Globalization;
using Cuemon.AspNetCore.Configuration;
using Cuemon.Configuration;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Options;

namespace Cuemon.AspNetCore.Razor.TagHelpers;
/// <summary>
/// Provides a base-class for static content related <see cref="TagHelper"/> implementation in Razor for ASP.NET Core.
/// </summary>
/// <seealso cref="TagHelper" />
/// <seealso cref="IConfigurable{TOptions}" />
public abstract class CacheBustingTagHelper<TOptions> : TagHelper, IConfigurable<TOptions> where TOptions : TagHelperOptions, new()
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CacheBustingTagHelper{TOptions}"/> class.
    /// </summary>
    /// <param name="setup">The <typeparamref name="TOptions"/> which need to be configured.</param>
    /// <param name="cacheBusting">An optional object implementing the <see cref="ICacheBusting"/> interface.</param>
    protected CacheBustingTagHelper(IOptions<TOptions> setup, ICacheBusting cacheBusting = null)
    {
        CacheBusting = cacheBusting;
        Options = setup.Value;
    }

    /// <summary>
    /// Gets the by constructor optional supplied object implementing the <see cref="ICacheBusting"/> interface.
    /// </summary>
    /// <value>The by constructor optional supplied object implementing the <see cref="ICacheBusting"/> interface.</value>
    protected ICacheBusting CacheBusting { get; }

    /// <summary>
    /// Gets a value indicating whether an object implementing the <see cref="ICacheBusting"/> interface is specified.
    /// </summary>
    /// <value><c>true</c> if an object implementing the <see cref="ICacheBusting"/> interface is specified; otherwise, <c>false</c>.</value>
    protected bool UseCacheBusting => CacheBusting != null;

    /// <summary>
    /// Gets or sets the current view context.
    /// </summary>
    /// <value>The current view context.</value>
    [HtmlAttributeNotBound]
    [ViewContext]
    public ViewContext ViewContext { get; set; }

    /// <summary>
    /// Gets the configured options of this instance.
    /// </summary>
    /// <value>The configured options of this instance.</value>
    public TOptions Options { get; }

    /// <summary>
    /// Resolves the fully qualified URL of the specified static resource.
    /// </summary>
    /// <param name="path">The relative path of the static resource.</param>
    /// <returns>The fully qualified URL of the specified static resource.</returns>
    protected string ResolveUrl(string path)
    {
        var baseUrl = Options.GetFormattedBaseUrl(ViewContext?.HttpContext?.Request);
        var resolvedPath = NormalizePath(path, !string.IsNullOrWhiteSpace(baseUrl));
        return string.Concat(baseUrl, UseCacheBusting ? string.Create(CultureInfo.InvariantCulture, $"{resolvedPath}?v={CacheBusting.Version}") : resolvedPath);
    }

    private static string NormalizePath(string path, bool normalize)
    {
        if (!normalize || string.IsNullOrWhiteSpace(path)) { return path; }
        if (path.StartsWith("~/", StringComparison.Ordinal)) { path = path.Substring(2); }
        return path.TrimStart('/');
    }
}
