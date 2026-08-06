using System.Threading.Tasks;
using Cuemon.AspNetCore.Mvc.Assets;
using Cuemon.Extensions.AspNetCore.Mvc.Filters;
using Cuemon.Extensions.AspNetCore.Mvc.Filters.Cacheable;
using Codebelt.Extensions.Xunit;
using Codebelt.Extensions.Xunit.Hosting.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Xunit;

namespace Cuemon.AspNetCore.Mvc.Filters.Cacheable;
public class HttpCacheableFilterTest : Test
{
    public HttpCacheableFilterTest(ITestOutputHelper output) : base(output)
    {
    }

    [Theory]
    [InlineData("/fake/getCacheByEtag")]
    [InlineData("/fake/it")]
    public async Task GetEtag_ShouldReturnOkWithEtagAndSubsequentlyNotModified(string relativeEndpoint)
    {
        using (var filter = WebHostTestFactory.Create(services =>
        {
            services.AddControllers(o => { o.Filters.AddHttpCacheable(); }).AddApplicationPart(typeof(FakeController).Assembly).AddHttpCacheableOptions(o =>
            {
                o.Filters.AddEntityTagHeader(io => io.UseEntityTagResponseParser = true);
            });
        }, app =>
               {
                   app.UseRouting();
                   app.UseEndpoints(routes => { routes.MapControllers(); });
               }))
        {
            var client = filter.Host.GetTestClient();
            var result = await client.GetAsync(relativeEndpoint);
            var etag = result.Headers.ETag.ToString();

            Assert.Equal(StatusCodes.Status200OK, (int)result.StatusCode);
            TestOutput.WriteLine(etag);

            client.DefaultRequestHeaders.Add(HeaderNames.IfNoneMatch, etag);

            result = await client.GetAsync(relativeEndpoint);
            Assert.Equal(StatusCodes.Status304NotModified, (int)result.StatusCode);
        }
    }

    [Fact]
    public async Task GetLastModified_ShouldReturnOkWithLastModifiedAndSubsequentlyNotModified()
    {
        using (var filter = WebHostTestFactory.Create(services =>
        {
            services.AddControllers(o => { o.Filters.AddHttpCacheable(); }).AddApplicationPart(typeof(FakeController).Assembly).AddHttpCacheableOptions(o =>
            {
                o.Filters.AddLastModifiedHeader();
            });
        }, app =>
               {
                   app.UseRouting();
                   app.UseEndpoints(routes => { routes.MapControllers(); });
               }))
        {
            var client = filter.Host.GetTestClient();
            var result = await client.GetAsync("/fake/getCacheByLastModified");
            var lastModified = result.Content.Headers.LastModified.Value;

            Assert.Equal(StatusCodes.Status200OK, (int)result.StatusCode);
            TestOutput.WriteLine(lastModified.ToString("O"));

            client.DefaultRequestHeaders.Add(HeaderNames.IfModifiedSince, lastModified.ToString("R"));

            result = await client.GetAsync("/fake/getCacheByLastModified");
            Assert.Equal(StatusCodes.Status304NotModified, (int)result.StatusCode);
        }
    }
}
