using System.Linq;
using System.Threading.Tasks;
using Cuemon.Extensions.AspNetCore.Diagnostics;
using Cuemon.Extensions.AspNetCore.Http.Headers;
using Cuemon.Extensions.IO;
using Codebelt.Extensions.Xunit;
using Codebelt.Extensions.Xunit.Hosting.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Xunit;

namespace Cuemon.AspNetCore.Http.Headers;
public class UserAgentSentinelMiddlewareTest : Test
{
    public UserAgentSentinelMiddlewareTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task InvokeAsync_ShouldThrowUserAgentException_BadRequest()
    {
        using (var middleware = WebHostTestFactory.Create(services =>
        {
            services.Configure<UserAgentSentinelOptions>(o => { o.RequireUserAgentHeader = true; });
        }, app =>
               {
                   app.UseUserAgentSentinel();
               }))
        {
            var context = middleware.Host.Services.GetRequiredService<IHttpContextAccessor>().HttpContext;
            var options = middleware.Host.Services.GetRequiredService<IOptions<UserAgentSentinelOptions>>();
            var pipeline = middleware.Application.Build();

            var uae = await Assert.ThrowsAsync<UserAgentException>(async () => await pipeline(context));

            Assert.Equal(uae.Message, options.Value.BadRequestMessage);
            Assert.Equal(uae.StatusCode, StatusCodes.Status400BadRequest);
            Assert.True(options.Value.RequireUserAgentHeader);
            Assert.False(options.Value.ValidateUserAgentHeader);
            Assert.False(options.Value.AllowedUserAgents.Any());
        }
    }

    [Fact]
    public async Task InvokeAsync_ShouldThrowUserAgentException_Forbidden()
    {
        using (var middleware = WebHostTestFactory.Create(services =>
               {
                   services.Configure<UserAgentSentinelOptions>(o =>
                   {
                       o.RequireUserAgentHeader = true;
                       o.ValidateUserAgentHeader = true;
                       o.AllowedUserAgents.Add("Cuemon-Agent");
                   });
               }, app =>
               {
                   app.UseUserAgentSentinel();
               }))
        {
            var context = middleware.Host.Services.GetRequiredService<IHttpContextAccessor>().HttpContext;
            var options = middleware.Host.Services.GetRequiredService<IOptions<UserAgentSentinelOptions>>();
            var pipeline = middleware.Application.Build();

            context.Request.Headers.Add(HeaderNames.UserAgent, "Invalid-Agent");

            var uae = await Assert.ThrowsAsync<UserAgentException>(async () => await pipeline(context));

            Assert.Equal(uae.Message, options.Value.ForbiddenMessage);
            Assert.Equal(uae.StatusCode, StatusCodes.Status403Forbidden);
            Assert.True(options.Value.RequireUserAgentHeader);
            Assert.True(options.Value.ValidateUserAgentHeader);
            Assert.True(options.Value.AllowedUserAgents.Any());
        }
    }

    [Fact]
    public async Task InvokeAsync_ShouldCaptureUserAgentException_Forbidden()
    {
        using (var middleware = WebHostTestFactory.Create(services =>
        {
            services.Configure<UserAgentSentinelOptions>(o =>
            {
                o.RequireUserAgentHeader = true;
                o.ValidateUserAgentHeader = true;
                o.AllowedUserAgents.Add("Cuemon-Agent");
            });
        }, app =>
               {
                   app.UseFaultDescriptorExceptionHandler();
                   app.UseUserAgentSentinel();

               }))
        {
            var context = middleware.Host.Services.GetRequiredService<IHttpContextAccessor>().HttpContext;
            var options = middleware.Host.Services.GetRequiredService<IOptions<UserAgentSentinelOptions>>();
            var pipeline = middleware.Application.Build();

            context.Request.Headers.Add(HeaderNames.UserAgent, "Invalid-Agent");

            await pipeline(context);

            Assert.True(options.Value.RequireUserAgentHeader);
            Assert.True(options.Value.ValidateUserAgentHeader);
            Assert.True(options.Value.AllowedUserAgents.Any());
            Assert.Equal(StatusCodes.Status403Forbidden, context.Response.StatusCode);
            Assert.Contains(options.Value.ForbiddenMessage, context.Response.Body.ToEncodedString());
        }
    }

