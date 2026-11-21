---
uid: Cuemon.Security.Cryptography.HmacSecureHashAlgorithm384
example:
- *content
---

The following example demonstrates how to compute an HMAC-SHA384 hash with <see cref="HmacSecureHashAlgorithm384" />.

```csharp
// Program.cs
using System;
using System.Text;
using Cuemon.Security.Cryptography;

var secret = Encoding.UTF8.GetBytes("docs-secret-sha384");
var algorithm = new HmacSecureHashAlgorithm384(secret, null);
var result = algorithm.ComputeHash(Encoding.UTF8.GetBytes("Payload for SHA-384"));

Console.WriteLine(result.GetBytes().Length);
Console.WriteLine(result.ToBase64String());
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="0583caae9b19a16d6379542e"><a href="~/run-actions/0583caae9b19a16d6379542e/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/0583caae9b19a16d6379542e/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
