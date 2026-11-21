---
uid: Cuemon.AspNetCore.Authentication.Digest.DigestHashFactory
example:
- *content
---

The following example demonstrates how to create a cryptographic hash instance using `DigestHashFactory` with a specified algorithm.

```csharp
// Program.cs
using System;
using System.Text;
using Cuemon.AspNetCore.Authentication.Digest;
using Cuemon.Security;

Hash hash = DigestHashFactory.CreateCrypto(DigestCryptoAlgorithm.Sha256);

var bytes = Encoding.UTF8.GetBytes("hello-world");
var result = hash.ComputeHash(bytes);

Console.WriteLine(result.ToHexadecimalString());
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="2341a0177c75331dffc7002e"><a href="~/run-actions/2341a0177c75331dffc7002e/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/2341a0177c75331dffc7002e/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
