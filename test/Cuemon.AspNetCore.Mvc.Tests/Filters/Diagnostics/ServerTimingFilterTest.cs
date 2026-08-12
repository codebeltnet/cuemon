using System;
using System.Linq;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.AspNetCore.Mvc.Assets;
using Cuemon.Diagnostics;
using Cuemon.Extensions;
using Cuemon.Extensions.AspNetCore.Diagnostics;
using Cuemon.Extensions.AspNetCore.Mvc.Filters;
using Codebelt.Extensions.Xunit;
using Codebelt.Extensions.Xunit.Hosting;
using Codebelt.Extensions.Xunit.Hosting.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Xunit;

namespace Cuemon.AspNetCore.Mvc.Filters.Diagnostics;
public class ServerTimingFilterTest : Test
{
    public ServerTimingFilterTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task OnActionExecutionAsync_ShouldTimeMeasureFakeControllerAndFictiveMeasurements()
    {
        using (var filter = WebHostTestFactory.Create(services =>
        {
            services.AddServerTiming();
            services.AddControllers(o => { o.Filters.AddServerTiming(); }).AddApplicationPart(typeof(FakeController).Assembly);
        }, app =>
               {
                   app.UseRouting();
                   app.Use(async (context, next) =>
                   {
                       var serverTiming = context.RequestServices.GetRequiredService<IServerTiming>();

                       await Task.Delay(22);
                       serverTiming.AddServerTiming("redis", TimeSpan.FromMilliseconds(22), "Redis Cache");

                       await Task.Delay(1700);
                       serverTiming.AddServerTiming("restApi", TimeSpan.FromSeconds(1.7), "Some REST API integration");

                       await next();
                   });
                   app.UseEndpoints(routes => { routes.MapControllers(); });
               }))
        {
            var client = filter.Host.GetTestClient();
            var profiler = await TimeMeasure.WithFuncAsync(client.GetAsync, "/fake/oneSecond");
            var serverTimings = profiler.Result.Headers.GetValues(ServerTiming.HeaderName).ToArray();

            Assert.InRange(profiler.Elapsed, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            Assert.True(profiler.Result.Headers.Contains("Server-Timing"));
            Assert.Equal("redis", serverTimings[0].Split(';').First());
            Assert.Equal("restApi", serverTimings[1].Split(';').First());
            Assert.Equal("sapIntegration", serverTimings[2].Split(';').First());

            TestOutput.WriteLine(serverTimings.ToDelimitedString());
        }
    }

    [Fact]
    public async Task OnActionExecutionAsync_ShouldTimeMeasureFakeController()
    {
        using (var filter = WebHostTestFactory.Create(services =>
        {
            services.AddServerTiming();
            services.AddControllers(o => { o.Filters.AddServerTiming(); }).AddApplicationPart(typeof(FakeController).Assembly);
        }, app =>
               {
                   app.UseRouting();
                   app.UseEndpoints(routes => { routes.MapControllers(); });
               }))
        {
            var client = filter.Host.GetTestClient();
            var profiler = await TimeMeasure.WithFuncAsync(client.GetAsync, "/fake/oneSecond");

            Assert.InRange(profiler.Elapsed, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            Assert.True(profiler.Result.Headers.Contains("Server-Timing"));
            Assert.StartsWith("sapIntegration", profiler.Result.Headers.GetValues(ServerTiming.HeaderName).Single());

            TestOutput.WriteLine(profiler.Elapsed.ToString());
        }
    }

    [Fact]
    public async Task OnActionExecutionAsync_ShouldTimeMeasureFakeController_Automatically()
    {
        using (var filter = WebHostTestFactory.Create(services =>
               {
                   services.AddServerTiming(o => o.UseTimeMeasureProfiler = true);
                   services.AddControllers(o => { o.Filters.AddServerTiming(); }).AddApplicationPart(typeof(FakeController).Assembly);
               }, app =>
               {
                   app.UseRouting();
                   app.UseEndpoints(routes => { routes.MapControllers(); });
               }))
        {
            var client = filter.Host.GetTestClient();
            var profiler = await TimeMeasure.WithFuncAsync(client.GetAsync, "/fake/oneSecondNoServerTimingFilter");
            var serverTimingHeader = profiler.Result.Headers.GetValues(ServerTiming.HeaderName).Single();

            Assert.InRange(profiler.Elapsed, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            Assert.True(profiler.Result.Headers.Contains("Server-Timing"));
            Assert.StartsWith("GetAfter1SecondNoServerTimingFilter", serverTimingHeader);

            TestOutput.WriteLine(serverTimingHeader);
        }
    }

    [Fact]
    public async Task OnActionExecutionAsync_ShouldNotTimeMeasureFakeController_Automatically()
    {
        using (var filter = WebHostTestFactory.Create(services =>
               {
                   services.AddServerTiming();
                   services.AddControllers(o => { o.Filters.AddServerTiming(); }).AddApplicationPart(typeof(FakeController).Assembly);
               }, app =>
               {
                   app.UseRouting();
                   app.UseEndpoints(routes => { routes.MapControllers(); });
               }))
        {
            var client = filter.Host.GetTestClient();
            var profiler = await TimeMeasure.WithFuncAsync(client.GetAsync, "/fake/oneSecondNoServerTimingFilter");

            Assert.InRange(profiler.Elapsed, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            Assert.False(profiler.Result.Headers.Contains("Server-Timing"));
        }
    }

    [Fact]
    public async Task ServerTimingAttribute_ShouldTimeMeasureFakeController_GetAfter1SecondDecorated()
    {
        using (var filter = WebHostTestFactory.Create(services =>
        {
            services.AddServerTiming();
            services.AddControllers().AddApplicationPart(typeof(FakeController).Assembly);
        }, app =>
               {
                   app.UseRouting();
                   app.UseEndpoints(routes => { routes.MapControllers(); });
               }))
        {
            var client = filter.Host.GetTestClient();
            var profiler = await TimeMeasure.WithFuncAsync(client.GetAsync, "/fake/oneSecondAttribute");

            Assert.InRange(profiler.Elapsed, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            Assert.True(profiler.Result.Headers.Contains("Server-Timing"));
            Assert.StartsWith("action-result", profiler.Result.Headers.GetValues(ServerTiming.HeaderName).Single());

            TestOutput.WriteLine(profiler.Elapsed.ToString());
        }
    }

    [Fact]
    public async Task ServerTimingAttribute_ShouldTimeMeasureFakeController_GetAfter1SecondDecoratedWithDefaults()
    {
        using (var filter = WebHostTestFactory.Create(services =>
               {
                   services.AddXunitTestLogging(TestOutput, LogLevel.Debug);
                   services.AddServerTiming();
                   services.AddControllers().AddApplicationPart(typeof(FakeController).Assembly);
               }, app =>
               {
                   app.UseRouting();
                   app.UseEndpoints(routes => { routes.MapControllers(); });
               }))
        {
            var client = filter.Host.GetTestClient();
            var profiler = await TimeMeasure.WithFuncAsync(client.GetAsync, "/fake/oneSecondAttributeWithDefaults");
            var serverTimingHeader = profiler.Result.Headers.GetValues(ServerTiming.HeaderName).Single();
            var loggerStore = filter.Host.Services.GetRequiredService<ILogger<ServerTimingFilter>>().GetTestStore();

            Assert.InRange(profiler.Elapsed, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            Assert.True(profiler.Result.Headers.Contains("Server-Timing"));
            Assert.StartsWith("GetAfter1SecondDecorated", serverTimingHeader);
            Assert.True(loggerStore.Query(entry => entry.Message.StartsWith("Debug: ServerTimingMetric { Name: GetAfter1SecondDecoratedWithDefaults, Duration:") &&
                                                   entry.Message.EndsWith("ms, Description: \"http://localhost/fake/onesecondattributewithdefaults\" }")).Any());
        }
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task ServerTimingAttribute_ShouldTimeMeasureFakeController_GetAfter1SecondDecoratedWithDefaults_ShouldOnlyRenderOnce(bool registerAsGlobalFilter)
    {
        using (var filter = WebHostTestFactory.Create(services =>
               {
                   services.AddXunitTestLogging(TestOutput, LogLevel.Debug);
                   services.AddServerTiming(o => o.UseTimeMeasureProfiler = registerAsGlobalFilter);
                   services
                       .AddControllers(o =>
                       {
                           o.Filters.AddServerTiming();
                       })
                       .AddApplicationPart(typeof(FakeController).Assembly);
               }, app =>
               {
                   app.UseRouting();
                   app.UseEndpoints(routes => { routes.MapControllers(); });
               }))
        {
            var client = filter.Host.GetTestClient();
            var profiler = await TimeMeasure.WithFuncAsync(client.GetAsync, "/fake/oneSecondAttributeWithDefaults");
            var serverTimingHeader = profiler.Result.Headers.GetValues(ServerTiming.HeaderName).Single();
            var loggerStore = filter.Host.Services.GetRequiredService<ILogger<ServerTimingFilter>>().GetTestStore();

            Assert.InRange(profiler.Elapsed, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            Assert.True(profiler.Result.Headers.Contains("Server-Timing"));
            Assert.StartsWith("GetAfter1SecondDecorated", serverTimingHeader);
            Assert.True(loggerStore.Query(entry => entry.Message.StartsWith("Debug: ServerTimingMetric { Name: GetAfter1SecondDecoratedWithDefaults, Duration:") &&
                                                   entry.Message.EndsWith("ms, Description: \"http://localhost/fake/onesecondattributewithdefaults\" }")).Any());
        }
    }

    [Fact]
    public async Task ServerTimingAttribute_ShouldSuppressTimeMeasureFakeControllerAndIncludeLogInformation_GetAfter1SecondDecorated()
    {
        using (var filter = WebHostTestFactory.Create(services =>
        {
            services.AddServerTiming();
            services.AddXunitTestLogging(TestOutput, LogLevel.Information);
            services.AddControllers().AddApplicationPart(typeof(FakeController).Assembly);
        }, app =>
               {
                   app.UseRouting();
                   app.UseEndpoints(routes => { routes.MapControllers(); });
               }, host =>
               {
                   host.ConfigureWebHost(builder => builder.UseEnvironment("Production"));
               }))
        {
            var options = filter.Host.Services.GetRequiredService<IOptions<ServerTimingOptions>>();
            var client = filter.Host.GetTestClient();
            var loggerStore = filter.Host.Services.GetRequiredService<ILogger<ServerTimingFilter>>().GetTestStore();

            var profiler = await TimeMeasure.WithFuncAsync(client.GetAsync, "/fake/oneSecondAttribute");

            Assert.InRange(profiler.Elapsed, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            Assert.True(options.Value.SuppressHeaderPredicate(filter.Environment));
            Assert.False(profiler.Result.Headers.Contains("Server-Timing"));
            Assert.True(loggerStore.Query(entry => entry.Message.StartsWith("Information: ServerTimingMetric { Name: action-result") && entry.Message.EndsWith("Description: \"action-description\" }")).Any(), "loggerStore.Query(entry => entry.Message.StartsWith('Information: ServerTimingMetric { Name: action-result')).Any()");

            TestOutput.WriteLine(profiler.Elapsed.ToString());
        }
    }

    [Fact]
    public async Task ServerTimingAttribute_ShouldSuppressTimeMeasureFakeController_GetAfter1Second()
    {
        using (var filter = WebHostTestFactory.Create(services =>
               {
                   services.AddServerTiming(o => o.SuppressHeaderPredicate = _ => true);
                   services.AddControllers().AddApplicationPart(typeof(FakeController).Assembly);
               }, app =>
               {
                   app.UseRouting();
                   app.UseEndpoints(routes => { routes.MapControllers(); });
               }))
        {
            var options = filter.Host.Services.GetRequiredService<IOptions<ServerTimingOptions>>();
            var client = filter.Host.GetTestClient();

            var profiler = await TimeMeasure.WithFuncAsync(client.GetAsync, "/fake/oneSecond");

            Assert.InRange(profiler.Elapsed, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            Assert.True(options.Value.SuppressHeaderPredicate(filter.Environment));
            Assert.False(profiler.Result.Headers.Contains("Server-Timing"));

            TestOutput.WriteLine(profiler.Elapsed.ToString());
        }
    }
}
