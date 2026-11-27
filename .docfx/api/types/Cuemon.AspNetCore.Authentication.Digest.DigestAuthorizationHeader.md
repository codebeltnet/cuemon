---
uid: Cuemon.AspNetCore.Authentication.Digest.DigestAuthorizationHeader
example:
- *content
---

The following example demonstrates how to serialize and parse a Digest access authentication header.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Authentication.Digest;

var header = new DigestAuthorizationHeader(
    realm: "docs-example",
    nonce: "abc123",
    opaque: "opaque456",
    algorithm: "SHA-256",
    userName: "Agent",
    uri: "/resource",
    nc: "00000001",
    cNonce: "client-nonce",
    qop: "auth",
    response: "deadbeef");

var parsed = DigestAuthorizationHeader.Create(header.ToString());

Console.WriteLine(parsed.UserName);
Console.WriteLine(parsed.Response);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="1927379364aef60e8d1ccb00"><a href="~/run-actions/1927379364aef60e8d1ccb00/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/1927379364aef60e8d1ccb00/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
