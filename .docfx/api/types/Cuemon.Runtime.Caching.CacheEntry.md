---
uid: Cuemon.Runtime.Caching.CacheEntry
example:
- *content
---

The following example demonstrates how to register a cache entry with invalidation rules and inspect its expiration behavior.

```csharp
// Program.cs
using System;
using Cuemon.Runtime.Caching;

var cache = new SlimMemoryCache();
var entry = new CacheEntry("session", "cached-value", "docs");

cache.Add(entry, new CacheInvalidation(TimeSpan.FromSeconds(30)));

Console.WriteLine(entry.CanExpire);
Console.WriteLine(entry.HasExpired(entry.Accessed.AddSeconds(31)));
Console.WriteLine(entry.ToString().Contains("Key=session"));
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="3b28b53c450a6c66e223b00a"><a href="~/run-actions/3b28b53c450a6c66e223b00a/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/3b28b53c450a6c66e223b00a/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
