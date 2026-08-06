using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Http.Throttling;
using Cuemon.AspNetCore.Mvc.Assets;
using Cuemon.AspNetCore.Mvc.Filters.Diagnostics;
using Cuemon.Extensions;
using Cuemon.Extensions.AspNetCore.Http.Throttling;
using Cuemon.Extensions.AspNetCore.Mvc.Filters;
using Codebelt.Extensions.Xunit;
using Codebelt.Extensions.Xunit.Hosting.AspNetCore;
using Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Cuemon.AspNetCore.Mvc.Filters.Throttling;
public class ThrottlingSentinelFilterTest : Test
{
    public ThrottlingSentinelFilterTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task OnActionExecutionAsync_ShouldCaptureThrottlingException()
    {
        using (var filter = WebHostTestFactory.Create(services =>
               {
                   services
                       .AddControllers(o =>
                       {
                           o.Filters.Add<FaultDescriptorFilter>();
                           o.Filters.AddThrottlingSentinel();
                       }).AddApplicationPart(typeof(FakeController).Assembly)
                       .AddJsonFormatters()
                       .AddThrottlingSentinelOptions(o =>
                       {
                           o.ContextResolver = context => nameof(OnActionExecutionAsync_ShouldCaptureThrottlingException);
                           o.Quota = new ThrottleQuota(10, 5, TimeUnit.Seconds);
                       });
                   services.AddMemoryThrottlingCache();
               }, app =>
                      {
                          app.UseRouting();
                          app.UseEndpoints(routes => { routes.MapControllers(); });
                      }))
        {
            var client = filter.Host.GetTestClient();

            HttpResponseMessage result = null;
            for (var i = 0; i < 10; i++)
            {
                result = await client.GetAsync("/fake/it");
                Assert.Equal(StatusCodes.Status200OK, (int)result.StatusCode);
                Assert.Equal("\"Unit Test\"", await result.Content.ReadAsStringAsync());
            }

            result = await client.GetAsync("/fake/it");

            var retryAfter = result.Headers.RetryAfter.Delta.Value.TotalSeconds;
            var ratelimitReset = result.Headers.GetValues("RateLimit-Reset").Single().As<int>();
            var actual = await result.Content.ReadAsStringAsync();

            TestOutput.WriteLine(actual);

            Assert.Equal(StatusCodes.Status429TooManyRequests, (int)result.StatusCode);
            Assert.StartsWith("{\r\n  \"error\": {\r\n    \"instance\": \"http://localhost/fake/it\",\r\n    \"status\": 429,\r\n    \"code\": \"TooManyRequests\",\r\n    \"message\": \"Throttling rate limit quota violation. Quota limit exceeded.\"\r\n  }".ReplaceLineEndings(), actual.ReplaceLineEndings());
            Assert.Contains("Retry-After", result.Headers.Select(pair => pair.Key));
            Assert.Contains("RateLimit-Limit", result.Headers.Select(pair => pair.Key));
            Assert.Contains("RateLimit-Remaining", result.Headers.Select(pair => pair.Key));
            Assert.Contains("RateLimit-Reset", result.Headers.Select(pair => pair.Key));
            Assert.Equal(retryAfter, ratelimitReset);
        }
    }

    [Fact]
    public async Task OnActionExecutionAsync_ShouldThrowThrottlingException()
    {
        using (var filter = WebHostTestFactory.Create(services =>
               {
                   services
                       .AddControllers(o =>
                       {
                           o.Filters.AddThrottlingSentinel();
                       }).AddApplicationPart(typeof(FakeController).Assembly)
                       .AddJsonFormatters()
                       .AddThrottlingSentinelOptions(o =>
                       {
                           o.ContextResolver = context => nameof(OnActionExecutionAsync_ShouldCaptureThrottlingException);
                           o.Quota = new ThrottleQuota(10, 5, TimeUnit.Seconds);
                       });
                   services.AddMemoryThrottlingCache();
               }, app =>
                      {
                          app.UseRouting();
                          app.UseEndpoints(routes => { routes.MapControllers(); });
                      }))
        {
            var client = filter.Host.GetTestClient();

            HttpResponseMessage result = null;
            for (var i = 0; i < 10; i++)
            {
                result = await client.GetAsync("/fake/it");
                Assert.Equal(StatusCodes.Status200OK, (int)result.StatusCode);
                Assert.Equal("\"Unit Test\"", await result.Content.ReadAsStringAsync());
            }

            var te = await Assert.ThrowsAsync<ThrottlingException>(async () =>
            {
                result = await client.GetAsync("/fake/it");
            });

            var options = filter.Host.Services.GetRequiredService<IOptions<ThrottlingSentinelOptions>>().Value;

            var ratelimitReset = result.Headers.GetValues("RateLimit-Reset").Single().As<int>();
            Assert.Null(result.Headers.RetryAfter);
            Assert.Equal(StatusCodes.Status200OK, (int)result.StatusCode);
            Assert.Equal(StatusCodes.Status429TooManyRequests, te.StatusCode);
            Assert.Equal(10, te.RateLimit);
            Assert.Equal("\"Unit Test\"", await result.Content.ReadAsStringAsync());
            Assert.Contains("RateLimit-Limit", result.Headers.Select(pair => pair.Key));
            Assert.Contains("RateLimit-Remaining", result.Headers.Select(pair => pair.Key));
            Assert.Contains("RateLimit-Reset", result.Headers.Select(pair => pair.Key));
            Assert.Equal(10, result.Headers.Single(pair => pair.Key == options.RateLimitHeaderName).Value.Single().As<int>());
            Assert.Equal(0, result.Headers.Single(pair => pair.Key == options.RateLimitRemainingHeaderName).Value.Single().As<int>());
            Assert.Equal(4, result.Headers.Single(pair => pair.Key == options.RateLimitResetHeaderName).Value.Single().As<int>());
        }
    }
}
