---
uid: Cuemon.AspNetCore.Authentication.Hmac.HmacAuthenticator
example:
- *content
---

The following example demonstrates how to assign and invoke a <see cref="HmacAuthenticator" /> delegate for client-id and shared-secret lookup.

```csharp
// Program.cs
using System;
using System.Security.Claims;
using Cuemon.AspNetCore.Authentication.Hmac;

HmacAuthenticator authenticator = (string clientId, out string clientSecret) =>
{
    clientSecret = clientId == "Agent-Api" ? "Test" : null;
    return clientSecret == null
        ? null
        : new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, clientId) }, HmacFields.Scheme));
};

var principal = authenticator("Agent-Api", out var secret);

Console.WriteLine(secret);
Console.WriteLine(principal?.Identity?.Name);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="2329156f7195300efb2c9872"><a href="~/run-actions/2329156f7195300efb2c9872/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/2329156f7195300efb2c9872/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
