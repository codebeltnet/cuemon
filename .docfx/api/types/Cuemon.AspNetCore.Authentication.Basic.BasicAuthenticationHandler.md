---
uid: Cuemon.AspNetCore.Authentication.Basic.BasicAuthenticationHandler
example:
- *content
---

The following example demonstrates how to register and exercise `BasicAuthenticationHandler` with ASP.NET Core authentication services. It configures a `ServiceCollection` with a custom authenticator callback, invokes `ChallengeAsync` to observe the `WWW-Authenticate` response header, and invokes `AuthenticateAsync` against a `DefaultHttpContext` carrying a Basic `Authorization` header to verify the authenticated principal.

```csharp
// Program.cs
using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Authentication.Basic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using MyApp.Examples;

await BasicAuthenticationHandlerExample.DemonstrateAsync();

namespace MyApp.Examples
{
    public static class BasicAuthenticationHandlerExample
    {
        public static async Task DemonstrateAsync()
        {
            var services = new ServiceCollection();

            services.AddLogging();
            services.AddAuthentication(BasicAuthorizationHeader.Scheme)
                .AddScheme<BasicAuthenticationOptions, BasicAuthenticationHandler>(BasicAuthorizationHeader.Scheme, options =>
                {
                    options.Realm = "docs-example";
                    options.RequireSecureConnection = false;
                    options.Authenticator = (username, password) =>
                    {
                        if (username == "Agent" && password == "Test")
                        {
                            return new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) }, BasicAuthorizationHeader.Scheme));
                        }

                        return null;
                    };
                });

            using var provider = services.BuildServiceProvider();

            var challengeContext = new DefaultHttpContext { RequestServices = provider };
            using (var challengeScope = provider.CreateScope())
            {
                var challengeAuth = challengeScope.ServiceProvider.GetRequiredService<IAuthenticationService>();
                await challengeAuth.ChallengeAsync(challengeContext, BasicAuthorizationHeader.Scheme, null);
            }
            Console.WriteLine($"WWW-Authenticate: {challengeContext.Response.Headers.WWWAuthenticate}");

            var authContext = new DefaultHttpContext { RequestServices = provider };
            var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes("Agent:Test"));
            authContext.Request.Headers[HeaderNames.Authorization] = $"{BasicAuthorizationHeader.Scheme} {credentials}";

            using (var authScope = provider.CreateScope())
            {
                var authService = authScope.ServiceProvider.GetRequiredService<IAuthenticationService>();
                var result = await authService.AuthenticateAsync(authContext, BasicAuthorizationHeader.Scheme);
                Console.WriteLine($"Authenticated: {result.Succeeded}");
                Console.WriteLine($"User: {result.Principal?.Identity?.Name}");
            }
        }
    }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="4ad54e348e2d0fb5d6e025be"><a href="~/run-actions/4ad54e348e2d0fb5d6e025be/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/4ad54e348e2d0fb5d6e025be/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
