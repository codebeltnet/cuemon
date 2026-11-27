---
uid: Cuemon.Extensions.AspNetCore.Authentication.ApplicationBuilderExtensions
example:
- *content
---

The following example demonstrates how to register and execute the Basic, Digest, and HMAC authentication middleware in an ASP.NET Core request pipeline. Each extension configures a different credential scheme, returns the same application builder for fluent chaining, and challenges or authenticates incoming HTTP requests when the pipeline is built and invoked.

```csharp
// Program.cs
using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Cuemon.Extensions.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

services.AddLogging();
services.AddInMemoryDigestAuthenticationNonceTracker();
services.AddAuthentication("docs")
    .AddScheme<Cuemon.AspNetCore.Authentication.Basic.BasicAuthenticationOptions, Cuemon.AspNetCore.Authentication.Basic.BasicAuthenticationHandler>(Cuemon.AspNetCore.Authentication.Basic.BasicAuthorizationHeader.Scheme, _ => { })
    .AddScheme<Cuemon.AspNetCore.Authentication.Digest.DigestAuthenticationOptions, Cuemon.AspNetCore.Authentication.Digest.DigestAuthenticationHandler>(Cuemon.AspNetCore.Authentication.Digest.DigestAuthorizationHeader.Scheme, _ => { })
    .AddScheme<Cuemon.AspNetCore.Authentication.Hmac.HmacAuthenticationOptions, Cuemon.AspNetCore.Authentication.Hmac.HmacAuthenticationHandler>("hmac-docs", _ => { });

using var provider = services.BuildServiceProvider();
var app = new ApplicationBuilder(provider);

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

app.Run(context =>
{
    context.Response.StatusCode = StatusCodes.Status200OK;
    return Task.CompletedTask;
});

var pipeline = app.Build();

var unauthenticated = new DefaultHttpContext { RequestServices = provider };
try
{
    await pipeline(unauthenticated);
}
catch (Cuemon.AspNetCore.Http.UnauthorizedException ex)
{
    Console.WriteLine($"Unauthenticated request rejected: {(int)ex.StatusCode} ({ex.Message})");
}

var authenticated = new DefaultHttpContext { RequestServices = provider };
authenticated.Request.Headers.Authorization = $"Basic {Convert.ToBase64String(Encoding.UTF8.GetBytes("admin:secret"))}";
try
{
    await pipeline(authenticated);
}
catch (Cuemon.AspNetCore.Http.UnauthorizedException ex)
{
    Console.WriteLine($"Subsequent middleware rejected non-Digest header: {(int)ex.StatusCode}");
}

Console.WriteLine($"Basic middleware returned the application builder: {ReferenceEquals(app, basicBuilder)}");
Console.WriteLine($"Digest middleware returned the application builder: {ReferenceEquals(app, digestBuilder)}");
Console.WriteLine($"HMAC middleware returned the application builder: {ReferenceEquals(app, hmacBuilder)}");
Console.WriteLine($"Authenticated user: {authenticated.User.Identity?.Name} (isAuthenticated: {authenticated.User.Identity?.IsAuthenticated})");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="428de6f5c714ccf79fe90c5c"><a href="~/run-actions/428de6f5c714ccf79fe90c5c/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/428de6f5c714ccf79fe90c5c/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
