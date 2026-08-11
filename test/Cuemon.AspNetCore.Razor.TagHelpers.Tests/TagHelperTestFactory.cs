using System;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Razor.TagHelpers.Assets;
using Cuemon.Extensions.AspNetCore.Configuration;
using Codebelt.Extensions.Xunit.Hosting.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace Cuemon.AspNetCore.Razor.TagHelpers;
internal static class TagHelperTestFactory
{
    internal static async Task<string> GetBodyAsync(string requestUri, Action<AppTagHelperOptions> appSetup = null, Action<CdnTagHelperOptions> cdnSetup = null, bool useCacheBusting = false, Uri baseAddress = null, string pathBase = null)
    {
        using var filter = WebHostTestFactory.Create(services =>
        {
            if (useCacheBusting) { services.AddCacheBusting<FakeCacheBusting>(); }
            services.AddRazorPages();
            services.Configure<CdnTagHelperOptions>(o =>
            {
                o.Scheme = ProtocolUriScheme.Https;
                o.BaseUrl = "nblcdn.net";
                cdnSetup?.Invoke(o);
            });
            services.Configure<AppTagHelperOptions>(o =>
            {
                o.Scheme = ProtocolUriScheme.Relative;
                o.BaseUrl = "static.cuemon.net";
                appSetup?.Invoke(o);
            });
        }, app =>
        {
            if (!string.IsNullOrWhiteSpace(pathBase)) { app.UsePathBase(pathBase); }
            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapRazorPages(); });
        });

        var client = filter.Host.GetTestClient();
        if (baseAddress != null) { client.BaseAddress = baseAddress; }

        var result = await client.GetAsync(requestUri);
        return (await result.Content.ReadAsStringAsync()).Trim();
    }
}
