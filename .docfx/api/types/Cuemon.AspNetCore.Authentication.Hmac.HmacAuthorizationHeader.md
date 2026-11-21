---
uid: Cuemon.AspNetCore.Authentication.Hmac.HmacAuthorizationHeader
example:
- *content
---

The following example demonstrates how to parse an HMAC Authorization header value.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Authentication.Hmac;

var header = HmacAuthorizationHeader.Create(
    HmacFields.Scheme,
    "HMAC Credential=alice/some-scope, SignedHeaders=date;host, Signature=abc123");
var clientId = header.ClientId; // "alice"
Console.WriteLine(clientId);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="c9b474f26b0501743c71aa79"><a href="~/run-actions/c9b474f26b0501743c71aa79/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/c9b474f26b0501743c71aa79/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
