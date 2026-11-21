---
uid: Cuemon.AspNetCore.Authentication.Hmac.HmacAuthenticationMiddleware
example:
- *content
---

The following example demonstrates how to construct `HmacAuthenticationMiddleware` with inline option setup.

```csharp
// Program.cs
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Authentication.Hmac;
using MyApp.Examples;

HmacAuthenticationMiddlewareExample.Demonstrate();

namespace MyApp.Examples
{
    public static class HmacAuthenticationMiddlewareExample
    {
        public static void Demonstrate()
        {
            var middleware = new HmacAuthenticationMiddleware(_ => Task.CompletedTask, options =>
            {
                options.AuthenticationScheme = "hmac-docs";
                options.RequireSecureConnection = false;
                options.Authenticator = (string clientId, out string clientSecret) =>
                {
                    clientSecret = clientId == "Agent-Api" ? "Test" : null;
                    return clientSecret == null ? null : new ClaimsPrincipal(new ClaimsIdentity());
                };
            });

            Console.WriteLine(middleware.Options.AuthenticationScheme);
        }
    }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="adb6a986c371daad49cc0500"><a href="~/run-actions/adb6a986c371daad49cc0500/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/adb6a986c371daad49cc0500/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
