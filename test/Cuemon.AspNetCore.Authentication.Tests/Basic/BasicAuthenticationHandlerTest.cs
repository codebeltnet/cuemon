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

namespace Cuemon.AspNetCore.Authentication.Basic;
public class BasicAuthenticationHandlerTest : Test
{
    public BasicAuthenticationHandlerTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task HandleAuthenticateAsync_ShouldReturnContent()
    {
        using (var webApp = WebHostTestFactory.Create(services =>
               {
                   services.AddControllers().AddApplicationPart(typeof(FakeController).Assembly);
                   services
                       .AddAuthentication(BasicAuthorizationHeader.Scheme)
                       .AddBasic(o =>
                       {
                           o.Authenticator = (username, password) =>
                           {
                               if (username == "Agent" && password == "Test")
                               {
                                   var cp = new ClaimsPrincipal();
                                   cp.AddIdentity(new ClaimsIdentity(Arguments.Yield(new Claim("Name", "Test Agent")), BasicAuthorizationHeader.Scheme));
                                   return cp;
                               }
                               return null;
                           };
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
            var bb = new BasicAuthorizationHeaderBuilder()
                .AddUserName("Agent")
                .AddPassword("Test");

            client.DefaultRequestHeaders.Add(HeaderNames.Authorization, bb.Build().ToString());

            var result = await client.GetAsync("/fake");

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
                       .AddAuthentication(BasicAuthorizationHeader.Scheme)
                       .AddBasic(o =>
                       {
                           o.Authenticator = (username, password) => ClaimsPrincipal.Current;
                       });
               }, app =>
               {
                   app.UseAuthentication();

                   app.UseRouting();

                   app.UseAuthorization();

                   app.UseEndpoints(routes => { routes.MapControllers(); });
               }))
        {
            var options = webApp.Host.Services.GetRequiredScopedService<IOptionsSnapshot<BasicAuthenticationOptions>>().Get(BasicAuthorizationHeader.Scheme);
            var client = webApp.Host.GetTestClient();

            var result = await client.GetAsync("/fake");

            Assert.Equal(options.UnauthorizedMessage, await result.Content.ReadAsStringAsync());
            Assert.Equal(StatusCodes.Status401Unauthorized, (int)result.StatusCode);

            var wwwAuthenticate = result.Headers.WwwAuthenticate;

            TestOutput.WriteLine(wwwAuthenticate.ToString());

            var bb = new BasicAuthorizationHeaderBuilder()
                .AddUserName("Agent")
                .AddPassword("Test");

            client.DefaultRequestHeaders.Add(HeaderNames.Authorization, bb.Build().ToString());

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
                       .AddAuthentication(BasicAuthorizationHeader.Scheme)
                       .AddBasic(o =>
                       {
                           o.Authenticator = (username, password) => ClaimsPrincipal.Current;
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
