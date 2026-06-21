---
uid: Cuemon.AspNetCore.Authentication.Authenticator
example:
- *content
---

The following example demonstrates how to authenticate an HTTP request by parsing the `Authorization` header and resolving a claims principal.

```csharp
using System;
using System.Security.Claims;
using Cuemon;
using Cuemon.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace MyApp.Examples;

public static class AuthenticatorExample
{
    public static void Demonstrate()
    {
        var context = new DefaultHttpContext()
        {
            Request = { IsHttps = true }
        };
        context.Request.Headers.Append(HeaderNames.Authorization, "Basic YWxpY2U6cGFzc3dvcmQ=");

        var result = Authenticator.Authenticate<string>(context, false,
            (HttpContext _, string authorizationHeader) => authorizationHeader,
            (HttpContext _, string credentials, out ConditionalValue<ClaimsPrincipal> principal) =>
            {
                principal = new SuccessfulValue<ClaimsPrincipal>(
                    new ClaimsPrincipal(new ClaimsIdentity(
                        new[] { new Claim(ClaimTypes.Name, "alice") }, "Basic")));
                return true;
            });

        Console.WriteLine("Succeeded: " + result.Succeeded);
        Console.WriteLine("User: " + result.Result?.Identity?.Name);
    }
}

```
