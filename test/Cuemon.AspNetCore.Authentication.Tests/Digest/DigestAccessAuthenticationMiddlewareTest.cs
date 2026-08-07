using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Authentication.Assets;
using Cuemon.AspNetCore.Http;
using Cuemon.Collections.Generic;
using Cuemon.Extensions;
using Cuemon.Extensions.AspNetCore.Authentication;
using Codebelt.Extensions.Xunit;
using Codebelt.Extensions.Xunit.Hosting.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Xunit;

namespace Cuemon.AspNetCore.Authentication.Digest;
public class DigestAccessAuthenticationMiddlewareTest : Test
{
    public DigestAccessAuthenticationMiddlewareTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task InvokeAsync_ShouldNotBeAuthenticated()
    {
        using (var middleware = WebHostTestFactory.Create(services =>
        {
            services.Configure<DigestAuthenticationOptions>(o =>
            {
                o.Authenticator = (string username, out string password) =>
                {
                    if (username == "Agent")
                    {
                        password = "Test";
                        var cp = new ClaimsPrincipal();
                        cp.AddIdentity(new ClaimsIdentity(Arguments.Yield(new Claim("Name", "Test Agent"))));
                        return cp;
                    }
                    password = null;
                    return null;
                };
                o.Realm = "unittest";
                o.RequireSecureConnection = false;
            });
            services.AddFakeHttpContextAccessor(ServiceLifetime.Singleton);
            services.AddInMemoryDigestAuthenticationNonceTracker();
        }, app =>
               {
                   app.UseExceptionMiddleware();
                   app.UseDigestAccessAuthentication();
               }))
        {
            var context = middleware.Host.Services.GetRequiredService<IHttpContextAccessor>().HttpContext;
            var options = middleware.Host.Services.GetRequiredService<IOptions<DigestAuthenticationOptions>>();
            var pipeline = middleware.Application.Build();

            var ue = await Assert.ThrowsAsync<UnauthorizedException>(async () => await pipeline(context));

            Assert.Equal(options.Value.UnauthorizedMessage, ue.Message);
            Assert.Equal(StatusCodes.Status401Unauthorized, ue.StatusCode);

            var wwwAuthenticate = context.Response.Headers[HeaderNames.WWWAuthenticate];

            TestOutput.WriteLine(wwwAuthenticate);
        }
    }

    [Fact]
    public async Task InvokeAsync_ShouldAuthenticateWhenApplyingAuthorizationHeader()
    {
        using (var middleware = WebHostTestFactory.Create(services =>
        {
            services.Configure<DigestAuthenticationOptions>(o =>
            {
                o.Authenticator = (string username, out string password) =>
               {
                   if (username == "Agent")
                   {
                       password = "Test";
                       var cp = new ClaimsPrincipal();
                       cp.AddIdentity(new ClaimsIdentity(Arguments.Yield(new Claim("Name", "Test Agent"))));
                       return cp;
                   }
                   password = null;
                   return null;
               };
                o.Realm = "unittest";
                o.RequireSecureConnection = false;
            });
            services.AddFakeHttpContextAccessor(ServiceLifetime.Singleton);
            services.AddInMemoryDigestAuthenticationNonceTracker();
        }, app =>
               {
                   app.UseDigestAccessAuthentication();
                   app.Run(context =>
                   {
                       context.Response.StatusCode = 200;
                       return Task.CompletedTask;
                   });
               }))
        {
            var context = middleware.Host.Services.GetRequiredService<IHttpContextAccessor>().HttpContext;
            var options = middleware.Host.Services.GetRequiredService<IOptions<DigestAuthenticationOptions>>();
            var pipeline = middleware.Application.Build();

            var ue = await Assert.ThrowsAsync<UnauthorizedException>(async () => await pipeline(context));

            Assert.Equal(options.Value.UnauthorizedMessage, ue.Message);
            Assert.Equal(StatusCodes.Status401Unauthorized, ue.StatusCode);

            var wwwAuthenticate = context.Response.Headers[HeaderNames.WWWAuthenticate];


            TestOutput.WriteLine(wwwAuthenticate);

            var db = new DigestAuthorizationHeaderBuilder(options.Value.DigestAlgorithm)
                .AddRealm(options.Value.Realm)
                .AddUserName("Agent")
                .AddUri("/")
                .AddNc(1)
                .AddCnonce()
                .AddQopAuthentication()
                .AddFromWwwAuthenticateHeader(context.Response.Headers);


            var ha1 = db.ComputeHash1("Test");

            TestOutput.WriteLine(ha1);

            var ha2 = db.ComputeHash2("GET");
            var response = db.ComputeResponse(ha1, ha2);

            db.AddResponse("Test", "GET");

            context.Response.Body = new MemoryStream();
            context.Request.Headers.Add(HeaderNames.Authorization, db.Build().ToString());

            await pipeline(context);

            Assert.Equal(StatusCodes.Status200OK, context.Response.StatusCode);
        }
    }

