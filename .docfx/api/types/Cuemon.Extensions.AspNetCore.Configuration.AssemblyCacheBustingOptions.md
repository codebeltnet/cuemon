---
uid: Cuemon.Extensions.AspNetCore.Configuration.AssemblyCacheBustingOptions
example:
- *content
---

The following example demonstrates how to configure `AssemblyCacheBustingOptions` for a reproducible cache-busting version that is derived from a known assembly and hash algorithm.

```csharp
// Program.cs
using System;
using Cuemon;
using Cuemon.Extensions.AspNetCore.Configuration;
using Cuemon.Security.Cryptography;
using Microsoft.Extensions.Options;

var options = Options.Create(new AssemblyCacheBustingOptions
{
    Assembly = typeof(AssemblyCacheBustingOptions).Assembly,
    Algorithm = UnkeyedCryptoAlgorithm.Sha256,
    PreferredCasing = CasingMethod.UpperCase,
    ReadByteForByteChecksum = true
});

var cacheBusting = new AssemblyCacheBusting(options);

Console.WriteLine(cacheBusting.Version);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="0c4231799dc4eda8b1ab2426"><a href="~/run-actions/0c4231799dc4eda8b1ab2426/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/0c4231799dc4eda8b1ab2426/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
