using System;
using System.Threading.Tasks;
using Cuemon.Extensions.AspNetCore.Diagnostics;
using Cuemon.Extensions.AspNetCore.Http.Throttling;
using Cuemon.Extensions.IO;
using Codebelt.Extensions.Xunit;
using Codebelt.Extensions.Xunit.Hosting.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Cuemon.AspNetCore.Http.Throttling;
public class ThrottlingSentinelMiddlewareTest : Test
{
    public ThrottlingSentinelMiddlewareTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task InvokeAsync_ShouldThrowThrottlingException_TooManyRequests()
    {
        using (var middleware = WebHostTestFactory.Create(services =>
        {
            services.Configure<ThrottlingSentinelOptions>(o =>
            {
                o.Quota = new ThrottleQuota(10, TimeSpan.FromMinutes(5));
                o.ContextResolver = cr => nameof(ThrottlingSentinelMiddlewareTest);
            });
            services.AddFakeHttpContextAccessor(ServiceLifetime.Singleton);
            services.AddMemoryThrottlingCache();
        }, app =>
               {
                   app.UseThrottlingSentinel();
               }))
        {
            var context = middleware.Host.Services.GetRequiredService<IHttpContextAccessor>().HttpContext;
            var options = middleware.Host.Services.GetRequiredService<IOptions<ThrottlingSentinelOptions>>();
            var cache = middleware.Host.Services.GetRequiredService<IThrottlingCache>();
            var pipeline = middleware.Application.Build();

            var te = await Assert.ThrowsAsync<ThrottlingException>(async () =>
            {
                for (var i = 0; i < 15; i++)
                {
                    await pipeline(context);
                }
            });

            var ce = cache[nameof(ThrottlingSentinelMiddlewareTest)];
            Assert.InRange(ce.Total, te.RateLimit, 15);

            Assert.Equal(te.RateLimit, options.Value.Quota.RateLimit);
            Assert.Equal(te.Message, options.Value.TooManyRequestsMessage);
            Assert.Equal(te.StatusCode, StatusCodes.Status429TooManyRequests);
            Assert.True(options.Value.UseRetryAfterHeader);
        }
    }

    [Fact]
    public async Task InvokeAsync_ShouldCaptureThrottlingException_TooManyRequests()
    {
        using (var middleware = WebHostTestFactory.Create(services =>
        {
            services.Configure<ThrottlingSentinelOptions>(o =>
            {
                o.Quota = new ThrottleQuota(10, TimeSpan.FromMinutes(5));
                o.ContextResolver = cr => nameof(ThrottlingSentinelMiddlewareTest);
            });
            services.AddFakeHttpContextAccessor(ServiceLifetime.Singleton);
            services.AddMemoryThrottlingCache();
        }, app =>
               {
                   app.UseFaultDescriptorExceptionHandler();
                   app.UseThrottlingSentinel();
               }))
        {
            var context = middleware.Host.Services.GetRequiredService<IHttpContextAccessor>().HttpContext;
            var options = middleware.Host.Services.GetRequiredService<IOptions<ThrottlingSentinelOptions>>();
            var cache = middleware.Host.Services.GetRequiredService<IThrottlingCache>();
            var pipeline = middleware.Application.Build();

            for (var i = 0; i < 15; i++)
            {
                if (!context.Response.HasStarted) // exceptionhandler will start response on first exception
                {
                    await pipeline(context);
                }
            }

            TestOutput.WriteLines(context.Response.Headers);

            Assert.Equal(options.Value.Quota.RateLimit, Convert.ToInt32(context.Response.Headers[options.Value.RateLimitHeaderName]));
            Assert.Equal(0, Convert.ToInt32(context.Response.Headers[options.Value.RateLimitRemainingHeaderName]));
            Assert.Equal(299, Convert.ToInt32(context.Response.Headers[options.Value.RateLimitResetHeaderName]));
            Assert.True(options.Value.UseRetryAfterHeader);
            Assert.Equal(StatusCodes.Status429TooManyRequests, context.Response.StatusCode);
            Assert.Contains(options.Value.TooManyRequestsMessage, context.Response.Body.ToEncodedString());
        }
    }

    [Fact]
    public async Task InvokeAsync_ShouldRehydrate()
    {
        var window = TimeSpan.FromSeconds(5);
        using (var middleware = WebHostTestFactory.Create(services =>
        {
            services.Configure<ThrottlingSentinelOptions>(o =>
            {
                o.Quota = new ThrottleQuota(10, window);
                o.ContextResolver = cr => nameof(ThrottlingSentinelMiddlewareTest);
            });
            services.AddFakeHttpContextAccessor(ServiceLifetime.Singleton);
            services.AddMemoryThrottlingCache();
        }, app =>
               {
                   app.UseThrottlingSentinel();
                   app.Run(context =>
                   {
                       context.Response.StatusCode = 200;
                       return Task.CompletedTask;
                   });
               }))
        {
            var context = middleware.Host.Services.GetRequiredService<IHttpContextAccessor>().HttpContext;
            var options = middleware.Host.Services.GetRequiredService<IOptions<ThrottlingSentinelOptions>>();
            var pipeline = middleware.Application.Build();

            for (var i = 0; i < 10; i++)
            {
                await pipeline(context);
            }

            var te = await Assert.ThrowsAsync<ThrottlingException>(async () => await pipeline(context));

            TestOutput.WriteLine(te.Delta.ToString());

            await Task.Delay(te.Delta.Add(TimeSpan.FromSeconds(1)));

            await pipeline(context);

            Assert.True(window >= te.Delta, "window >= te.Delta");
            Assert.True(options.Value.UseRetryAfterHeader);
            Assert.Equal(StatusCodes.Status200OK, context.Response.StatusCode);
        }
    }
}
