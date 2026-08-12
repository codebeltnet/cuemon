using System;
using System.Linq;
using System.Threading.Tasks;
using Cuemon.Diagnostics;
using Cuemon.Extensions.AspNetCore.Diagnostics;
using Cuemon.Extensions.AspNetCore.Http.Headers;
using Cuemon.Extensions.IO;
using Codebelt.Extensions.Xunit;
using Codebelt.Extensions.Xunit.Hosting.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Xunit;

namespace Cuemon.AspNetCore.Http.Headers;
public class ApiKeySentinelMiddlewareTest : Test
{
    public ApiKeySentinelMiddlewareTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task InvokeAsync_ShouldThrowApiKeyException_BadRequest()
    {
        using (var middleware = WebHostTestFactory.Create(pipelineSetup: app =>
        {
            app.UseApiKeySentinel();
        }))
        {
            var context = middleware.Host.Services.GetRequiredService<IHttpContextAccessor>().HttpContext;
            var options = middleware.Host.Services.GetRequiredService<IOptions<ApiKeySentinelOptions>>();
            var pipeline = middleware.Application.Build();

            var uae = await Assert.ThrowsAsync<ApiKeyException>(async () => await pipeline(context));

            Assert.Equal(uae.Message, options.Value.GenericClientMessage);
            Assert.Equal(uae.StatusCode, StatusCodes.Status400BadRequest);
            Assert.False(options.Value.AllowedKeys.Any());
        }
    }

    [Fact]
    public async Task InvokeAsync_ShouldThrowApiKeyException_Forbidden()
    {
        using (var middleware = WebHostTestFactory.Create(services =>
               {
                   services.AddApiKeySentinelOptions(o =>
                   {
                       o.AllowedKeys.Add(Guid.NewGuid().ToString());
                   });
               }, app =>
               {
                   app.UseApiKeySentinel();
               }))
        {
            var context = middleware.Host.Services.GetRequiredService<IHttpContextAccessor>().HttpContext;
            var options = middleware.Host.Services.GetRequiredService<IOptions<ApiKeySentinelOptions>>();
            var pipeline = middleware.Application.Build();

            context.Request.Headers.Add(options.Value.HeaderName, "Invalid-Key");

            var uae = await Assert.ThrowsAsync<ApiKeyException>(async () => await pipeline(context));

            Assert.Equal(uae.Message, options.Value.ForbiddenMessage);
            Assert.Equal(uae.StatusCode, StatusCodes.Status403Forbidden);
            Assert.True(options.Value.AllowedKeys.Any());
        }
    }

    [Fact]
    public async Task InvokeAsync_ShouldCaptureApiKeyException_Forbidden()
    {
        using (var middleware = WebHostTestFactory.Create(services =>
               {
                   services.AddFaultDescriptorOptions(o => o.SensitivityDetails = FaultSensitivityDetails.All);
                   services.AddApiKeySentinelOptions(o =>
                   {
                       o.AllowedKeys.Add(Guid.NewGuid().ToString());
                   });
               }, app =>
                      {
                          app.UseFaultDescriptorExceptionHandler();
                          app.UseApiKeySentinel();

                      }))
        {
            var context = middleware.Host.Services.GetRequiredService<IHttpContextAccessor>().HttpContext;
            var options = middleware.Host.Services.GetRequiredService<IOptions<ApiKeySentinelOptions>>();
            var pipeline = middleware.Application.Build();

            context.Request.Headers.Add(options.Value.HeaderName, "Invalid-Key");

            await pipeline(context);

            TestOutput.WriteLine(context.Response.Body.ToEncodedString(o => o.LeaveOpen = true));

            Assert.True(options.Value.AllowedKeys.Any());
            Assert.Equal(StatusCodes.Status403Forbidden, context.Response.StatusCode);
            Assert.Contains(options.Value.ForbiddenMessage, context.Response.Body.ToEncodedString());
        }
    }

