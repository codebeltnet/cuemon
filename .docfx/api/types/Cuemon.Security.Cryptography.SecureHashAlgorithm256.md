---
uid: Cuemon.Security.Cryptography.SecureHashAlgorithm256
example:
- *content
---

The following example demonstrates how to compute a SHA-256 hash with <see cref="SecureHashAlgorithm256" />.

```csharp
// Program.cs
using System;
using System.Text;
using Cuemon.Security.Cryptography;

var algorithm = new SecureHashAlgorithm256();
var result = algorithm.ComputeHash(Encoding.UTF8.GetBytes("The quick brown fox jumps over the lazy dog"));
var repeated = algorithm.ComputeHash(Encoding.UTF8.GetBytes("The quick brown fox jumps over the lazy dog"));

Console.WriteLine(SecureHashAlgorithm256.BitSize);
Console.WriteLine(result.GetBytes().Length);
Console.WriteLine(result.ToHexadecimalString() == repeated.ToHexadecimalString());
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="4cef14dfa1743b3c84bca42b"><a href="~/run-actions/4cef14dfa1743b3c84bca42b/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/4cef14dfa1743b3c84bca42b/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
