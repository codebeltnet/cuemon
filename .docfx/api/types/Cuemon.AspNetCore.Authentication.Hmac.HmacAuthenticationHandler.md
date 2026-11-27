---
uid: Cuemon.AspNetCore.Authentication.Hmac.HmacAuthenticationHandler
example:
- *content
---

The following example demonstrates how to register and exercise `HmacAuthenticationHandler` with ASP.NET Core authentication services. It registers the handler under a custom HMAC scheme, invokes `ChallengeAsync` to observe the `WWW-Authenticate` header, signs a `DefaultHttpContext` request with `HmacAuthorizationHeaderBuilder`, and invokes `AuthenticateAsync` to verify the authenticated principal.

```csharp
// Program.cs
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Authentication.Hmac;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using MyApp.Examples;

await HmacAuthenticationHandlerExample.DemonstrateAsync();

namespace MyApp.Examples
{
    public static class HmacAuthenticationHandlerExample
    {
        public static async Task DemonstrateAsync()
        {
            const string authenticationScheme = "hmac-docs";
            var services = new ServiceCollection();

            services.AddLogging();
            services.AddAuthentication(authenticationScheme)
                .AddScheme<HmacAuthenticationOptions, HmacAuthenticationHandler>(authenticationScheme, options =>
                {
                    options.AuthenticationScheme = authenticationScheme;
                    options.RequireSecureConnection = false;
                    options.Authenticator = (string clientId, out string clientSecret) =>
                    {
                        clientSecret = clientId == "Agent-Api" ? "Test" : string.Empty;
                        return clientId == "Agent-Api"
                            ? new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, clientId) }, authenticationScheme))
                            : null;
                    };
                });

            using var provider = services.BuildServiceProvider();

            var challengeContext = new DefaultHttpContext { RequestServices = provider };
            using (var challengeScope = provider.CreateScope())
            {
                var challengeAuth = challengeScope.ServiceProvider.GetRequiredService<IAuthenticationService>();
                await challengeAuth.ChallengeAsync(challengeContext, authenticationScheme, null);
            }
            Console.WriteLine($"WWW-Authenticate: {challengeContext.Response.Headers.WWWAuthenticate}");

            var authContext = new DefaultHttpContext { RequestServices = provider };
            authContext.Request.Method = HttpMethods.Get;
            authContext.Request.Scheme = "https";
            authContext.Request.Host = new HostString("www.cuemon.net");
            authContext.Request.Path = "/orders";
            authContext.Request.Headers[HeaderNames.Date] = DateTime.UtcNow.ToString("R");

            var token = new HmacAuthorizationHeaderBuilder(authenticationScheme)
                .AddFromRequest(authContext.Request)
                .AddClientId("Agent-Api")
                .AddClientSecret("Test")
                .AddCredentialScope("20150830/us-east-1/iam/aws4_request")
                .Build()
                .ToString();

            authContext.Request.Headers[HeaderNames.Authorization] = token;

            using (var authScope = provider.CreateScope())
            {
                var authService = authScope.ServiceProvider.GetRequiredService<IAuthenticationService>();
                var result = await authService.AuthenticateAsync(authContext, authenticationScheme);
                Console.WriteLine($"Authenticated: {result.Succeeded}");
                Console.WriteLine($"Client: {result.Principal?.Identity?.Name}");
            }
        }
    }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="eb442a69d4b162d3db44ca60"><a href="~/run-actions/eb442a69d4b162d3db44ca60/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/eb442a69d4b162d3db44ca60/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
