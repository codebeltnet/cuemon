---
uid: Cuemon.Security.Cryptography.SecureHashAlgorithm384
example:
- *content
---

The following example demonstrates how to compute a SHA-384 hash with <see cref="SecureHashAlgorithm384" />.

```csharp
// Program.cs
using System;
using System.Text;
using Cuemon.Security.Cryptography;

var algorithm = new SecureHashAlgorithm384();
var result = algorithm.ComputeHash(Encoding.UTF8.GetBytes("Payload for SHA-384"));

Console.WriteLine(SecureHashAlgorithm384.BitSize);
Console.WriteLine(result.GetBytes().Length);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="689097df401157b541058acb"><a href="~/run-actions/689097df401157b541058acb/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/689097df401157b541058acb/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
