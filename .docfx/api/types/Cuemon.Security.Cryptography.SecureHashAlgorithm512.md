---
uid: Cuemon.Security.Cryptography.SecureHashAlgorithm512
example:
- *content
---

The following example demonstrates how to compute a SHA-512 hash with <see cref="SecureHashAlgorithm512" />.

```csharp
// Program.cs
using System;
using System.Text;
using Cuemon.Security.Cryptography;

var algorithm = new SecureHashAlgorithm512();
var result = algorithm.ComputeHash(Encoding.UTF8.GetBytes("Payload for SHA-512"));

Console.WriteLine(SecureHashAlgorithm512.BitSize);
Console.WriteLine(result.ToBase64String());
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="3b469bb8d03f3c851d439824"><a href="~/run-actions/3b469bb8d03f3c851d439824/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/3b469bb8d03f3c851d439824/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
