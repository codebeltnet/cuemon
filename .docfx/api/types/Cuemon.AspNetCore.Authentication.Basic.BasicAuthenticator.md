---
uid: Cuemon.AspNetCore.Authentication.Basic.BasicAuthenticator
example:
- *content
---

The following example demonstrates how to assign and invoke a <see cref="BasicAuthenticator" /> delegate when validating a basic-auth username and password pair.

```csharp
// Program.cs
using System;
using System.Security.Claims;
using Cuemon.AspNetCore.Authentication.Basic;

BasicAuthenticator authenticator = (username, password) =>
{
    return username == "Agent" && password == "Test"
        ? new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) }, BasicAuthorizationHeader.Scheme))
        : null;
};

var principal = authenticator("Agent", "Test");

Console.WriteLine(principal?.Identity?.Name);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="2ccde57a5295495b513a2f7f"><a href="~/run-actions/2ccde57a5295495b513a2f7f/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/2ccde57a5295495b513a2f7f/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
