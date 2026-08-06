using System.Threading.Tasks;
using Cuemon.AspNetCore.Razor.TagHelpers.Assets;
using Cuemon.Extensions.AspNetCore.Configuration;
using Codebelt.Extensions.Xunit;
using Codebelt.Extensions.Xunit.Hosting.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Cuemon.AspNetCore.Razor.TagHelpers;
public class CdnScriptTagHelperTest : Test
{
    public CdnScriptTagHelperTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task Page_RenderScriptTagForCdnRole()
    {
        using (var filter = WebHostTestFactory.Create(services =>
        {
            services.AddRazorPages();
            services.Configure<CdnTagHelperOptions>(o =>
            {
                o.Scheme = ProtocolUriScheme.Https;
                o.BaseUrl = "nblcdn.net";
            });
            services.Configure<AppTagHelperOptions>(o =>
            {
                o.Scheme = ProtocolUriScheme.Relative;
                o.BaseUrl = "static.cuemon.net";
            });
        }, app =>
               {
                   app.UseRouting();
                   app.UseEndpoints(endpoints => { endpoints.MapRazorPages(); });
               }))
        {
            var client = filter.Host.GetTestClient();
            var result = await client.GetAsync("/CdnScriptTagHelper");
            var body = await result.Content.ReadAsStringAsync();

            TestOutput.WriteLine(body);

            Assert.Equal(@"<script type=""text/javascript"" src=""https://nblcdn.net/packages/fontawesome/5.15.3/js/all.js"" defer=""defer""></script>", body, ignoreLineEndingDifferences: true);
        }
    }

    [Fact]
    public async Task Page_RenderScriptTagForCdnRole_WithCacheBusting()
    {
        using (var filter = WebHostTestFactory.Create(services =>
        {
            services.AddCacheBusting<FakeCacheBusting>();
            services.AddRazorPages();
            services.Configure<CdnTagHelperOptions>(o =>
            {
                o.Scheme = ProtocolUriScheme.Https;
                o.BaseUrl = "nblcdn.net";
            });
            services.Configure<AppTagHelperOptions>(o =>
            {
                o.Scheme = ProtocolUriScheme.Relative;
                o.BaseUrl = "static.cuemon.net";
            });
        }, app =>
               {
                   app.UseRouting();
                   app.UseEndpoints(endpoints => { endpoints.MapRazorPages(); });
               }))
        {
            var client = filter.Host.GetTestClient();
            var result = await client.GetAsync("/CdnScriptTagHelper");
            var body = await result.Content.ReadAsStringAsync();

            TestOutput.WriteLine(body);

            Assert.Equal(@"<script type=""text/javascript"" src=""https://nblcdn.net/packages/fontawesome/5.15.3/js/all.js?v=00000000000000000000000000000000"" defer=""defer""></script>", body, ignoreLineEndingDifferences: true);
        }
    }
}
