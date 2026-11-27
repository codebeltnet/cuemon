---
uid: Cuemon.AspNetCore.Authentication.Digest.DigestAuthenticationMiddleware
example:
- *content
---

The following example demonstrates how to construct `DigestAuthenticationMiddleware` with inline option setup and invoke it against a `DefaultHttpContext` using `InvokeAsync`.

```csharp
// Program.cs
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Authentication;
using Cuemon.AspNetCore.Authentication.Digest;
using Cuemon.AspNetCore.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using MyApp.Examples;

await DigestAuthenticationMiddlewareExample.DemonstrateAsync();

namespace MyApp.Examples
{
    public static class DigestAuthenticationMiddlewareExample
    {
        public static async Task DemonstrateAsync()
        {
            var nonceTracker = new MemoryNonceTracker();
            var middleware = new DigestAuthenticationMiddleware(_ => Task.CompletedTask, options =>
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

            var challengeContext = new DefaultHttpContext();
            try
            {
                await middleware.InvokeAsync(challengeContext, nonceTracker);
            }
            catch (UnauthorizedException)
            {
                // Expected on the initial unauthenticated request; WWW-Authenticate is now populated.
            }

            var headerBuilder = new DigestAuthorizationHeaderBuilder(middleware.Options.DigestAlgorithm)
                .AddRealm(middleware.Options.Realm)
                .AddUserName("Agent")
                .AddUri("/")
                .AddNc(1)
                .AddCnonce()
                .AddQopAuthentication()
                .AddFromWwwAuthenticateHeader(challengeContext.Response.Headers);
            headerBuilder.AddResponse("Test", HttpMethods.Get);

            var authContext = new DefaultHttpContext();
            authContext.Request.Method = HttpMethods.Get;
            authContext.Request.Path = "/";
            authContext.Request.Headers[HeaderNames.Authorization] = headerBuilder.Build().ToString();

            await middleware.InvokeAsync(authContext, nonceTracker);

            Console.WriteLine($"Realm: {middleware.Options.Realm}");
            Console.WriteLine($"Authenticated: {authContext.User.Identity?.IsAuthenticated}");
            Console.WriteLine($"User: {authContext.User.Identity?.Name}");
        }
    }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="15ff1d72ee8ee36578f2b35b"><a href="~/run-actions/15ff1d72ee8ee36578f2b35b/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/15ff1d72ee8ee36578f2b35b/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
