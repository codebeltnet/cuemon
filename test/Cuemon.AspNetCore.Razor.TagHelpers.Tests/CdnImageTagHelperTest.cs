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
public class CdnImageTagHelperTest : Test
{
    public CdnImageTagHelperTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task Page_RenderImageTagForCdnRole()
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
            var result = await client.GetAsync("/CdnImageTagHelper");
            var body = await result.Content.ReadAsStringAsync();

            TestOutput.WriteLine(body);

            Assert.Equal(@"<img src=""https://nblcdn.net/devops.svg"" alt=""DevOps Factory"">", body, ignoreLineEndingDifferences: true);
        }
    }

    [Fact]
    public async Task Page_RenderImageTagForCdnRole_WithCacheBusting()
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
            var result = await client.GetAsync("/CdnImageTagHelper");
            var body = await result.Content.ReadAsStringAsync();

            TestOutput.WriteLine(body);

            Assert.Equal(@"<img src=""https://nblcdn.net/devops.svg?v=00000000000000000000000000000000"" alt=""DevOps Factory"">", body, ignoreLineEndingDifferences: true);
        }
    }
}
