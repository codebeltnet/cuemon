---
uid: Cuemon.Security.Cryptography.HmacSecureHashAlgorithm512
example:
- *content
---

The following example demonstrates how different payloads produce different HMAC-SHA512 values with <see cref="HmacSecureHashAlgorithm512" />.

```csharp
// Program.cs
using System;
using System.Text;
using Cuemon.Security.Cryptography;

var secret = Encoding.UTF8.GetBytes("docs-secret-sha512");
var algorithm = new HmacSecureHashAlgorithm512(secret, null);
var first = algorithm.ComputeHash(Encoding.UTF8.GetBytes("first"));
var second = algorithm.ComputeHash(Encoding.UTF8.GetBytes("second"));

Console.WriteLine(first.GetBytes().Length);
Console.WriteLine(!first.Equals(second));
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="db61efb5417ae12c98f5d55d"><a href="~/run-actions/db61efb5417ae12c98f5d55d/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/db61efb5417ae12c98f5d55d/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
