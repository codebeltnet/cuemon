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

            var basicHandler = provider.GetRequiredService<BasicAuthenticationHandler>();
            var digestHandler = provider.GetRequiredService<DigestAuthenticationHandler>();
            var hmacHandler = provider.GetRequiredService<HmacAuthenticationHandler>();

            Console.WriteLine($"Basic handler: {basicHandler.GetType().Name}, requires HTTPS: {basicOptions.RequireSecureConnection}");
            Console.WriteLine($"Digest handler: {digestHandler.GetType().Name}, requires HTTPS: {digestOptions.RequireSecureConnection}");
            Console.WriteLine($"HMAC handler: {hmacHandler.GetType().Name}, scheme: {hmacOptions.AuthenticationScheme}");
        }
    }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="9dc776983832a642ac7ce5ca"><a href="~/run-actions/9dc776983832a642ac7ce5ca/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/9dc776983832a642ac7ce5ca/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
