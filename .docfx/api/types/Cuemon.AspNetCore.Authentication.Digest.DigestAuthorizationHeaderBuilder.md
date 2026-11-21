---
uid: Cuemon.AspNetCore.Authentication.Digest.DigestAuthorizationHeaderBuilder
example:
- *content
---

The following example demonstrates how to build a Digest access authorization header from a challenge header.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Authentication.Digest;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

var headers = new HeaderDictionary
{
    [HeaderNames.WWWAuthenticate] = "Digest realm=\"docs-example\", qop=\"auth, auth-int\", nonce=\"abc123\", opaque=\"opaque456\", stale=false, algorithm=SHA-256"
};

var builder = new DigestAuthorizationHeaderBuilder(DigestCryptoAlgorithm.Sha256)
    .AddRealm("docs-example")
    .AddUserName("Agent")
    .AddUri("/resource")
    .AddNc(1)
    .AddCnonce("client-nonce")
    .AddQopAuthentication()
    .AddFromWwwAuthenticateHeader(headers)
    .AddResponse("Test", "GET");

var header = builder.Build();

Console.WriteLine(header.ToString());
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="983b44896018cf477dd1d6df"><a href="~/run-actions/983b44896018cf477dd1d6df/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/983b44896018cf477dd1d6df/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
