namespace Cuemon.AspNetCore.Razor.TagHelpers;
/// <summary>
/// Specifies how the base URL of a static resource is resolved.
/// </summary>
public enum TagHelperBaseUrlMode
{
    /// <summary>
    /// Specifies that the base URL is resolved exclusively from the configured values of <see cref="TagHelperOptions.BaseUrl"/> and <see cref="TagHelperOptions.Scheme"/>.
    /// </summary>
    Configured,
    /// <summary>
    /// Specifies that the base URL is resolved from the configured value of <see cref="TagHelperOptions.BaseUrl"/> when available; otherwise from the current HTTP request.
    /// </summary>
    Automatic
}
