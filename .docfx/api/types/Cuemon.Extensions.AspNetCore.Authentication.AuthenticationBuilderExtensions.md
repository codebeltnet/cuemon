---
uid: Cuemon.Extensions.AspNetCore.Authentication.AuthenticationBuilderExtensions
example:
- *content
---

The following example demonstrates how to register the Basic, Digest, and HMAC authentication handlers through the extension methods on <xref cref="Microsoft.AspNetCore.Authentication.AuthenticationBuilder" />. It resolves each configured handler after building the service provider and prints the scheme-specific options that the authentication middleware will use.

```csharp
// Program.cs
using System;
using System.Security.Claims;
using Cuemon.AspNetCore.Authentication.Basic;
using Cuemon.AspNetCore.Authentication.Digest;
using Cuemon.AspNetCore.Authentication.Hmac;
using Cuemon.Extensions.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MyApp.Examples;

AuthenticationBuilderExtensionsExample.Demonstrate();

namespace MyApp.Examples
{
    public static class AuthenticationBuilderExtensionsExample
    {
        public static void Demonstrate()
        {
            var services = new ServiceCollection();

            services.AddLogging();
            services.AddInMemoryDigestAuthenticationNonceTracker();
            services.AddAuthentication(BasicAuthorizationHeader.Scheme)
                .AddBasic(o =>
                {
                    o.Authenticator = (username, password) => new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) }, BasicAuthorizationHeader.Scheme));
                    o.RequireSecureConnection = false;
                })
                .AddDigestAccess(o =>
                {
                    o.Authenticator = (string username, out string password) =>
                    {
                        password = "Test";
                        return new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) }, DigestAuthorizationHeader.Scheme));
                    };
                    o.RequireSecureConnection = false;
                })
                .AddHmac(o =>
                {
                    o.AuthenticationScheme = "hmac-docs";
                    o.Authenticator = (string clientId, out string clientSecret) =>
                    {
                        clientSecret = "Test";
                        return new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, clientId) }, o.AuthenticationScheme));
                    };
                    o.RequireSecureConnection = false;
                });

            using var provider = services.BuildServiceProvider();

            var basicOptions = provider.GetRequiredService<IOptionsMonitor<BasicAuthenticationOptions>>().Get(BasicAuthorizationHeader.Scheme);
            var digestOptions = provider.GetRequiredService<IOptionsMonitor<DigestAuthenticationOptions>>().Get(DigestAuthorizationHeader.Scheme);
            var hmacOptions = provider.GetRequiredService<IOptionsMonitor<HmacAuthenticationOptions>>().Get("hmac-docs");

            Console.WriteLine($"Basic authentication requires HTTPS: {basicOptions.RequireSecureConnection}");
            Console.WriteLine($"Digest authentication requires HTTPS: {digestOptions.RequireSecureConnection}");
            Console.WriteLine($"HMAC authentication scheme: {hmacOptions.AuthenticationScheme}");
        }
    }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="6faa4e81d231e4605cdd95e8"><a href="~/run-actions/6faa4e81d231e4605cdd95e8/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/6faa4e81d231e4605cdd95e8/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
