---
uid: Cuemon.Extensions.AspNetCore.Configuration.AssemblyCacheBusting
example:
- *content
---

The following example demonstrates how to create an `AssemblyCacheBusting` instance to provide cache-busting version strings derived from the entry assembly.

```csharp
// Program.cs
using System;
using Cuemon.Extensions.AspNetCore.Configuration;
using Cuemon.Security.Cryptography;
using Microsoft.Extensions.Options;

var options = Options.Create(new AssemblyCacheBustingOptions
{
    Algorithm = UnkeyedCryptoAlgorithm.Sha256,
    ReadByteForByteChecksum = true
});

var cacheBusting = new AssemblyCacheBusting(options);
string version = cacheBusting.Version;

Console.WriteLine($"Cache-busting version: {version}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="0cce42e8751e5dbfa4a5daf3"><a href="~/run-actions/0cce42e8751e5dbfa4a5daf3/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/0cce42e8751e5dbfa4a5daf3/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
