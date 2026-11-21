---
uid: Cuemon.Extensions.AspNetCore.Configuration.ServiceCollectionExtensions
example:
- *content
---

The following example demonstrates how to register the built-in cache-busting services and a custom `ICacheBusting` implementation.

```csharp
// Program.cs
using System;
using System.Linq;
using Cuemon.AspNetCore.Configuration;
using Cuemon.Extensions.AspNetCore.Configuration;
using Cuemon.Security.Cryptography;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddOptions();
services.Configure<AssemblyCacheBustingOptions>(options =>
{
    options.Assembly = typeof(ReleaseCacheBusting).Assembly;
    options.Algorithm = UnkeyedCryptoAlgorithm.Sha256;
    options.ReadByteForByteChecksum = true;
});

services.AddAssemblyCacheBusting();
services.AddDynamicCacheBusting();
services.AddCacheBusting<ReleaseCacheBusting>();

using var provider = services.BuildServiceProvider();
var versions = provider.GetServices<ICacheBusting>().Select(cache => cache.Version).ToList();

Console.WriteLine(versions.Count);
Console.WriteLine(string.Join(", ", versions));

public sealed class ReleaseCacheBusting : ICacheBusting
{
    public string Version => "20260618";
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="7083124b13769a25e3631a5d"><a href="~/run-actions/7083124b13769a25e3631a5d/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/7083124b13769a25e3631a5d/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
