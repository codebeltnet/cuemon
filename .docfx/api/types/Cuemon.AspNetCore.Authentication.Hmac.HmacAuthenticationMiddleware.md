---
uid: Cuemon.AspNetCore.Authentication.Hmac.HmacAuthenticationMiddleware
example:
- *content
---

The following example demonstrates how to construct `HmacAuthenticationMiddleware` with inline option setup and invoke it against a signed `DefaultHttpContext` request.

```csharp
// Program.cs
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Authentication.Hmac;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using MyApp.Examples;

await HmacAuthenticationMiddlewareExample.DemonstrateAsync();

namespace MyApp.Examples
{
    public static class HmacAuthenticationMiddlewareExample
    {
        public static async Task DemonstrateAsync()
        {
            const string authenticationScheme = "hmac-docs";
            var middleware = new HmacAuthenticationMiddleware(_ => Task.CompletedTask, options =>
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

            var context = new DefaultHttpContext();
            context.Request.Method = HttpMethods.Get;
            context.Request.Scheme = "https";
            context.Request.Host = new HostString("www.cuemon.net");
            context.Request.Path = "/orders";
            context.Request.Headers[HeaderNames.Date] = DateTime.UtcNow.ToString("R");

            var token = new HmacAuthorizationHeaderBuilder(authenticationScheme)
                .AddFromRequest(context.Request)
                .AddClientId("Agent-Api")
                .AddClientSecret("Test")
                .AddCredentialScope("20150830/us-east-1/iam/aws4_request")
                .Build()
                .ToString();

            context.Request.Headers[HeaderNames.Authorization] = token;

            await middleware.InvokeAsync(context);

            Console.WriteLine($"Scheme: {middleware.Options.AuthenticationScheme}");
            Console.WriteLine($"Authenticated: {context.User.Identity?.IsAuthenticated}");
            Console.WriteLine($"Client: {context.User.Identity?.Name}");
        }
    }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="f13faa7ea45b6bcad2085a15"><a href="~/run-actions/f13faa7ea45b6bcad2085a15/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/f13faa7ea45b6bcad2085a15/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
