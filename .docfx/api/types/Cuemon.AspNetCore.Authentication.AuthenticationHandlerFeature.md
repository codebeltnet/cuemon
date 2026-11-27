---
uid: Cuemon.AspNetCore.Authentication.AuthenticationHandlerFeature
example:
- *content
---

The following example demonstrates how to keep the authenticate result and user principal synchronized on the current HTTP features.

```csharp
// Program.cs
using System;
using System.Security.Claims;
using Cuemon.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features.Authentication;

var principal = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, "Agent") }, "Basic"));
var result = AuthenticateResult.Success(new AuthenticationTicket(principal, "Basic"));
var context = new DefaultHttpContext();

AuthenticationHandlerFeature.Set(result, context);

var authenticateFeature = (AuthenticationHandlerFeature)context.Features.Get<IAuthenticateResultFeature>()!;
var httpAuthenticationFeature = (AuthenticationHandlerFeature)context.Features.Get<IHttpAuthenticationFeature>()!;

Console.WriteLine(authenticateFeature.AuthenticateResult == result);
Console.WriteLine(httpAuthenticationFeature.User.Identity?.Name);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="67d4fc15a7a4ec439a4f68cf"><a href="~/run-actions/67d4fc15a7a4ec439a4f68cf/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/67d4fc15a7a4ec439a4f68cf/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