    [Fact]
    public async Task InvokeAsync_ShouldAuthenticateWhenApplyingAuthorizationHeaderNoPlainTextPassword()
    {
        using (var middleware = WebHostTestFactory.Create(services =>
        {
            services.Configure<DigestAuthenticationOptions>(o =>
            {
                o.UseServerSideHa1Storage = true;
                o.Authenticator = (string username, out string password) =>
               {
                   if (username == "Agent")
                   {
                       password = "a69d6da3eea4fa832dc1c0534863988e550e523f1f786c238951b7ec7abf4d57";
                       var cp = new ClaimsPrincipal();
                       cp.AddIdentity(new ClaimsIdentity(Arguments.Yield(new Claim("Name", "Test Agent"))));
                       return cp;
                   }
                   password = null;
                   return null;
               };
                o.Realm = "unittest";
                o.RequireSecureConnection = false;
            });
            services.AddFakeHttpContextAccessor(ServiceLifetime.Singleton);
            services.AddInMemoryDigestAuthenticationNonceTracker();
        }, app =>
               {
                   app.UseDigestAccessAuthentication();
                   app.Run(context =>
                   {
                       context.Response.StatusCode = 200;
                       return Task.CompletedTask;
                   });
               }))
        {
            var context = middleware.Host.Services.GetRequiredService<IHttpContextAccessor>().HttpContext;
            var options = middleware.Host.Services.GetRequiredService<IOptions<DigestAuthenticationOptions>>();
            var pipeline = middleware.Application.Build();

            var ue = await Assert.ThrowsAsync<UnauthorizedException>(async () => await pipeline(context));

            Assert.Equal(options.Value.UnauthorizedMessage, ue.Message);
            Assert.Equal(StatusCodes.Status401Unauthorized, ue.StatusCode);

            var wwwAuthenticate = context.Response.Headers[HeaderNames.WWWAuthenticate];

            TestOutput.WriteLine(wwwAuthenticate);

            var db = new DigestAuthorizationHeaderBuilder(options.Value.DigestAlgorithm)
                .AddRealm(options.Value.Realm)
                .AddUserName("Agent")
                .AddUri("/")
                .AddNc(1)
                .AddCnonce()
                .AddQopAuthentication()
                .AddFromWwwAuthenticateHeader(context.Response.Headers);


            var ha1 = db.ComputeHash1("Test");

            TestOutput.WriteLine(ha1);

            var ha2 = db.ComputeHash2("GET");
            var response = db.ComputeResponse(ha1, ha2);

            db.AddResponse("Test", "GET");

            context.Response.Body = new MemoryStream();
            context.Request.Headers.Add(HeaderNames.Authorization, db.Build().ToString());

            await pipeline(context);

            Assert.Equal(StatusCodes.Status200OK, context.Response.StatusCode);
        }
    }

    [Fact]
    public async Task InvokeAsync_ShouldAuthenticateWhenApplyingAuthorizationHeaderWithQopIntegrity()
    {
        using (var middleware = WebHostTestFactory.Create(services =>
        {
            services.Configure<DigestAuthenticationOptions>(o =>
            {
                o.Authenticator = (string username, out string password) =>
               {
                   if (username == "Agent")
                   {
                       password = "Test";
                       var cp = new ClaimsPrincipal();
                       cp.AddIdentity(new ClaimsIdentity(Arguments.Yield(new Claim("Name", "Test Agent"))));
                       return cp;
                   }
                   password = null;
                   return null;
               };
                o.Realm = "unittest";
                o.RequireSecureConnection = false;
            });
            services.AddFakeHttpContextAccessor(ServiceLifetime.Singleton);
            services.AddInMemoryDigestAuthenticationNonceTracker();
        }, app =>
               {
                   app.UseDigestAccessAuthentication();
                   app.Run(context =>
                   {
                       context.Response.StatusCode = 200;
                       return Task.CompletedTask;
                   });
               }))
        {
            var context = middleware.Host.Services.GetRequiredService<IHttpContextAccessor>().HttpContext;
            var options = middleware.Host.Services.GetRequiredService<IOptions<DigestAuthenticationOptions>>();
            var pipeline = middleware.Application.Build();

            var ue = await Assert.ThrowsAsync<UnauthorizedException>(async () => await pipeline(context));

            Assert.Equal(options.Value.UnauthorizedMessage, ue.Message);
            Assert.Equal(StatusCodes.Status401Unauthorized, ue.StatusCode);

            var wwwAuthenticate = context.Response.Headers[HeaderNames.WWWAuthenticate];

            TestOutput.WriteLine(wwwAuthenticate);

            var db = new DigestAuthorizationHeaderBuilder(options.Value.DigestAlgorithm)
                .AddRealm(options.Value.Realm)
                .AddUserName("Agent")
                .AddUri("/")
                .AddNc(1)
                .AddCnonce()
                .AddQopAuthenticationIntegrity()
                .AddFromWwwAuthenticateHeader(context.Response.Headers);

            TestOutput.WriteLine("Body:");

            var entityBody = "test of entityBody in request";

            var ha1 = db.ComputeHash1("Test");

            TestOutput.WriteLine("HA1:");
            TestOutput.WriteLine(ha1);

            var ha2 = db.ComputeHash2("POST", entityBody);
            var response = db.ComputeResponse(ha1, ha2);

            TestOutput.WriteLine("HA2:");
            TestOutput.WriteLine(ha2);

            db.AddResponse("Test", "POST", entityBody);

            context.Request.Method = "POST";
            context.Request.Body = new MemoryStream(entityBody.ToByteArray());
            context.Request.Headers.Add(HeaderNames.Authorization, db.Build().ToString());

            await pipeline(context);

            Assert.Equal(StatusCodes.Status200OK, context.Response.StatusCode);
        }
    }
}
