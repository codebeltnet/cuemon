using System;
using System.Globalization;
using System.Threading.Tasks;
using Cuemon.Extensions.AspNetCore.Http.Headers;
using Codebelt.Extensions.Xunit;
using Codebelt.Extensions.Xunit.Hosting.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Xunit;

namespace Cuemon.AspNetCore.Http.Headers;
public class CacheableMiddlewareTest : Test
{
    public CacheableMiddlewareTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task InvokeAsync_ShouldUseDefaultCacheControl()
    {
        var utcNow = DateTime.UtcNow;
        var utcExpiresLow = utcNow.AddSeconds(604795);
        var utcExpiresHigh = utcNow.AddSeconds(604805);
        var cacheControlHeaderValue = string.Empty;
        var etagHeaderValue = string.Empty;
        var calculatedExpires = utcNow;
        var expiresAsDateTime = utcNow;

        await WebHostTestFactory.RunAsync(pipelineSetup: app =>
        {
            app.UseCacheControl();

            app.Use(async (context, next) =>
            {
                await context.Response.WriteAsync("This is a test.");
                await next();
            });
            app.Run(context =>
            {
                cacheControlHeaderValue = context.Response.Headers[HeaderNames.CacheControl];
                var expiresHeaderValue = context.Response.Headers[HeaderNames.Expires];
                etagHeaderValue = context.Response.Headers[HeaderNames.ETag];

                expiresAsDateTime = DateTime.ParseExact(expiresHeaderValue, "R", null, DateTimeStyles.None);
                calculatedExpires = utcNow.AddSeconds(604800);

                TestOutput.WriteLine(cacheControlHeaderValue);
                TestOutput.WriteLine(expiresHeaderValue);
                TestOutput.WriteLine(calculatedExpires.ToString("R"));

                return Task.CompletedTask;
            });
        });

        Assert.Equal("no-transform, public, must-revalidate, max-age=604800", cacheControlHeaderValue);

        Assert.InRange(expiresAsDateTime, utcExpiresLow, utcExpiresHigh); // ideally equal - but DateTime.UtcNow can have a small skew
        Assert.InRange(calculatedExpires, utcExpiresLow, utcExpiresHigh); // ideally equal - but DateTime.UtcNow can have a small skew

        Assert.Null(etagHeaderValue);
    }

    [Fact]
    public async Task InvokeAsync_ShouldUseDefaultCacheControl_WithoutExpires()
    {
        var cacheControlHeaderValue = string.Empty;
        var expiresHeaderValue = string.Empty;
        var etagHeaderValue = string.Empty;

        await WebHostTestFactory.RunAsync(pipelineSetup: app =>
        {
            app.UseCacheControl(o => o.Expires = null);

            app.Use(async (context, next) =>
            {
                await context.Response.WriteAsync("This is a test.");
                await next();
            });
            app.Run(context =>
            {
                cacheControlHeaderValue = context.Response.Headers[HeaderNames.CacheControl];
                expiresHeaderValue = context.Response.Headers[HeaderNames.Expires];
                etagHeaderValue = context.Response.Headers[HeaderNames.ETag];

                TestOutput.WriteLine(cacheControlHeaderValue);

                return Task.CompletedTask;
            });
        });

        Assert.Equal("no-transform, public, must-revalidate, max-age=604800", cacheControlHeaderValue);

        Assert.Null(expiresHeaderValue);

        Assert.Null(etagHeaderValue);
    }

    [Fact]
    public async Task InvokeAsync_ShouldUseDefaultCacheControl_WithoutCacheControl()
    {
        var utcNow = DateTime.UtcNow;
        var utcExpiresLow = utcNow.AddSeconds(604795);
        var utcExpiresHigh = utcNow.AddSeconds(604805);
        var cacheControlHeaderValue = string.Empty;
        var etagHeaderValue = string.Empty;
        var expiresAsDateTime = utcNow;

        await WebHostTestFactory.RunAsync(pipelineSetup: app =>
        {
            app.UseCacheControl(o => o.CacheControl = null);

            app.Use(async (context, next) =>
            {
                await context.Response.WriteAsync("This is a test.");
                await next();
            });
            app.Run(context =>
            {
                cacheControlHeaderValue = context.Response.Headers[HeaderNames.CacheControl];
                var expiresHeaderValue = context.Response.Headers[HeaderNames.Expires];
                etagHeaderValue = context.Response.Headers[HeaderNames.ETag];

                expiresAsDateTime = DateTime.ParseExact(expiresHeaderValue, "R", null, DateTimeStyles.None);

                TestOutput.WriteLine(expiresHeaderValue);

                return Task.CompletedTask;
            });
        });

        Assert.Null(cacheControlHeaderValue);

        Assert.InRange(expiresAsDateTime, utcExpiresLow, utcExpiresHigh); // ideally equal - but DateTime.UtcNow can have a small skew

        Assert.Null(etagHeaderValue);
    }

