---
uid: Cuemon.AspNetCore.Authentication.Basic.BasicAuthenticationHandler
example:
- *content
---

The following example demonstrates how to register `BasicAuthenticationHandler` with ASP.NET Core authentication services. It sets up a `ServiceCollection`, registers the handler with a custom authenticator callback that validates credentials against hardcoded values, and builds the service provider. The handler is then resolved from DI and its configured realm is written to the console, confirming the authentication pipeline wires up correctly.

```csharp
// Program.cs
using System;
using System.Security.Claims;
using Cuemon.AspNetCore.Authentication.Basic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MyApp.Examples;

BasicAuthenticationHandlerExample.Demonstrate();

namespace MyApp.Examples
{
    public static class BasicAuthenticationHandlerExample
    {
        public static void Demonstrate()
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
            var options = provider.GetRequiredService<IOptionsMonitor<BasicAuthenticationOptions>>().Get(BasicAuthorizationHeader.Scheme);

            Console.WriteLine($"Authentication realm: {options.Realm}");
        }
    }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="5acec4474b4e95e2fbcef7c9"><a href="~/run-actions/5acec4474b4e95e2fbcef7c9/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/5acec4474b4e95e2fbcef7c9/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
