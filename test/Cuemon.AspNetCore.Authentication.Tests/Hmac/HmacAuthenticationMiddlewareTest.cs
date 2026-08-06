using System.Security.Claims;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Http;
using Cuemon.Collections.Generic;
using Cuemon.Extensions.AspNetCore.Authentication;
using Codebelt.Extensions.Xunit;
using Codebelt.Extensions.Xunit.Hosting.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Xunit;

namespace Cuemon.AspNetCore.Authentication.Hmac;
public class HmacAuthenticationMiddlewareTest : Test
{
    public HmacAuthenticationMiddlewareTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task InvokeAsync_ShouldAuthenticateWhenApplyingAuthorizationHeader()
    {
        using (var middleware = WebHostTestFactory.Create(services =>
        {
            services.Configure<HmacAuthenticationOptions>(o =>
            {
                o.Authenticator = (string clientId, out string clientSecret) =>
                {
                    clientSecret = null;
                    if (clientId == "Agent-Api")
                    {
                        clientSecret = "Test";
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
                   app.UseHmacAuthentication();
                   app.Run(context =>
                   {
                       context.Response.StatusCode = 200;
                       return Task.CompletedTask;
                   });
               }))
        {
            var context = middleware.Host.Services.GetRequiredService<IHttpContextAccessor>().HttpContext;
            var options = middleware.Host.Services.GetRequiredService<IOptions<HmacAuthenticationOptions>>();
            var pipeline = middleware.Application.Build();

            var ue = await Assert.ThrowsAsync<UnauthorizedException>(async () => await pipeline(context));

            Assert.Equal(options.Value.UnauthorizedMessage, ue.Message);
            Assert.Equal(StatusCodes.Status401Unauthorized, ue.StatusCode);

            var wwwAuthenticate = context.Response.Headers[HeaderNames.WWWAuthenticate];


            TestOutput.WriteLine(wwwAuthenticate);

            context.Request.Host = new HostString("www.cuemon.net");
            context.Request.Headers.Add(HeaderNames.Date, context.Response.Headers[HeaderNames.Date]);

            var hb = new HmacAuthorizationHeaderBuilder()
                .AddFromRequest(context.Request)
                .AddClientId("Agent-Api")
                .AddClientSecret("Test")
                .AddCredentialScope("20150830/us-east-1/iam/aws4_request");

            var hmacHeader = hb.Build();

            TestOutput.WriteLine(hmacHeader.ToString());
            TestOutput.WriteLine("");
            TestOutput.WriteLine("--- HmacAuthorizationHeaderBuilder ---");
            TestOutput.WriteLine(hb.ToString());

            context.Request.Headers.Add(HeaderNames.Authorization, hmacHeader.ToString());

            await pipeline(context);

            Assert.Equal(StatusCodes.Status200OK, context.Response.StatusCode);
        }
    }
}
