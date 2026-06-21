---
uid: Cuemon.Extensions.AspNetCore.Authentication.AuthenticationBuilderExtensions
example:
- *content
---

The following example demonstrates how to register the Basic, Digest, and HMAC authentication handlers through the extension methods on <xref cref="Microsoft.AspNetCore.Authentication.AuthenticationBuilder" />.

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

        Console.WriteLine(provider.GetRequiredService<BasicAuthenticationHandler>().GetType().Name);
        Console.WriteLine(provider.GetRequiredService<DigestAuthenticationHandler>().GetType().Name);
        Console.WriteLine(provider.GetRequiredService<HmacAuthenticationHandler>().GetType().Name);
    }
}

```
