---
uid: Cuemon.AspNetCore.Authentication.Digest.DigestAuthenticationHandler
example:
- *content
---

The following example demonstrates how to register `DigestAuthenticationHandler` with ASP.NET Core authentication services. It configures DI with `INonceTracker`, registers a digest authentication scheme with a username/password lookup callback, and builds the service provider. The handler is then resolved from DI and its configured realm is written to the console, verifying that digest authentication wiring is operational.

```csharp
// Program.cs
using System;
using System.Security.Claims;
using Cuemon.AspNetCore.Authentication;
using Cuemon.AspNetCore.Authentication.Digest;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MyApp.Examples;

DigestAuthenticationHandlerExample.Demonstrate();

namespace MyApp.Examples
{
    public static class DigestAuthenticationHandlerExample
    {
        public static void Demonstrate()
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
                        password = username == "Agent" ? "Test" : null;
                        return password == null
                            ? null
                            : new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) }, DigestAuthorizationHeader.Scheme));
                    };
                });

            using var provider = services.BuildServiceProvider();
            var options = provider.GetRequiredService<IOptionsMonitor<DigestAuthenticationOptions>>().Get(DigestAuthorizationHeader.Scheme);

            Console.WriteLine($"Authentication realm: {options.Realm}");
        }
    }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="1acf06e643f58130b6e7337b"><a href="~/run-actions/1acf06e643f58130b6e7337b/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/1acf06e643f58130b6e7337b/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
