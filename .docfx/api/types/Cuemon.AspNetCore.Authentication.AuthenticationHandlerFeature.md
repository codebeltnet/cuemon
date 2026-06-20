---
uid: Cuemon.AspNetCore.Authentication.AuthenticationHandlerFeature
example:
- *content
---

The following example demonstrates how to keep the authenticate result and user principal synchronized on the current HTTP features.

```csharp
using System;
using System.Security.Claims;
using Cuemon.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features.Authentication;

namespace MyApp.Examples;

public static class AuthenticationHandlerFeatureExample
{
    public static void Demonstrate()
    {
        var principal = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, "Agent") }, "Basic"));
        var result = AuthenticateResult.Success(new AuthenticationTicket(principal, "Basic"));
        var context = new DefaultHttpContext();

        AuthenticationHandlerFeature.Set(result, context);

        var authenticateFeature = (AuthenticationHandlerFeature)context.Features.Get<IAuthenticateResultFeature>()!;
        var httpAuthenticationFeature = (AuthenticationHandlerFeature)context.Features.Get<IHttpAuthenticationFeature>()!;

        Console.WriteLine(authenticateFeature.AuthenticateResult == result);
        Console.WriteLine(httpAuthenticationFeature.User.Identity?.Name);
    }
}

```
