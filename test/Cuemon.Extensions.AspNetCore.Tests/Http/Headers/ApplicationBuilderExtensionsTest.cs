using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Codebelt.Extensions.Xunit.Hosting.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Xunit;

namespace Cuemon.Extensions.AspNetCore.Http.Headers
{
    public class ApplicationBuilderExtensionsTest : Test
    {
        public ApplicationBuilderExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task UseCorrelationIdentifier_ShouldAddConfiguredHeader()
        {
            using var response = await WebHostTestFactory.RunAsync(pipelineSetup: app =>
            {
                app.UseCorrelationIdentifier(o => o.HeaderName = "X-Test-Correlation");
                app.Run(context => context.Response.WriteAsync("ok"));
            });

            Assert.True(response.Headers.TryGetValues("X-Test-Correlation", out var headerValues));
            Assert.False(string.IsNullOrWhiteSpace(System.Linq.Enumerable.Single(headerValues)));
        }

        [Fact]
        public async Task UseRequestIdentifier_ShouldAddConfiguredHeader()
        {
            using var response = await WebHostTestFactory.RunAsync(pipelineSetup: app =>
            {
                app.UseRequestIdentifier(o => o.HeaderName = "X-Test-Request");
                app.Run(context => context.Response.WriteAsync("ok"));
            });

            Assert.True(response.Headers.TryGetValues("X-Test-Request", out var headerValues));
            Assert.False(string.IsNullOrWhiteSpace(System.Linq.Enumerable.Single(headerValues)));
        }

        [Fact]
        public async Task UseUserAgentSentinel_ShouldAllowKnownUserAgent()
        {
            using var host = WebHostTestFactory.Create(
                services =>
                {
                    services.AddRouting();
                    services.AddUserAgentSentinelOptions(o =>
                    {
                        o.RequireUserAgentHeader = true;
                        o.ValidateUserAgentHeader = true;
                        o.AllowedUserAgents.Add("Cuemon-Agent");
                    });
                },
                app =>
                {
                    app.UseUserAgentSentinel();
                    app.UseRouting();
                    app.UseEndpoints(endpoints => endpoints.MapGet("/", () => "ok"));
                });

            var client = host.Host.GetTestClient();
            client.DefaultRequestHeaders.Add(HeaderNames.UserAgent, "Cuemon-Agent");
            using var response = await client.GetAsync("/");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task UseApiKeySentinel_ShouldAllowKnownApiKey()
        {
            using var host = WebHostTestFactory.Create(
                services =>
                {
                    services.AddRouting();
                    services.AddApiKeySentinelOptions(o =>
                    {
                        o.AllowedKeys.Add("known-key");
                        o.HeaderName = "X-Test-Key";
                    });
                },
                app =>
                {
                    app.UseApiKeySentinel();
                    app.UseRouting();
                    app.UseEndpoints(endpoints => endpoints.MapGet("/", () => "ok"));
                });

            var client = host.Host.GetTestClient();
            client.DefaultRequestHeaders.Add("X-Test-Key", "known-key");
            using var response = await client.GetAsync("/");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task UseCacheControl_ShouldAddCacheHeaders()
        {
            using var response = await WebHostTestFactory.RunAsync(pipelineSetup: app =>
            {
                app.UseCacheControl();
                app.Run(context => context.Response.WriteAsync("payload"));
            });

            Assert.True(response.Headers.Contains(HeaderNames.CacheControl));
            Assert.NotNull(response.Content.Headers.Expires);
        }

        [Fact]
        public async Task UseVaryAccept_ShouldAddVaryHeader()
        {
            using var response = await WebHostTestFactory.RunAsync(pipelineSetup: app =>
            {
                app.UseVaryAccept();
                app.Run(context => context.Response.WriteAsync("payload"));
            });

            Assert.True(response.Headers.TryGetValues(HeaderNames.Vary, out var values));
            Assert.Contains(HeaderNames.Accept, values);
        }
    }
}
