using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Authentication.Assets;
using Cuemon.Collections.Generic;
using Cuemon.Extensions.AspNetCore.Authentication;
using Codebelt.Extensions.Xunit;
using Codebelt.Extensions.Xunit.Hosting;
using Codebelt.Extensions.Xunit.Hosting.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Xunit;

namespace Cuemon.AspNetCore.Authentication.Hmac;
public class HmacAuthenticationHandlerTest : Test
{
    private static readonly string AuthenticationScheme = "hmac-unit-test";

    public HmacAuthenticationHandlerTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task HandleAuthenticateAsync_ShouldReturnContent()
    {
        using (var webApp = WebHostTestFactory.Create(services =>
               {
                   services.AddControllers().AddApplicationPart(typeof(FakeController).Assembly);
                   services
                       .AddAuthentication(AuthenticationScheme)
                       .AddHmac(o =>
                       {
                           o.Authenticator = (string clientId, out string clientSecret) =>
                           {
                               clientSecret = null;
                               if (clientId == "Agent-Api")
                               {
                                   clientSecret = "Test";
                                   var cp = new ClaimsPrincipal();
                                   cp.AddIdentity(new ClaimsIdentity(Arguments.Yield(new Claim("Name", "Test Agent")), AuthenticationScheme));
                                   return cp;
                               }
                               return null;
                           };
                           o.AuthenticationScheme = AuthenticationScheme;
                           o.RequireSecureConnection = false;
                       });
               }, app =>
               {
                   app.UseAuthentication();

                   app.UseRouting();

                   app.UseAuthorization();

                   app.UseEndpoints(routes => { routes.MapControllers(); });
               }))
        {
            var client = webApp.Host.GetTestClient();

            client.DefaultRequestHeaders.Add(HeaderNames.Date, DateTime.UtcNow.ToString("R"));
            client.DefaultRequestHeaders.Add(HeaderNames.Host, "www.cuemon.net");
            client.DefaultRequestHeaders.Add(HeaderNames.Accept, "text/plain");

            var result = await client.GetAsync("/fake?test=unit");

            var wwwAuthenticate = result.Headers.WwwAuthenticate.ToString();

            TestOutput.WriteLine("WWW-Authenticate:");
            TestOutput.WriteLine(wwwAuthenticate);

            var hb = new HmacAuthorizationHeaderBuilder(AuthenticationScheme)
                .AddFromRequest(result.RequestMessage)
                .AddClientId("Agent-Api")
                .AddClientSecret("Test")
                .AddCredentialScope("20150830/us-east-1/iam/aws4_request");

            var token = hb.Build().ToString();

            TestOutput.WriteLine(token);

            TestOutput.WriteLine("");
            TestOutput.WriteLine("--- HmacAuthorizationHeaderBuilder ---");
            TestOutput.WriteLine(hb.ToString());


            client.DefaultRequestHeaders.TryAddWithoutValidation(HeaderNames.Authorization, token);

            result = await client.GetAsync("/fake?test=unit");

            Assert.Equal("Unit Test", await result.Content.ReadAsStringAsync());
            Assert.Equal(StatusCodes.Status200OK, (int)result.StatusCode);
        }
    }

    [Fact]
    public async Task HandleAuthenticateAsync_ShouldReturn401WithUnauthorizedMessage_WhenAuthenticatorIsNotProbablySetup()
    {
        using (var webApp = WebHostTestFactory.Create(services =>
               {
                   services.AddAuthorizationResponseHandler();
                   services.AddControllers().AddApplicationPart(typeof(FakeController).Assembly);
                   services
                       .AddAuthentication(AuthenticationScheme)
                       .AddHmac(o =>
                       {
                           o.Authenticator = (string clientId, out string clientSecret) =>
                           {
                               clientSecret = null;
                               return ClaimsPrincipal.Current;
                           };
                           o.AuthenticationScheme = AuthenticationScheme;
                       });
               }, app =>
               {
                   app.UseAuthentication();

                   app.UseRouting();

                   app.UseAuthorization();

                   app.UseEndpoints(routes => { routes.MapControllers(); });
               }))
        {
            var options = webApp.Host.Services.GetRequiredScopedService<IOptionsSnapshot<HmacAuthenticationOptions>>().Get(AuthenticationScheme);
            var client = webApp.Host.GetTestClient();

            client.DefaultRequestHeaders.Add(HeaderNames.Date, DateTime.UtcNow.ToString("R"));
            client.DefaultRequestHeaders.Add(HeaderNames.Host, "www.cuemon.net");

            var result = await client.GetAsync("/fake");

            Assert.Equal(options.UnauthorizedMessage, await result.Content.ReadAsStringAsync());
            Assert.Equal(StatusCodes.Status401Unauthorized, (int)result.StatusCode);

            var wwwAuthenticate = result.Headers.WwwAuthenticate;

            TestOutput.WriteLine(wwwAuthenticate.ToString());

            var hb = new HmacAuthorizationHeaderBuilder(AuthenticationScheme)
                .AddFromRequest(result.RequestMessage)
                .AddClientId("Agent-Api")
                .AddClientSecret("Test")
                .AddCredentialScope("20150830/us-east-1/iam/aws4_request");

            client.DefaultRequestHeaders.TryAddWithoutValidation(HeaderNames.Authorization, hb.Build().ToString());

            result = await client.GetAsync("/fake");

            Assert.Equal(options.UnauthorizedMessage, await result.Content.ReadAsStringAsync());
            Assert.Equal(StatusCodes.Status401Unauthorized, (int)result.StatusCode);
        }
    }

    [Fact]
    public async Task HandleAuthenticateAsync_EnsureAnonymousIsWorking()
    {
        using (var webApp = WebHostTestFactory.Create(services =>
               {
                   services.AddControllers().AddApplicationPart(typeof(FakeController).Assembly);
                   services
                       .AddAuthentication(AuthenticationScheme)
                       .AddHmac(o =>
                       {
                           o.Authenticator = (string clientId, out string clientSecret) =>
                           {
                               clientSecret = null;
                               return ClaimsPrincipal.Current;
                           };
                       });
               }, app =>
               {
                   app.UseAuthentication();

                   app.UseRouting();

                   app.UseAuthorization();

                   app.UseEndpoints(routes => { routes.MapControllers(); });
               }))
        {
            var client = webApp.Host.GetTestClient();

            var result = await client.GetAsync("/fake/anonymous");

            Assert.Equal("Unit Test", await result.Content.ReadAsStringAsync());
            Assert.Equal(StatusCodes.Status200OK, (int)result.StatusCode);
        }
    }
}
