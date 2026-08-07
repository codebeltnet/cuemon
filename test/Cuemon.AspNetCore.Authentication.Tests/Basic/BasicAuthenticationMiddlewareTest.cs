using System.Security.Claims;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Http;
using Cuemon.Collections.Generic;
using Cuemon.Extensions.AspNetCore.Authentication;
using Cuemon.Extensions.AspNetCore.Diagnostics;
using Cuemon.Extensions.IO;
using Codebelt.Extensions.Xunit;
using Codebelt.Extensions.Xunit.Hosting.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Xunit;

namespace Cuemon.AspNetCore.Authentication.Basic;
public class BasicAuthenticationMiddlewareTest : Test
{
    public BasicAuthenticationMiddlewareTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task InvokeAsync_ShouldThrowUnauthorizedException_InvalidCredentials()
    {
        using (var middleware = WebHostTestFactory.Create(services =>
        {
            services.Configure<BasicAuthenticationOptions>(o =>
            {
                o.Authenticator = (username, password) => null;
                o.RequireSecureConnection = false;
            });
        }, app =>
               {
                   app.UseBasicAuthentication();
               }))
        {
            var context = middleware.Host.Services.GetRequiredService<IHttpContextAccessor>().HttpContext;
            var options = middleware.Host.Services.GetRequiredService<IOptions<BasicAuthenticationOptions>>();
            var pipeline = middleware.Application.Build();

            var ue = await Assert.ThrowsAsync<UnauthorizedException>(async () => await pipeline(context));

            Assert.Equal(options.Value.UnauthorizedMessage, ue.Message);
            Assert.Equal(StatusCodes.Status401Unauthorized, ue.StatusCode);

            var wwwAuthenticate = context.Response.Headers[HeaderNames.WWWAuthenticate];

            TestOutput.WriteLine(wwwAuthenticate);

            var bb = new BasicAuthorizationHeaderBuilder()
                .AddUserName("Agent")
                .AddPassword("Test");

            context.Request.Headers.Add(HeaderNames.Authorization, bb.Build().ToString());

            ue = await Assert.ThrowsAsync<UnauthorizedException>(async () => await pipeline(context));

            Assert.Equal(options.Value.UnauthorizedMessage, ue.Message);
            Assert.Equal(StatusCodes.Status401Unauthorized, ue.StatusCode);
        }
    }

    [Fact]
    public async Task InvokeAsync_ShouldCaptureUnauthorizedException_InvalidCredentials()
    {
        using (var middleware = WebHostTestFactory.Create(services =>
               {
                   services.Configure<BasicAuthenticationOptions>(o =>
                   {
                       o.Authenticator = (username, password) => null;
                       o.RequireSecureConnection = false;
                   });
               }, app =>
               {
                   app.UseFaultDescriptorExceptionHandler();
                   app.UseBasicAuthentication();
               }))
        {
            var context = middleware.Host.Services.GetRequiredService<IHttpContextAccessor>().HttpContext;
            var options = middleware.Host.Services.GetRequiredService<IOptions<BasicAuthenticationOptions>>();
            var pipeline = middleware.Application.Build();

            var bb = new BasicAuthorizationHeaderBuilder()
                .AddUserName("Agent")
                .AddPassword("Test");

            context.Request.Headers.Add(HeaderNames.Authorization, bb.Build().ToString());

            await pipeline(context);

            TestOutput.WriteLine(context.Response.Body.ToEncodedString(o => o.LeaveOpen = true));

            Assert.EndsWith(options.Value.UnauthorizedMessage, context.Response.Body.ToEncodedString().Trim()); // TODO: make sure text/plain does not have trailing linefeed
            Assert.Equal(StatusCodes.Status401Unauthorized, context.Response.StatusCode);

            var wwwAuthenticate = context.Response.Headers[HeaderNames.WWWAuthenticate];

            TestOutput.WriteLine(wwwAuthenticate);
        }
    }

    [Fact]
    public async Task InvokeAsync_ShouldAuthenticateWhenApplyingAuthorizationHeader()
    {
        using (var middleware = WebHostTestFactory.Create(services =>
        {
            services.Configure<BasicAuthenticationOptions>(o =>
            {
                o.Authenticator = (username, password) =>
                {
                    if (username == "Agent" && password == "Test")
                    {
                        var cp = new ClaimsPrincipal();
                        cp.AddIdentity(new ClaimsIdentity(Arguments.Yield(new Claim("Name", "Test Agent"))));
                        return cp;
                    }
                    return null;
                };
                o.RequireSecureConnection = false;
            });
        }, app =>
               {
                   app.UseBasicAuthentication();
                   app.Run(context =>
                   {
                       context.Response.StatusCode = 200;
                       return Task.CompletedTask;
                   });
               }))
        {
            var context = middleware.Host.Services.GetRequiredService<IHttpContextAccessor>().HttpContext;
            var options = middleware.Host.Services.GetRequiredService<IOptions<BasicAuthenticationOptions>>();
            var pipeline = middleware.Application.Build();

            var ue = await Assert.ThrowsAsync<UnauthorizedException>(async () => await pipeline(context));

            Assert.Equal(options.Value.UnauthorizedMessage, ue.Message);
            Assert.Equal(StatusCodes.Status401Unauthorized, ue.StatusCode);

            var wwwAuthenticate = context.Response.Headers[HeaderNames.WWWAuthenticate];

            TestOutput.WriteLine(wwwAuthenticate);

            var bb = new BasicAuthorizationHeaderBuilder()
                .AddUserName("Agent")
                .AddPassword("Test");

            context.Request.Headers.Add(HeaderNames.Authorization, bb.Build().ToString());

            await pipeline(context);

            Assert.Equal(StatusCodes.Status200OK, context.Response.StatusCode);
        }
    }
}
