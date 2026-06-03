using System;
using System.Net;
using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Codebelt.Extensions.Xunit.Hosting.AspNetCore;
using Cuemon.AspNetCore.Http.Throttling;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Cuemon.Extensions.AspNetCore.Http.Throttling
{
    public class ApplicationBuilderExtensionsTest : Test
    {
        public ApplicationBuilderExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task UseThrottlingSentinel_ShouldThrottleSecondRequest_WhenQuotaIsExceeded()
        {
            using var host = WebHostTestFactory.Create(
                services =>
                {
                    services.AddRouting();
                    services.AddMemoryThrottlingCache();
                    services.AddThrottlingSentinelOptions(o =>
                    {
                        o.ContextResolver = _ => "global";
                        o.Quota = new ThrottleQuota(1, TimeSpan.FromMinutes(1));
                    });
                },
                app =>
                {
                    app.UseThrottlingSentinel();
                    app.UseRouting();
                    app.UseEndpoints(endpoints => endpoints.MapGet("/", () => "ok"));
                });

            var client = host.Host.GetTestClient();
            using var first = await client.GetAsync("/");

            var second = await Assert.ThrowsAsync<ThrottlingException>(() => client.GetAsync("/"));

            Assert.Equal(HttpStatusCode.OK, first.StatusCode);
            Assert.Equal("Throttling rate limit quota violation. Quota limit exceeded.", second.Message);
        }
    }
}
