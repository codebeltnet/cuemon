using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Authentication.Assets;
using Cuemon.Collections.Generic;
using Cuemon.Extensions.AspNetCore.Authentication;
using Codebelt.Extensions.Xunit;
using Codebelt.Extensions.Xunit.Hosting;
using Codebelt.Extensions.Xunit.Hosting.AspNetCore;
using Cuemon.Security.Cryptography;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Xunit;

namespace Cuemon.AspNetCore.Authentication.Digest;
public class DigestAccessAuthenticationHandlerTest : Test
{
    public DigestAccessAuthenticationHandlerTest(ITestOutputHelper output) : base(output)
    {
    }

    [Theory]
    [InlineData(DigestCryptoAlgorithm.Sha256)]
    [InlineData(DigestCryptoAlgorithm.Sha256Session)]
    [InlineData(DigestCryptoAlgorithm.Sha512Slash256)]
    [InlineData(DigestCryptoAlgorithm.Sha512Slash256Session)]
    [InlineData(DigestCryptoAlgorithm.Md5)]
    [InlineData(DigestCryptoAlgorithm.Md5Session)]
    public async Task HandleAuthenticateAsync_ShouldReturnContent_WithQopAuthentication(DigestCryptoAlgorithm algorithm)
    {
        using (var webApp = WebHostTestFactory.Create(services =>
               {
                   services.AddInMemoryDigestAuthenticationNonceTracker();
                   services.AddControllers().AddApplicationPart(typeof(FakeController).Assembly);
                   services
                       .AddAuthentication(DigestAuthorizationHeader.Scheme)
                       .AddDigestAccess(o =>
                       {
                           o.Authenticator = (string username, out string password) =>
                           {
                               if (username == "Agent")
                               {
                                   password = "Test";
                                   var cp = new ClaimsPrincipal();
                                   cp.AddIdentity(new ClaimsIdentity(Arguments.Yield(new Claim("Name", "Test Agent")), DigestAuthorizationHeader.Scheme));
                                   return cp;
                               }
                               password = null;
                               return null;
                           };
                           o.RequireSecureConnection = false;
                           o.DigestAlgorithm = algorithm;
                       });
               }, app =>
               {
                   app.UseAuthentication();

                   app.UseRouting();

                   app.UseAuthorization();

                   app.UseEndpoints(routes => { routes.MapControllers(); });
               }))
        {

            var options = webApp.Host.Services.GetRequiredScopedService<IOptionsSnapshot<DigestAuthenticationOptions>>().Get(DigestAuthorizationHeader.Scheme);
            var client = webApp.Host.GetTestClient();

            var result = await client.GetAsync("/fake");

            var wwwAuthenticate = result.Headers.WwwAuthenticate.ToString();

            TestOutput.WriteLine("WWW-Authenticate:");
            TestOutput.WriteLine(wwwAuthenticate);

            var db = new DigestAuthorizationHeaderBuilder(options.DigestAlgorithm)
                .AddRealm(options.Realm)
                .AddUserName("Agent")
                .AddUri("/")
                .AddNc(1)
                .AddCnonce()
                .AddQopAuthentication()
                .AddFromWwwAuthenticateHeader(result.Headers);

            var ha1 = db.ComputeHash1("Test");
            var ha2 = db.ComputeHash2("GET");

            db.ComputeResponse(ha1, ha2);
            db.AddResponse("Test", "GET");

            var token = db.Build().ToString();

            TestOutput.WriteLine("Token:");
            TestOutput.WriteLine(token);

            client.DefaultRequestHeaders.Add(HeaderNames.Authorization, token);

            result = await client.GetAsync("/fake");

            Assert.Equal("Unit Test", await result.Content.ReadAsStringAsync());
            Assert.Equal(StatusCodes.Status200OK, (int)result.StatusCode);
        }
    }

