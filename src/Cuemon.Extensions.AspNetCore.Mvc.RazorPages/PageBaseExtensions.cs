using System.Globalization;
using Cuemon.AspNetCore.Configuration;
using Cuemon.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Cuemon.Extensions.AspNetCore.Mvc.RazorPages;
/// <summary>
/// Extension methods for the <see cref="PageBase"/> class.
/// </summary>
public static class PageBaseExtensions
{
    /// <summary>
    /// Gets the fully qualified URL for a static resource of your application.
    /// </summary>
    /// <param name="pageModel">The <see cref="PageBase"/> to extend.</param>
    /// <param name="src">The relative source of the static resource.</param>
    /// <returns>A concatenated string of the formatted base URL from <see cref="AppTagHelperOptions"/> and the specified <paramref name="src"/>.</returns>
    public static string GetAppUrl(this PageBase pageModel, string src)
    {
        var options = pageModel.HttpContext.RequestServices.GetRequiredService<IOptions<AppTagHelperOptions>>();
        var cb = pageModel.HttpContext.RequestServices.GetService<ICacheBusting>();
        return string.Concat(options.Value.GetFormattedBaseUrl(), cb != null ? string.Create(CultureInfo.InvariantCulture, $"{src}?v={cb.Version}") : src);
    }

    /// <summary>
    /// Gets the fully qualified URL for a static resource placed outside your application (typical CDN).
    /// </summary>
    /// <param name="pageModel">The <see cref="PageBase"/> to extend.</param>
    /// <param name="src">The relative source of the static resource.</param>
    /// <returns>A concatenated string of the formatted base URL from <see cref="CdnTagHelperOptions"/> and the specified <paramref name="src"/>.</returns>
    public static string GetCdnUrl(this PageBase pageModel, string src)
    {
        var options = pageModel.HttpContext.RequestServices.GetRequiredService<IOptions<CdnTagHelperOptions>>();
        var cb = pageModel.HttpContext.RequestServices.GetService<ICacheBusting>();
        return string.Concat(options.Value.GetFormattedBaseUrl(), cb != null ? string.Create(CultureInfo.InvariantCulture, $"{src}?v={cb.Version}") : src);
    }
}
