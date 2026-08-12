using System.Linq;
using System.Security.Claims;
using Cuemon.AspNetCore.Authentication.Basic;
using Cuemon.AspNetCore.Authentication.Digest;
using Cuemon.AspNetCore.Authentication.Hmac;
using Cuemon.Extensions.Reflection;
using Codebelt.Extensions.Xunit;
using Codebelt.Extensions.Xunit.Hosting.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Cuemon.Extensions.AspNetCore.Authentication;
public class ApplicationBuilderExtensionsTest : Test
{
    public ApplicationBuilderExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void UseBasicAuthentication_ShouldAddBasicAuthenticationMiddlewareAndBasicAuthenticationOptions_ToHost()
    {
        using (var host = WebHostTestFactory.Create(pipelineSetup: app =>
               {
                   app.UseBasicAuthentication(o =>
                   {
                       o.Authenticator = (username, password) => ClaimsPrincipal.Current;
                       o.RequireSecureConnection = false;
                   });
               }))
        {
            var options = host.Host.Services.GetRequiredService<IOptions<BasicAuthenticationOptions>>();
            var middleware = host.Application.Build();

            Assert.NotNull(options);
            Assert.NotNull(middleware);
            Assert.IsType<BasicAuthenticationOptions>(options.Value);
            Assert.IsType<BasicAuthenticationMiddleware>(middleware.Target);
        }
    }

    [Fact]
    public void UseDigestAuthentication_ShouldAddDigestAuthenticationMiddlewareAndDigestAuthenticationOptions_ToHost()
    {
        using (var host = WebHostTestFactory.Create(pipelineSetup: app =>
               {
                   app.UseDigestAccessAuthentication(o =>
                   {
                       o.Authenticator = (string username, out string password) =>
                       {
                           password = null;
                           return ClaimsPrincipal.Current;
                       };
                       o.RequireSecureConnection = false;
                   });
               }))
        {
            var options = host.Host.Services.GetRequiredService<IOptions<DigestAuthenticationOptions>>();
            var middleware = host.Application.Build();

            Assert.NotNull(options);
            Assert.NotNull(middleware);
            Assert.IsType<DigestAuthenticationOptions>(options.Value);
            Assert.IsType<DigestAuthenticationMiddleware>(middleware.Target!.GetType().GetAllFields().Single(fi => fi.Name == "instance").GetValue(middleware.Target));
        }
    }

    [Fact]
    public void UseHmacAuthentication_ShouldAddHmacAuthenticationMiddlewareAndHmacAuthenticationOptions_ToHost()
    {
        using (var host = WebHostTestFactory.Create(pipelineSetup: app =>
               {
                   app.UseHmacAuthentication(o =>
                   {
                       o.Authenticator = (string clientId, out string clientSecret) =>
                       {
                           clientSecret = null;
                           return ClaimsPrincipal.Current;
                       };
                       o.RequireSecureConnection = false;
                   });
               }))
        {
            var options = host.Host.Services.GetRequiredService<IOptions<HmacAuthenticationOptions>>();
            var middleware = host.Application.Build();

            Assert.NotNull(options);
            Assert.NotNull(middleware);
            Assert.IsType<HmacAuthenticationOptions>(options.Value);
            Assert.IsType<HmacAuthenticationMiddleware>(middleware.Target);
        }
    }
}
