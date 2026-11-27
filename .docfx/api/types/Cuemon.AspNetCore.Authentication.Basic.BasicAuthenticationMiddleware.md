---
uid: Cuemon.AspNetCore.Authentication.Basic.BasicAuthenticationMiddleware
example:
- *content
---

The following example demonstrates how to construct `BasicAuthenticationMiddleware` with inline option setup and invoke it against a `DefaultHttpContext` carrying a Basic `Authorization` header.

```csharp
// Program.cs
using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Authentication.Basic;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

var middleware = new BasicAuthenticationMiddleware(_ => Task.CompletedTask, options =>
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

var context = new DefaultHttpContext();
var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes("Agent:Test"));
context.Request.Headers[HeaderNames.Authorization] = $"{BasicAuthorizationHeader.Scheme} {credentials}";

await middleware.InvokeAsync(context);

Console.WriteLine($"Realm: {middleware.Options.Realm}");
Console.WriteLine($"Authenticated: {context.User.Identity?.IsAuthenticated}");
Console.WriteLine($"User: {context.User.Identity?.Name}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="a9392514ecb9f01142339915"><a href="~/run-actions/a9392514ecb9f01142339915/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/a9392514ecb9f01142339915/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
