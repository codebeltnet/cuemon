---
uid: Cuemon.AspNetCore.Authentication.Hmac.HmacAuthenticationOptions
example:
- *content
---

The following example demonstrates how to configure `HmacAuthenticationOptions` for HMAC request signing.

```csharp
// Program.cs
using System;
using System.Security.Claims;
using Cuemon.AspNetCore.Authentication.Hmac;
using Cuemon.Security.Cryptography;

var options = new HmacAuthenticationOptions
{
    AuthenticationScheme = "hmac-docs",
    Algorithm = KeyedCryptoAlgorithm.HmacSha256,
    RequireSecureConnection = false,
    Authenticator = (string clientId, out string clientSecret) =>
    {
        clientSecret = clientId == "Agent-Api" ? "Test" : null;
        return clientSecret == null
            ? null
            : new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, clientId) }, "hmac-docs"));
    }
};

options.ValidateOptions();

Console.WriteLine(options.AuthenticationScheme);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="667b2f2a616981c05ac90547"><a href="~/run-actions/667b2f2a616981c05ac90547/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/667b2f2a616981c05ac90547/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
