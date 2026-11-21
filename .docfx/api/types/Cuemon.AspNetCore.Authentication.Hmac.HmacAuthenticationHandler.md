---
uid: Cuemon.AspNetCore.Authentication.Hmac.HmacAuthenticationHandler
example:
- *content
---

The following example demonstrates how to register `HmacAuthenticationHandler` with ASP.NET Core authentication services. It creates a `ServiceCollection`, registers the handler under a custom HMAC scheme with a client ID/secret authenticator callback, and builds the service provider. The handler is resolved from DI and its configured scheme is written to the console, confirming that HMAC authentication configuration works end-to-end.

```csharp
// Program.cs
using System;
using System.Security.Claims;
using Cuemon.AspNetCore.Authentication.Hmac;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MyApp.Examples;

HmacAuthenticationHandlerExample.Demonstrate();

namespace MyApp.Examples
{
    public static class HmacAuthenticationHandlerExample
    {
        public static void Demonstrate()
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
                        clientSecret = clientId == "Agent-Api" ? "Test" : null;
                        return clientSecret == null
                            ? null
                            : new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, clientId) }, authenticationScheme));
                    };
                });

            using var provider = services.BuildServiceProvider();
            var options = provider.GetRequiredService<IOptionsMonitor<HmacAuthenticationOptions>>().Get(authenticationScheme);

            Console.WriteLine($"Authentication scheme: {options.AuthenticationScheme}");
        }
    }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="338531210f0e333b32ed4f22"><a href="~/run-actions/338531210f0e333b32ed4f22/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/338531210f0e333b32ed4f22/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
