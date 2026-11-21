---
uid: Cuemon.Extensions.AspNetCore.Authentication.ApplicationBuilderExtensions
example:
- *content
---

The following example demonstrates how to register the Basic, Digest, and HMAC authentication middleware in an ASP.NET Core request pipeline. Each extension configures a different credential scheme and returns the same application builder so additional middleware can be appended. The final checks make that fluent-pipeline contract visible without depending on an implementation type name.

```csharp
// Program.cs
using System;
using System.Security.Claims;
using Cuemon.Extensions.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

services.AddLogging();
services.AddInMemoryDigestAuthenticationNonceTracker();
services.AddAuthentication("docs")
    .AddScheme<Cuemon.AspNetCore.Authentication.Basic.BasicAuthenticationOptions, Cuemon.AspNetCore.Authentication.Basic.BasicAuthenticationHandler>(Cuemon.AspNetCore.Authentication.Basic.BasicAuthorizationHeader.Scheme, _ => { })
    .AddScheme<Cuemon.AspNetCore.Authentication.Digest.DigestAuthenticationOptions, Cuemon.AspNetCore.Authentication.Digest.DigestAuthenticationHandler>(Cuemon.AspNetCore.Authentication.Digest.DigestAuthorizationHeader.Scheme, _ => { })
    .AddScheme<Cuemon.AspNetCore.Authentication.Hmac.HmacAuthenticationOptions, Cuemon.AspNetCore.Authentication.Hmac.HmacAuthenticationHandler>("hmac-docs", _ => { });

var app = new ApplicationBuilder(services.BuildServiceProvider());

var basicBuilder = app.UseBasicAuthentication(options =>
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

var digestBuilder = app.UseDigestAccessAuthentication(options =>
{
    options.Realm = "SecureArea";
    options.Authenticator = (string username, out string password) =>
    {
        password = "storedPassword";
        return new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) }, "Digest"));
    };
    options.RequireSecureConnection = false;
});

var hmacBuilder = app.UseHmacAuthentication(options =>
{
    options.AuthenticationScheme = "MyHmac";
    options.Authenticator = (string clientId, out string clientSecret) =>
    {
        clientSecret = "storedSecret";
        return new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, clientId) }, "Hmac"));
    };
    options.RequireSecureConnection = false;
});

Console.WriteLine($"Basic middleware returned the application builder: {ReferenceEquals(app, basicBuilder)}");
Console.WriteLine($"Digest middleware returned the application builder: {ReferenceEquals(app, digestBuilder)}");
Console.WriteLine($"HMAC middleware returned the application builder: {ReferenceEquals(app, hmacBuilder)}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="47d39a9a485701d47a367386"><a href="~/run-actions/47d39a9a485701d47a367386/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/47d39a9a485701d47a367386/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
