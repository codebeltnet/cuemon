using Cuemon.AspNetCore.Configuration;
using Cuemon.Configuration;
using Microsoft.AspNetCore.Razor.TagHelpers;
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
    /// Gets the configured options of this instance.
    /// </summary>
    /// <value>The configured options of this instance.</value>
    public TOptions Options { get; }
}
