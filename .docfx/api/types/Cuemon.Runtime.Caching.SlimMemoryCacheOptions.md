---
uid: Cuemon.Runtime.Caching.SlimMemoryCacheOptions
example:
- *content
---

The following example demonstrates how to configure <see cref="SlimMemoryCacheOptions" /> for sweep intervals and cache-key generation.

```csharp
// Program.cs
using System;
using Cuemon.Runtime.Caching;

var options = new SlimMemoryCacheOptions
{
    EnableCleanup = false,
    FirstSweep = TimeSpan.FromSeconds(10),
    SucceedingSweep = TimeSpan.FromSeconds(30),
    KeyProvider = (key, ns) => key.Length + (ns == CacheEntry.NoScope ? 0 : ns.Length)
};

options.ValidateOptions();

Console.WriteLine(options.EnableCleanup);
Console.WriteLine(options.FirstSweep);
Console.WriteLine(options.KeyProvider("session", "docs"));
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="9d13a697d398c17b65270438"><a href="~/run-actions/9d13a697d398c17b65270438/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/9d13a697d398c17b65270438/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
