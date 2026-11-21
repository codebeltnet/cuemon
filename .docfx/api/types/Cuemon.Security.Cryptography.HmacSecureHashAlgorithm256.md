---
uid: Cuemon.Security.Cryptography.HmacSecureHashAlgorithm256
example:
- *content
---

The following example demonstrates how to compute an HMAC-SHA256 hash with <see cref="HmacSecureHashAlgorithm256" />.

```csharp
// Program.cs
using System;
using System.Text;
using Cuemon.Security.Cryptography;

var secret = Encoding.UTF8.GetBytes("unittest-secret");
var payload = Encoding.UTF8.GetBytes("Authenticate this message");
var algorithm = new HmacSecureHashAlgorithm256(secret, null);
var result = algorithm.ComputeHash(payload);

Console.WriteLine(result.GetBytes().Length);
Console.WriteLine(result.ToHexadecimalString());
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="cd04881a86de27f98218d100"><a href="~/run-actions/cd04881a86de27f98218d100/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/cd04881a86de27f98218d100/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
