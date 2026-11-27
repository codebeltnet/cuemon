---
uid: Cuemon.AspNetCore.Authentication.Digest.DigestAuthenticationOptions
example:
- *content
---

The following example demonstrates how to configure `DigestAuthenticationOptions` with a realm, digest algorithm, and authenticator delegate.

```csharp
// Program.cs
using System;
using System.Security.Claims;
using Cuemon.AspNetCore.Authentication.Digest;

var options = new DigestAuthenticationOptions
{
    DigestAlgorithm = DigestCryptoAlgorithm.Sha256,
    Realm = "docs-example",
    RequireSecureConnection = false,
    Authenticator = (string username, out string password) =>
    {
        password = username == "Agent" ? "Test" : null;
        return password == null
            ? null
            : new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) }, DigestAuthorizationHeader.Scheme));
    }
};

options.ValidateOptions();

Console.WriteLine(options.DigestAlgorithm);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="023ab0ec6b67d9348ca5655e"><a href="~/run-actions/023ab0ec6b67d9348ca5655e/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/023ab0ec6b67d9348ca5655e/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
