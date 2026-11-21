---
uid: Cuemon.AspNetCore.Authentication.Basic.BasicAuthenticationOptions
example:
- *content
---

The following example demonstrates how to configure `BasicAuthenticationOptions` with a realm and authenticator delegate.

```csharp
// Program.cs
using System;
using System.Security.Claims;
using Cuemon.AspNetCore.Authentication.Basic;

var options = new BasicAuthenticationOptions
{
    Realm = "docs-example",
    RequireSecureConnection = false,
    Authenticator = (username, password) =>
    {
        if (username == "Agent" && password == "Test")
        {
            return new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) }, BasicAuthorizationHeader.Scheme));
        }

        return null;
    }
};

options.ValidateOptions();

Console.WriteLine(options.Realm);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="617841890b44fc101e61a831"><a href="~/run-actions/617841890b44fc101e61a831/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/617841890b44fc101e61a831/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
