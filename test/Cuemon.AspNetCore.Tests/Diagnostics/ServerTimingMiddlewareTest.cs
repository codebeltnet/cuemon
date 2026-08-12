using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cuemon.Extensions.AspNetCore.Diagnostics;
using Codebelt.Extensions.Xunit;
using Codebelt.Extensions.Xunit.Hosting;
using Codebelt.Extensions.Xunit.Hosting.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Cuemon.AspNetCore.Diagnostics;
public class ServerTimingMiddlewareTest : Test
{
    public ServerTimingMiddlewareTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task InvokeAsync_ShouldMimicSimpleAspNetProject()
    {
        using var response = await WebHostTestFactory.RunAsync(
            services =>
            {
                services.AddServerTiming(o => o.SuppressHeaderPredicate = _ => false);
            }
            , app =>
            {
                app.UseServerTiming();
                app.Use(async (context, next) =>
                {
                    var sw = Stopwatch.StartNew();
                    context.Response.OnStarting(() =>
                    {
                        sw.Stop();
                        context.RequestServices.GetRequiredService<IServerTiming>().AddServerTiming("use-middleware", sw.Elapsed);
                        return Task.CompletedTask;
                    });
                    await next(context).ConfigureAwait(false);
                });
                app.Run(context =>
                {
                    Thread.Sleep(400);
                    return context.Response.WriteAsync("Hello World!");
                });
            }).ConfigureAwait(false);

        Assert.StartsWith("use-middleware;dur=", response.Headers.Single(kvp => kvp.Key == ServerTiming.HeaderName).Value.FirstOrDefault());
    }

    [Fact]
    public async Task InvokeAsync_ShouldProviderServerTimingHeaderWithMetrics()
    {
        using var response = await WebHostTestFactory.RunAsync(services =>
        {
            services.AddServerTiming();
        }, app =>
        {
            app.Use(async (context, next) =>
            {
                var serverTiming = context.RequestServices.GetRequiredService<IServerTiming>();

                serverTiming.AddServerTiming("redis", TimeSpan.FromMilliseconds(22), "Redis Cache");
                serverTiming.AddServerTiming("restApi", TimeSpan.FromSeconds(1.7), "Some REST API integration");

                await next(context);
            });

            app.UseServerTiming();
        }).ConfigureAwait(false);

        var header = response.Headers.GetValues(ServerTiming.HeaderName).ToArray();

        TestOutput.WriteLines(header);

        Assert.Equal("redis", header[0].Split(';').First());
        Assert.Equal("restApi", header[1].Split(';').First());
    }

    [Fact]
    public async Task InvokeAsync_ShouldExcludeServerTimingHeaderWithMetrics_ButIncludeLogLevelDebug()
    {
        using var webApp = WebHostTestFactory.Create(services =>
        {
            services.AddServerTiming(o => o.SuppressHeaderPredicate = _ => true);
            services.AddXunitTestLogging(TestOutput);
        }, app =>
        {
            app.Use(async (context, next) =>
            {
                var serverTiming = context.RequestServices.GetRequiredService<IServerTiming>();

                serverTiming.AddServerTiming("redis", TimeSpan.FromMilliseconds(22), "Redis Cache");
                serverTiming.AddServerTiming("restApi", TimeSpan.FromSeconds(1.7), "Some REST API integration");

                await next(context);
            });
            app.UseServerTiming();
        });
        {
            var logger = webApp.Host.Services.GetRequiredService<ILogger<ServerTimingMiddleware>>();
            var loggerStore = logger.GetTestStore();

            var client = webApp.Host.GetTestClient();
            var response = await client.GetAsync("/").ConfigureAwait(false);

            response.Headers.TryGetValues(ServerTiming.HeaderName, out var serverTimingHeader);

            Assert.Null(serverTimingHeader);

            Assert.Collection(loggerStore.Query(),
                entry => Assert.Equal("Debug: ServerTimingMetric { Name: redis, Duration: 22.0ms, Description: \"Redis Cache\" }", entry.ToString()),
                entry => Assert.Equal("Debug: ServerTimingMetric { Name: restApi, Duration: 1700.0ms, Description: \"Some REST API integration\" }", entry.ToString()));
        }
    }

    [Fact]
    public async Task InvokeAsync_ShouldNotProviderServerTimingHeaderWithMetrics()
    {
        using var response = await WebHostTestFactory.RunAsync(services =>
        {
            services.AddServerTiming();
        }, app =>
        {
            app.Use(async (context, next) =>
            {
                var serverTiming = context.RequestServices.GetRequiredService<IServerTiming>();

                serverTiming.AddServerTiming("redis", TimeSpan.FromMilliseconds(22), "Redis Cache");
                serverTiming.AddServerTiming("restApi", TimeSpan.FromSeconds(1.7), "Some REST API integration");

                await next(context);
            });
            //app.UseServerTiming();
        }).ConfigureAwait(false);

        response.Headers.TryGetValues(ServerTiming.HeaderName, out var serverTimingHeader);

        Assert.Null(serverTimingHeader);
    }
}