    [Fact]
    public async Task InvokeAsync_ShouldCaptureUserAgentException_BadRequest()
    {
        using (var middleware = WebHostTestFactory.Create(services =>
               {
                   services.Configure<UserAgentSentinelOptions>(o =>
                   {
                       o.RequireUserAgentHeader = true;
                   });
               }, app =>
               {
                   app.UseFaultDescriptorExceptionHandler();
                   app.UseUserAgentSentinel();

               }))
        {
            var context = middleware.Host.Services.GetRequiredService<IHttpContextAccessor>().HttpContext;
            var options = middleware.Host.Services.GetRequiredService<IOptions<UserAgentSentinelOptions>>();
            var pipeline = middleware.Application.Build();

            await pipeline(context);

            Assert.True(options.Value.RequireUserAgentHeader);
            Assert.False(options.Value.ValidateUserAgentHeader);
            Assert.False(options.Value.AllowedUserAgents.Any());
            Assert.Equal(StatusCodes.Status400BadRequest, context.Response.StatusCode);
            Assert.Contains(options.Value.BadRequestMessage, context.Response.Body.ToEncodedString());
        }
    }

    [Fact]
    public async Task InvokeAsync_ShouldThrowUserAgentException_BadRequest_BecauseOfUseGenericResponse()
    {
        using (var middleware = WebHostTestFactory.Create(services =>
        {
            services.Configure<UserAgentSentinelOptions>(o =>
            {
                o.RequireUserAgentHeader = true;
                o.ValidateUserAgentHeader = true;
                o.UseGenericResponse = true;
                o.AllowedUserAgents.Add("Cuemon-Agent");
            });
        }, app =>
               {
                   app.UseUserAgentSentinel();
               }))
        {
            var context = middleware.Host.Services.GetRequiredService<IHttpContextAccessor>().HttpContext;
            var options = middleware.Host.Services.GetRequiredService<IOptions<UserAgentSentinelOptions>>();
            var pipeline = middleware.Application.Build();

            context.Request.Headers.Add(HeaderNames.UserAgent, "Invalid-Agent");

            var uae = await Assert.ThrowsAsync<UserAgentException>(async () => await pipeline(context));

            Assert.Equal(uae.Message, options.Value.BadRequestMessage);
            Assert.Equal(uae.StatusCode, StatusCodes.Status400BadRequest);

            Assert.True(options.Value.RequireUserAgentHeader);
            Assert.True(options.Value.ValidateUserAgentHeader);
            Assert.True(options.Value.UseGenericResponse);
            Assert.True(options.Value.AllowedUserAgents.Any());
        }
    }

    [Fact]
    public async Task InvokeAsync_ShouldAllowRequestUnconditional()
    {
        using (var middleware = WebHostTestFactory.Create(pipelineSetup: app =>
        {
            app.UseUserAgentSentinel();
            app.Run(context =>
            {
                context.Response.StatusCode = 200;
                return Task.CompletedTask;
            });
        }))
        {
            var context = middleware.Host.Services.GetRequiredService<IHttpContextAccessor>().HttpContext;
            var options = middleware.Host.Services.GetRequiredService<IOptions<UserAgentSentinelOptions>>();
            var pipeline = middleware.Application.Build();

            await pipeline(context);

            Assert.False(options.Value.RequireUserAgentHeader);
            Assert.Equal(StatusCodes.Status200OK, context.Response.StatusCode);
        }
    }

    [Fact]
    public async Task InvokeAsync_ShouldAllowRequestAfterBeingValidated()
    {
        using (var middleware = WebHostTestFactory.Create(services =>
        {
            services.Configure<UserAgentSentinelOptions>(o =>
            {
                o.RequireUserAgentHeader = true;
                o.ValidateUserAgentHeader = true;
                o.AllowedUserAgents.Add("Cuemon-Agent");
            });
        }, app =>
               {
                   app.UseUserAgentSentinel();
                   app.Run(context =>
                   {
                       context.Response.StatusCode = 200;
                       return Task.CompletedTask;
                   });
               }))
        {
            var context = middleware.Host.Services.GetRequiredService<IHttpContextAccessor>().HttpContext;
            var options = middleware.Host.Services.GetRequiredService<IOptions<UserAgentSentinelOptions>>();
            var pipeline = middleware.Application.Build();

            context.Request.Headers.Add(HeaderNames.UserAgent, "Cuemon-Agent");

            await pipeline(context);

            Assert.True(options.Value.RequireUserAgentHeader);
            Assert.True(options.Value.ValidateUserAgentHeader);
            Assert.Equal(StatusCodes.Status200OK, context.Response.StatusCode);
        }
    }
}
