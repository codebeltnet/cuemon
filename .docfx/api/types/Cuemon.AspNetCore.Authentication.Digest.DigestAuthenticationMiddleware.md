---
uid: Cuemon.AspNetCore.Authentication.Digest.DigestAuthenticationMiddleware
example:
- *content
---

The following example demonstrates how to construct `DigestAuthenticationMiddleware` with inline option setup.

```csharp
// Program.cs
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Authentication.Digest;
using MyApp.Examples;

DigestAuthenticationMiddlewareExample.Demonstrate();

namespace MyApp.Examples
{
    public static class DigestAuthenticationMiddlewareExample
    {
        public static void Demonstrate()
        {
            var middleware = new DigestAuthenticationMiddleware(_ => Task.CompletedTask, options =>
            {
                options.Realm = "docs-example";
                options.RequireSecureConnection = false;
                options.Authenticator = (string username, out string password) =>
                {
                    password = username == "Agent" ? "Test" : null;
                    return password == null ? null : new ClaimsPrincipal(new ClaimsIdentity());
                };
            });

            Console.WriteLine(middleware.Options.Realm);
        }
    }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="1ed4b7c31349df56241cefcb"><a href="~/run-actions/1ed4b7c31349df56241cefcb/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/1ed4b7c31349df56241cefcb/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
