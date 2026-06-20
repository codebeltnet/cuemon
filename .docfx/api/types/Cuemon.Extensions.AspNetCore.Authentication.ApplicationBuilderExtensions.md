---
uid: Cuemon.Extensions.AspNetCore.Authentication.ApplicationBuilderExtensions
example:
- *content
---

The following example demonstrates how to register the Basic, Digest, and HMAC authentication middleware in an ASP.NET Core request pipeline.

```csharp
using System;
using System.Security.Claims;
using Cuemon.Extensions.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace MyApp.Examples;

public static class ApplicationBuilderExtensionsExample
{
    public static void Demonstrate()
    {
        var services = new ServiceCollection();

        services.AddLogging();
        services.AddInMemoryDigestAuthenticationNonceTracker();
        services.AddAuthentication("docs")
            .AddScheme<Cuemon.AspNetCore.Authentication.Basic.BasicAuthenticationOptions, Cuemon.AspNetCore.Authentication.Basic.BasicAuthenticationHandler>(Cuemon.AspNetCore.Authentication.Basic.BasicAuthorizationHeader.Scheme, _ => { })
            .AddScheme<Cuemon.AspNetCore.Authentication.Digest.DigestAuthenticationOptions, Cuemon.AspNetCore.Authentication.Digest.DigestAuthenticationHandler>(Cuemon.AspNetCore.Authentication.Digest.DigestAuthorizationHeader.Scheme, _ => { })
            .AddScheme<Cuemon.AspNetCore.Authentication.Hmac.HmacAuthenticationOptions, Cuemon.AspNetCore.Authentication.Hmac.HmacAuthenticationHandler>("hmac-docs", _ => { });

        var app = new ApplicationBuilder(services.BuildServiceProvider());

        app.UseBasicAuthentication(options =>
        {
            options.Realm = "SecureArea";
            options.Authenticator = (username, password) =>
            {
                if (username == "admin" && password == "secret")
                {
                    return new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) }, "Basic"));
                }

                return null;
            };
            options.RequireSecureConnection = false;
        });

        app.UseDigestAccessAuthentication(options =>
        {
            options.Realm = "SecureArea";
            options.Authenticator = (string username, out string password) =>
            {
                password = "storedPassword";
                return new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) }, "Digest"));
            };
            options.RequireSecureConnection = false;
        });

        app.UseHmacAuthentication(options =>
        {
            options.AuthenticationScheme = "MyHmac";
            options.Authenticator = (string clientId, out string clientSecret) =>
            {
                clientSecret = "storedSecret";
                return new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, clientId) }, "Hmac"));
            };
            options.RequireSecureConnection = false;
        });

        Console.WriteLine(app.GetType().Name);
    }
}

```
