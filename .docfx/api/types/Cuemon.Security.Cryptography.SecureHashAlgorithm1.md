---
uid: Cuemon.Security.Cryptography.SecureHashAlgorithm1
example:
- *content
---

The following example demonstrates how to compute a SHA-1 hash with <see cref="SecureHashAlgorithm1" />.

```csharp
// Program.cs
using System;
using System.Text;
using Cuemon.Security.Cryptography;

var algorithm = new SecureHashAlgorithm1();
var result = algorithm.ComputeHash(Encoding.UTF8.GetBytes("legacy-compatible digest"));

Console.WriteLine(SecureHashAlgorithm1.BitSize);
Console.WriteLine(result.GetBytes().Length);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="eefd8a3b81a7571d1a5d9fde"><a href="~/run-actions/eefd8a3b81a7571d1a5d9fde/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/eefd8a3b81a7571d1a5d9fde/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
