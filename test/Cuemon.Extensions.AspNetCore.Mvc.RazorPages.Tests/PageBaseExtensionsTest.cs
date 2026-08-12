using System.Threading.Tasks;
using Cuemon.AspNetCore.Razor.TagHelpers;
using Cuemon.Extensions.AspNetCore.Configuration;
using Cuemon.Extensions.AspNetCore.Mvc.RazorPages.Assets;
using Codebelt.Extensions.Xunit;
using Codebelt.Extensions.Xunit.Hosting.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Cuemon.Extensions.AspNetCore.Mvc.RazorPages;
public class PageBaseExtensionsTest : Test
{
    public PageBaseExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task Page_RenderUrlForAppRole()
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
            var result = await client.GetAsync("/AppUrl");
            var body = await result.Content.ReadAsStringAsync();

            TestOutput.WriteLine(body);

            Assert.Equal(@"<div class=""hero-bg"" style=""background-image: url('//static.cuemon.net/img/hero-solution.jpg');""></div>", body, ignoreLineEndingDifferences: true);
        }
    }

    [Fact]
    public async Task Page_RenderUrlForAppRole_WithCacheBusting()
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
            var result = await client.GetAsync("/AppUrl");
            var body = await result.Content.ReadAsStringAsync();

            TestOutput.WriteLine(body);

            Assert.Equal(@"<div class=""hero-bg"" style=""background-image: url('//static.cuemon.net/img/hero-solution.jpg?v=00000000000000000000000000000000');""></div>", body, ignoreLineEndingDifferences: true);
        }
    }

    [Fact]
    public async Task Page_RenderUrlForCdnRole()
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
            var result = await client.GetAsync("/CdnUrl");
            var body = await result.Content.ReadAsStringAsync();

            TestOutput.WriteLine(body);

            Assert.Equal(@"<div class=""factory-bg"" style=""background-image: url('https://nblcdn.net/img/devops-factory.png');""></div>", body, ignoreLineEndingDifferences: true);
        }
    }

    [Fact]
    public async Task Page_RenderUrlForCdnRole_WithCacheBusting()
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
            var result = await client.GetAsync("/CdnUrl");
            var body = await result.Content.ReadAsStringAsync();

            TestOutput.WriteLine(body);

            Assert.Equal(@"<div class=""factory-bg"" style=""background-image: url('https://nblcdn.net/img/devops-factory.png?v=00000000000000000000000000000000');""></div>", body, ignoreLineEndingDifferences: true);
        }
    }
}
