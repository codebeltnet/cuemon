---
uid: Cuemon.AspNetCore.Authentication.Digest.DigestCryptoAlgorithm
example:
- *content
---

The following example demonstrates how `DigestCryptoAlgorithm` selects the hash algorithm used by `DigestAuthorizationHeaderBuilder`.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Authentication.Digest;

var builder = new DigestAuthorizationHeaderBuilder(DigestCryptoAlgorithm.Sha512Slash256);

Console.WriteLine(builder.DigestAlgorithm);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="f9c726bf7c661116d8e57f4f"><a href="~/run-actions/f9c726bf7c661116d8e57f4f/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/f9c726bf7c661116d8e57f4f/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
