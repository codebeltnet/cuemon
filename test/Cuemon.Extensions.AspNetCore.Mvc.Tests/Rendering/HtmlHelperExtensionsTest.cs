using System.Net;
using System.Threading.Tasks;
using Cuemon.Extensions.AspNetCore.Configuration;
using Codebelt.Extensions.Xunit;
using Codebelt.Extensions.Xunit.Hosting.AspNetCore;
using Codebelt.Extensions.Xunit.Hosting.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Cuemon.Extensions.AspNetCore.Mvc.Rendering;
public class HtmlHelperExtensionsTest : Test
{
    public HtmlHelperExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task EnsureThatBothRazorPagesAndControllerViewAreWorking()
    {
        using (var filter = WebHostTestFactory.Create(services =>
        {
            services.AddAssemblyCacheBusting();
            services.AddControllersWithViews();
            services.AddRazorPages(o => o.Conventions.AddPageRoute("/", "/"));
        }, app =>
               {
                   app.UseStaticFiles();
                   app.UseRouting();
                   app.UseEndpoints(endpoints =>
                   {
                       endpoints.MapRazorPages();
                       endpoints.MapControllerRoute(
                           name: "default",
                           pattern: "{controller=Home}/{action=Index}");
                   });
               }))
        {
            var client = filter.Host.GetTestClient();
            var page = await client.GetAsync("/regions");
            var view = await client.GetAsync("/home");

            Assert.Equal(HttpStatusCode.OK, page.StatusCode);
            Assert.Equal(HttpStatusCode.OK, view.StatusCode);
        }
    }

    [Fact]
    public async Task UseWhen_ShouldRenderClassWithActiveKeywordOnRegionsAnchorTag_AsControllerIsRegionAndActionIsRegion()
    {
        using (var filter = WebHostTestFactory.Create(services =>
        {
            services.AddAssemblyCacheBusting();
            services.AddControllersWithViews();
        }, app =>
               {
                   app.UseStaticFiles();
                   app.UseRouting();
                   app.UseEndpoints(endpoints =>
                   {
                       endpoints.MapControllerRoute(
                           name: "default",
                           pattern: "{controller=Home}/{action=Index}");
                   });
               }))
        {
            var client = filter.Host.GetTestClient();
            var view = await client.GetAsync("/regions/da-dk/denmark");
            var sut = await view.Content.ReadAsStringAsync();

            TestOutput.WriteLine(sut);

            Assert.Equal(HttpStatusCode.OK, view.StatusCode);
            Assert.Contains("<li class=\"active\"><a href=\"/regions\">Regions</a></li>", sut);
            Assert.Contains("<li><a href=\"/\">Home</a></li>", sut);
        }
    }

    [Fact]
    public async Task UseWhen_ShouldRenderClassWithActiveKeywordOnHomeAnchorTag_AsControllerIsHomeAndActionIsIndex()
    {
        using (var filter = WebHostTestFactory.Create(services =>
        {
            services.AddAssemblyCacheBusting();
            services.AddControllersWithViews();
        }, app =>
               {
                   app.UseStaticFiles();
                   app.UseRouting();
                   app.UseEndpoints(endpoints =>
                   {
                       endpoints.MapControllerRoute(
                           name: "default",
                           pattern: "{controller=Home}/{action=Index}");
                   });
               }))
        {
            var client = filter.Host.GetTestClient();
            var view = await client.GetAsync("/");
            var sut = await view.Content.ReadAsStringAsync();

            TestOutput.WriteLine(sut);

            Assert.Equal(HttpStatusCode.OK, view.StatusCode);
            Assert.Contains("<li><a href=\"/regions\">Regions</a></li>", sut);
            Assert.Contains("<li class=\"active\"><a href=\"/\">Home</a></li>", sut);
        }
    }

    [Fact]
    public async Task UseWhen_ShouldRenderClassWithActiveKeywordOnRegionsAnchorTag_AsPerPageStructure()
    {
        using (var filter = WebHostTestFactory.Create(services =>
        {
            services.AddAssemblyCacheBusting();
            services.AddRazorPages(o =>
            {
                o.Conventions.AddPageRoute("/Regions/Index", "regions");
                o.Conventions.AddPageRoute("/Regions/CultureCollection", "regions/{regionName}/{regionDisplayName}");
            });
        }, app =>
               {
                   app.UseStaticFiles();
                   app.UseRouting();
                   app.UseEndpoints(endpoints =>
                   {
                       endpoints.MapRazorPages();
                   });
               }))
        {
            var client = filter.Host.GetTestClient();
            var page = await client.GetAsync("/regions/da-dk/denmark");
            var sut = await page.Content.ReadAsStringAsync();

            TestOutput.WriteLine(sut);

            Assert.Equal(HttpStatusCode.OK, page.StatusCode);
            Assert.Contains("<li class=\"active\"><a>Regions</a></li>", sut);
            Assert.Contains("<li><a>Home</a></li>", sut);
        }
    }

    [Fact]
    public async Task UseWhen_ShouldRenderClassWithActiveKeywordOnHomeAnchorTag_AsPerPageStructure()
    {
        using (var filter = WebHostTestFactory.Create(services =>
        {
            services.AddAssemblyCacheBusting();
            services.AddRazorPages(o =>
            {
                o.Conventions.AddPageRoute("/", "/");
                o.Conventions.AddPageRoute("/Regions/Index", "regions");
                o.Conventions.AddPageRoute("/Regions/CultureCollection", "/regions/{regionName}/{regionDisplayName}");
            });
        }, app =>
               {
                   app.UseStaticFiles();
                   app.UseRouting();
                   app.UseEndpoints(endpoints =>
                   {
                       endpoints.MapRazorPages();
                   });
               }))
        {
            var client = filter.Host.GetTestClient();
            var page = await client.GetAsync("/");
            var sut = await page.Content.ReadAsStringAsync();

            TestOutput.WriteLine(sut);

            Assert.Equal(HttpStatusCode.OK, page.StatusCode);
            Assert.Contains("<li><a>Regions</a></li>", sut);
            Assert.Contains("<li class=\"active\"><a>Home</a></li>", sut);
        }
    }
}
