---
uid: Cuemon.Extensions.Net.Security.SignedUriOptions
example:
- *content
---

The following example demonstrates how to configure `SignedUriOptions` for generating time-limited signed URIs with HMAC-SHA256.

```csharp
// Program.cs
using System;
using Cuemon.Extensions.Net.Security;
using Cuemon.Security.Cryptography;

var options = new SignedUriOptions
{
    Algorithm = KeyedCryptoAlgorithm.HmacSha256,
    SignatureFieldName = "sig",
    StartFieldName = "st",
    ExpiryFieldName = "se",
    UrlEncode = true
};

Console.WriteLine(options.Algorithm);
Console.WriteLine(options.SignatureFieldName);
Console.WriteLine(options.UrlEncode);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="e42d326ddb523935687d1402"><a href="~/run-actions/e42d326ddb523935687d1402/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/e42d326ddb523935687d1402/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
