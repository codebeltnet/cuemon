---
uid: Cuemon.AspNetCore.Authentication.Digest.DigestAuthenticator
example:
- *content
---

The following example demonstrates how to assign and invoke a <see cref="DigestAuthenticator" /> delegate when a digest-auth username resolves to a stored password.

```csharp
// Program.cs
using System;
using System.Security.Claims;
using Cuemon.AspNetCore.Authentication.Digest;

DigestAuthenticator authenticator = (string username, out string password) =>
{
    password = username == "Agent" ? "Test" : null;
    return password == null
        ? null
        : new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) }, DigestAuthorizationHeader.Scheme));
};

var principal = authenticator("Agent", out var password);

Console.WriteLine(password);
Console.WriteLine(principal?.Identity?.Name);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="fa929d9d250ca4468d3b9b8c"><a href="~/run-actions/fa929d9d250ca4468d3b9b8c/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/fa929d9d250ca4468d3b9b8c/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