    [Theory]
    [InlineData(DigestCryptoAlgorithm.Sha256)]
    [InlineData(DigestCryptoAlgorithm.Sha256Session)]
    [InlineData(DigestCryptoAlgorithm.Sha512Slash256)]
    [InlineData(DigestCryptoAlgorithm.Sha512Slash256Session)]
    [InlineData(DigestCryptoAlgorithm.Md5)]
    [InlineData(DigestCryptoAlgorithm.Md5Session)]
    public async Task HandleAuthenticateAsync_ShouldReturnContent_QopAuthenticationIntegrity(DigestCryptoAlgorithm algorithm)
    {
        using (var webApp = WebHostTestFactory.Create(services =>
               {
                   services.AddInMemoryDigestAuthenticationNonceTracker();
                   services.AddControllers().AddApplicationPart(typeof(FakeController).Assembly);
                   services
                       .AddAuthentication(DigestAuthorizationHeader.Scheme)
                       .AddDigestAccess(o =>
                       {
                           o.Authenticator = (string username, out string password) =>
                           {
                               if (username == "Agent")
                               {
                                   password = "Test";
                                   var cp = new ClaimsPrincipal();
                                   cp.AddIdentity(new ClaimsIdentity(Arguments.Yield(new Claim("Name", "Test Agent")), DigestAuthorizationHeader.Scheme));
                                   return cp;
                               }
                               password = null;
                               return null;
                           };
                           o.RequireSecureConnection = false;
                           o.DigestAlgorithm = algorithm;
                       });
               }, app =>
               {
                   app.UseAuthentication();

                   app.UseRouting();

                   app.UseAuthorization();

                   app.UseEndpoints(routes => { routes.MapControllers(); });
               }))
        {

            var options = webApp.Host.Services.GetRequiredScopedService<IOptionsSnapshot<DigestAuthenticationOptions>>().Get(DigestAuthorizationHeader.Scheme);
            var client = webApp.Host.GetTestClient();

            var result = await client.GetAsync("/fake");

            var wwwAuthenticate = result.Headers.WwwAuthenticate.ToString();

            TestOutput.WriteLine("WWW-Authenticate:");
            TestOutput.WriteLine(wwwAuthenticate);

            var db = new DigestAuthorizationHeaderBuilder(options.DigestAlgorithm)
                .AddRealm(options.Realm)
                .AddUserName("Agent")
                .AddUri("/")
                .AddNc(1)
                .AddCnonce()
                .AddQopAuthenticationIntegrity()
                .AddFromWwwAuthenticateHeader(result.Headers);

            var entityBody = "test of entityBody in request";

            var ha1 = db.ComputeHash1("Test");
            var ha2 = db.ComputeHash2("POST", entityBody);
            db.ComputeResponse(ha1, ha2);

            db.AddResponse("Test", "POST", entityBody);

            var token = db.Build().ToString();

            TestOutput.WriteLine("Token:");
            TestOutput.WriteLine(token);

            client.DefaultRequestHeaders.Add(HeaderNames.Authorization, token);
            result = await client.PostAsync("/fake", new StringContent(entityBody));

            Assert.Equal("Unit Test", await result.Content.ReadAsStringAsync());
            Assert.Equal(StatusCodes.Status200OK, (int)result.StatusCode);
        }
    }

    [Fact]
    public async Task HandleAuthenticateAsync_ShouldReturnContent_WithServerSideHa1Storage()
    {
        using (var webApp = WebHostTestFactory.Create(services =>
               {
                   services.AddInMemoryDigestAuthenticationNonceTracker();
                   services.AddControllers().AddApplicationPart(typeof(FakeController).Assembly);
                   services
                       .AddAuthentication(DigestAuthorizationHeader.Scheme)
                       .AddDigestAccess(o =>
                       {
                           o.DigestAlgorithm = DigestCryptoAlgorithm.Sha512Slash256;
                           o.UseServerSideHa1Storage = true;
                           o.Authenticator = (string username, out string password) =>
                           {
                               if (username == "Agent")
                               {
                                   password = "7a0adced41ceeaf77c95a4bb382a80303536fd3ee166a3a67a2dc9c100a9d7be";
                                   var cp = new ClaimsPrincipal();
                                   cp.AddIdentity(new ClaimsIdentity(Arguments.Yield(new Claim("Name", "Test Agent")), DigestAuthorizationHeader.Scheme));
                                   return cp;
                               }
                               password = null;
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

            var options = webApp.Host.Services.GetRequiredScopedService<IOptionsSnapshot<DigestAuthenticationOptions>>().Get(DigestAuthorizationHeader.Scheme);
            var client = webApp.Host.GetTestClient();

            var result = await client.GetAsync("/fake");

            var wwwAuthenticate = result.Headers.WwwAuthenticate.ToString();

            TestOutput.WriteLine("WWW-Authenticate:");
            TestOutput.WriteLine(wwwAuthenticate);

            var db = new DigestAuthorizationHeaderBuilder(options.DigestAlgorithm)
                .AddRealm(options.Realm)
                .AddUserName("Agent")
                .AddUri("/")
                .AddNc(1)
                .AddCnonce()
                .AddQopAuthentication()
                .AddFromWwwAuthenticateHeader(result.Headers);

            var ha1 = db.ComputeHash1("Test");
            var ha2 = db.ComputeHash2("GET");
            db.ComputeResponse(ha1, ha2);

            TestOutput.WriteLine(ha1);

            db.AddResponse("Test", "GET");

            var token = db.Build().ToString();

            TestOutput.WriteLine("Token:");
            TestOutput.WriteLine(token);

            client.DefaultRequestHeaders.Add(HeaderNames.Authorization, token);

            result = await client.GetAsync("/fake");

            Assert.Equal(DigestCryptoAlgorithm.Sha512Slash256, options.DigestAlgorithm);
            Assert.True(options.UseServerSideHa1Storage);
            Assert.False(options.RequireSecureConnection);

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
                   services.AddInMemoryDigestAuthenticationNonceTracker();
                   services.AddControllers().AddApplicationPart(typeof(FakeController).Assembly);
                   services
                       .AddAuthentication(DigestAuthorizationHeader.Scheme)
                       .AddDigestAccess(o =>
                       {
                           o.Authenticator = (string username, out string password) =>
                           {
                               password = "";
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
            var options = webApp.Host.Services.GetRequiredScopedService<IOptionsSnapshot<DigestAuthenticationOptions>>().Get(DigestAuthorizationHeader.Scheme);
            var client = webApp.Host.GetTestClient();

            var result = await client.GetAsync("/fake");

            Assert.Equal(options.UnauthorizedMessage, await result.Content.ReadAsStringAsync());
            Assert.Equal(StatusCodes.Status401Unauthorized, (int)result.StatusCode);

            var wwwAuthenticate = result.Headers.WwwAuthenticate;

            TestOutput.WriteLine(wwwAuthenticate.ToString());

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
                       .AddAuthentication(DigestAuthorizationHeader.Scheme)
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