    [Fact]
    public async Task InvokeAsync_ShouldCaptureApiKeyException_BadRequest()
    {
        using (var middleware = WebHostTestFactory.Create(pipelineSetup: app =>
               {
                   app.UseFaultDescriptorExceptionHandler();
                   app.UseApiKeySentinel();

               }))
        {
            var context = middleware.Host.Services.GetRequiredService<IHttpContextAccessor>().HttpContext;
            var options = middleware.Host.Services.GetRequiredService<IOptions<ApiKeySentinelOptions>>();
            var pipeline = middleware.Application.Build();

            await pipeline(context);

            Assert.False(options.Value.AllowedKeys.Any());
            Assert.Equal(StatusCodes.Status400BadRequest, context.Response.StatusCode);
            Assert.Contains(options.Value.GenericClientMessage, context.Response.Body.ToEncodedString());
        }
    }

    [Fact]
    public async Task InvokeAsync_ShouldThrowApiKeyException_BadRequest_BecauseOfUseGenericResponse()
    {
        var allowedKey = Generate.RandomString(24);
        using (var middleware = WebHostTestFactory.Create(services =>
        {
            services.AddApiKeySentinelOptions(o =>
            {
                o.UseGenericResponse = true;
                o.AllowedKeys.Add(allowedKey);
            });
        }, app =>
               {
                   app.UseApiKeySentinel();
               }))
        {
            var context = middleware.Host.Services.GetRequiredService<IHttpContextAccessor>().HttpContext;
            var options = middleware.Host.Services.GetRequiredService<IOptions<ApiKeySentinelOptions>>();
            var pipeline = middleware.Application.Build();

            context.Request.Headers.Add(options.Value.HeaderName, "Invalid-Key");

            var uae = await Assert.ThrowsAsync<ApiKeyException>(async () => await pipeline(context));

            Assert.Equal(uae.Message, options.Value.GenericClientMessage);
            Assert.Equal(uae.StatusCode, StatusCodes.Status400BadRequest);

            Assert.True(options.Value.UseGenericResponse);
            Assert.True(options.Value.AllowedKeys.Any());
            Assert.Equal(allowedKey, options.Value.AllowedKeys.First());
        }
    }

    [Fact]
    public async Task InvokeAsync_ShouldNotAllowRequest()
    {
        using (var middleware = WebHostTestFactory.Create(pipelineSetup: app =>
        {
            app.UseFaultDescriptorExceptionHandler();
            app.UseApiKeySentinel();
            app.Run(context =>
            {
                context.Response.StatusCode = 200;
                return Task.CompletedTask;
            });
        }))
        {
            var context = middleware.Host.Services.GetRequiredService<IHttpContextAccessor>().HttpContext;
            var options = middleware.Host.Services.GetRequiredService<IOptions<ApiKeySentinelOptions>>();
            var pipeline = middleware.Application.Build();

            await pipeline(context);

            Assert.Equal(StatusCodes.Status400BadRequest, context.Response.StatusCode); // 400 because we did not specify X-Api-Key
        }
    }

    [Fact]
    public async Task InvokeAsync_ShouldAllowRequestAfterBeingValidated()
    {
        var allowedKey = Generate.RandomString(24);
        using (var middleware = WebHostTestFactory.Create(services =>
        {
            services.AddApiKeySentinelOptions(o =>
            {
                o.AllowedKeys.Add(allowedKey);
            });
        }, app =>
               {
                   app.UseApiKeySentinel();
                   app.Run(context =>
                   {
                       context.Response.StatusCode = 200;
                       return Task.CompletedTask;
                   });
               }))
        {
            var context = middleware.Host.Services.GetRequiredService<IHttpContextAccessor>().HttpContext;
            var options = middleware.Host.Services.GetRequiredService<IOptions<ApiKeySentinelOptions>>();
            var pipeline = middleware.Application.Build();

            context.Request.Headers.Add(options.Value.HeaderName, allowedKey);

            await pipeline(context);

            Assert.Equal(StatusCodes.Status200OK, context.Response.StatusCode);
        }
    }
}