    [Fact]
    public async Task InvokeAsync_ShouldUseDefaultCacheControl_WithEtagValidator()
    {
        var utcNow = DateTime.UtcNow;
        var utcExpiresLow = utcNow.AddSeconds(604795);
        var utcExpiresHigh = utcNow.AddSeconds(604805);

        using (var middleware = WebHostTestFactory.Create(pipelineSetup: app =>
        {
            app.UseCacheControl(o => o.Validators.Add(new EntityTagCacheableValidator()));
            app.Run(async context => // simulate route to something writing to the body
            {
                await context.Response.WriteAsync("This is a test.");
            });
        }))
        {
            var context = middleware.Host.Services.GetRequiredService<IHttpContextAccessor>().HttpContext;
            var pipeline = middleware.Application.Build();

            await pipeline(context);

            var cacheControlHeaderValue = context.Response.Headers[HeaderNames.CacheControl];
            var expiresHeaderValue = context.Response.Headers[HeaderNames.Expires];
            var etagHeaderValue = context.Response.Headers[HeaderNames.ETag];

            var expiresAsDateTime = DateTime.ParseExact(expiresHeaderValue, "R", null, DateTimeStyles.None);
            var calculatedExpires = utcNow.AddSeconds(604800);

            TestOutput.WriteLine(cacheControlHeaderValue);
            TestOutput.WriteLine(expiresHeaderValue);
            TestOutput.WriteLine(calculatedExpires.ToString("R"));
            TestOutput.WriteLine(etagHeaderValue);

            Assert.Equal(StatusCodes.Status200OK, context.Response.StatusCode);
            Assert.Equal("no-transform, public, must-revalidate, max-age=604800", cacheControlHeaderValue);
            Assert.Equal("\"88b96039b6d7d57e7c7f30f4198882d5\"", etagHeaderValue);
            Assert.InRange(expiresAsDateTime, utcExpiresLow, utcExpiresHigh); // ideally equal - but DateTime.UtcNow can have a small skew
            Assert.InRange(calculatedExpires, utcExpiresLow, utcExpiresHigh); // ideally equal - but DateTime.UtcNow can have a small skew
        }
    }

    [Fact]
    public async Task InvokeAsync_ShouldUseDefaultCacheControl_WithEtagValidator_Expect_304()
    {
        var utcNow = DateTime.UtcNow;
        var utcExpiresLow = utcNow.AddSeconds(604795);
        var utcExpiresHigh = utcNow.AddSeconds(604805);

        using (var middleware = WebHostTestFactory.Create(pipelineSetup: app =>
        {
            app.UseCacheControl(o => o.Validators.Add(new EntityTagCacheableValidator()));
            app.Run(async context => // simulate route to something writing to the body
            {
                context.Request.Headers.Add(HeaderNames.IfNoneMatch, "\"88b96039b6d7d57e7c7f30f4198882d5\"");
                await context.Response.WriteAsync("This is a test.");
            });
        }))
        {
            var context = middleware.Host.Services.GetRequiredService<IHttpContextAccessor>().HttpContext;
            var pipeline = middleware.Application.Build();

            await pipeline(context);

            var cacheControlHeaderValue = context.Response.Headers[HeaderNames.CacheControl];
            var expiresHeaderValue = context.Response.Headers[HeaderNames.Expires];
            var etagHeaderValue = context.Response.Headers[HeaderNames.ETag];

            var expiresAsDateTime = DateTime.ParseExact(expiresHeaderValue, "R", null, DateTimeStyles.None);
            var calculatedExpires = utcNow.AddSeconds(604800);

            TestOutput.WriteLine(cacheControlHeaderValue);
            TestOutput.WriteLine(expiresHeaderValue);
            TestOutput.WriteLine(calculatedExpires.ToString("R"));
            TestOutput.WriteLine(etagHeaderValue);

            Assert.Equal(StatusCodes.Status304NotModified, context.Response.StatusCode);
            Assert.Equal("no-transform, public, must-revalidate, max-age=604800", cacheControlHeaderValue);
            Assert.Equal("\"88b96039b6d7d57e7c7f30f4198882d5\"", etagHeaderValue);
            Assert.InRange(expiresAsDateTime, utcExpiresLow, utcExpiresHigh); // ideally equal - but DateTime.UtcNow can have a small skew
            Assert.InRange(calculatedExpires, utcExpiresLow, utcExpiresHigh); // ideally equal - but DateTime.UtcNow can have a small skew
        }
    }
}
