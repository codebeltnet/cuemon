---
uid: Cuemon.Extensions.AspNetCore.Authentication.AuthenticationBuilderExtensions
example:
- *content
---

The following example demonstrates how to register the Basic, Digest, and HMAC authentication handlers through the extension methods on <xref cref="Microsoft.AspNetCore.Authentication.AuthenticationBuilder" />. It resolves each configured handler after building the service provider and prints the scheme-specific options that the authentication middleware will use.

```csharp
using System;
using System.Security.Claims;
using Cuemon.AspNetCore.Authentication.Basic;
using Cuemon.AspNetCore.Authentication.Digest;
using Cuemon.AspNetCore.Authentication.Hmac;
using Cuemon.Extensions.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace MyApp.Examples;

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

        var basicHandler = provider.GetRequiredService<BasicAuthenticationHandler>();
        var digestHandler = provider.GetRequiredService<DigestAuthenticationHandler>();
        var hmacHandler = provider.GetRequiredService<HmacAuthenticationHandler>();

        Console.WriteLine($"Basic authentication requires HTTPS: {basicHandler.Options.RequireSecureConnection}");
        Console.WriteLine($"Digest authentication requires HTTPS: {digestHandler.Options.RequireSecureConnection}");
        Console.WriteLine($"HMAC authentication scheme: {hmacHandler.Options.AuthenticationScheme}");
    }
}

```
