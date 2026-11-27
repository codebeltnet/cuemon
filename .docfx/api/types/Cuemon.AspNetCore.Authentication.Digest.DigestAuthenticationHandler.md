---
uid: Cuemon.AspNetCore.Authentication.Digest.DigestAuthenticationHandler
example:
- *content
---

The following example demonstrates how to register and exercise `DigestAuthenticationHandler` with ASP.NET Core authentication services. It configures DI with `INonceTracker`, invokes `ChallengeAsync` to obtain the `WWW-Authenticate` digest challenge, builds a matching `Digest` authorization header, and invokes `AuthenticateAsync` to verify the authenticated principal.

```csharp
// Program.cs
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Authentication;
using Cuemon.AspNetCore.Authentication.Digest;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using MyApp.Examples;

await DigestAuthenticationHandlerExample.DemonstrateAsync();

namespace MyApp.Examples
{
    public static class DigestAuthenticationHandlerExample
    {
        public static async Task DemonstrateAsync()
        {
            var services = new ServiceCollection();

            services.AddLogging();
            services.AddSingleton<INonceTracker, MemoryNonceTracker>();
            services.AddAuthentication(DigestAuthorizationHeader.Scheme)
                .AddScheme<DigestAuthenticationOptions, DigestAuthenticationHandler>(DigestAuthorizationHeader.Scheme, options =>
                {
                    options.Realm = "docs-example";
                    options.RequireSecureConnection = false;
                    options.Authenticator = (string username, out string password) =>
                    {
                        password = username == "Agent" ? "Test" : string.Empty;
                        return username == "Agent"
                            ? new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) }, DigestAuthorizationHeader.Scheme))
                            : null;
                    };
                });

            using var provider = services.BuildServiceProvider();
            var options = provider.GetRequiredService<IOptionsMonitor<DigestAuthenticationOptions>>().Get(DigestAuthorizationHeader.Scheme);

            var challengeContext = new DefaultHttpContext { RequestServices = provider };
            using (var challengeScope = provider.CreateScope())
            {
                var challengeAuth = challengeScope.ServiceProvider.GetRequiredService<IAuthenticationService>();
                await challengeAuth.ChallengeAsync(challengeContext, DigestAuthorizationHeader.Scheme, null);
            }

            var headerBuilder = new DigestAuthorizationHeaderBuilder(options.DigestAlgorithm)
                .AddRealm(options.Realm)
                .AddUserName("Agent")
                .AddUri("/")
                .AddNc(1)
                .AddCnonce()
                .AddQopAuthentication()
                .AddFromWwwAuthenticateHeader(challengeContext.Response.Headers);
            headerBuilder.AddResponse("Test", HttpMethods.Get);

            var authContext = new DefaultHttpContext { RequestServices = provider };
            authContext.Request.Method = HttpMethods.Get;
            authContext.Request.Path = "/";
            authContext.Request.Headers[HeaderNames.Authorization] = headerBuilder.Build().ToString();

            using (var authScope = provider.CreateScope())
            {
                var authService = authScope.ServiceProvider.GetRequiredService<IAuthenticationService>();
                var result = await authService.AuthenticateAsync(authContext, DigestAuthorizationHeader.Scheme);
                Console.WriteLine($"Authentication realm: {options.Realm}");
                Console.WriteLine($"Authenticated: {result.Succeeded}");
                Console.WriteLine($"User: {result.Principal?.Identity?.Name}");
            }
        }
    }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="e448262b2e3284dee2997572"><a href="~/run-actions/e448262b2e3284dee2997572/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/e448262b2e3284dee2997572/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
