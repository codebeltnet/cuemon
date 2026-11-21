---
uid: Cuemon.Security.Cryptography.SecureHashAlgorithm512256
example:
- *content
---

The following example demonstrates how to compute a SHA-512/256 hash with <see cref="SecureHashAlgorithm512256" />.

```csharp
// Program.cs
using System;
using System.Text;
using Cuemon.Security.Cryptography;

var algorithm = new SecureHashAlgorithm512256(null);
var result = algorithm.ComputeHash(Encoding.UTF8.GetBytes("Payload for SHA-512/256"));

Console.WriteLine(SecureHashAlgorithm512256.BitSize);
Console.WriteLine(result.GetBytes().Length);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="120cfd2c5cc87d738cc55954"><a href="~/run-actions/120cfd2c5cc87d738cc55954/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/120cfd2c5cc87d738cc55954/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
