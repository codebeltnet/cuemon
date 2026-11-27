---
uid: Cuemon.Extensions.Runtime.Caching.CacheEnumerableExtensions
example:
- *content
---

The following example demonstrates how to cache a generated value and memoize a delegate with <see cref="CacheEnumerableExtensions" />.

```csharp
// Program.cs
using System;
using Cuemon.Extensions.Runtime.Caching;
using Cuemon.Runtime.Caching;

ICacheEnumerable<long> cache = new SlimMemoryCache();

var timestamp = cache.GetOrAdd("current-time", TimeSpan.FromSeconds(30), () => DateTime.UtcNow.ToString("O"));
var cachedAgain = cache.GetOrAdd("current-time", TimeSpan.FromSeconds(30), () => "should-not-be-used");

var memoized = cache.Memoize(TimeSpan.FromSeconds(30), () => 42);

Console.WriteLine(timestamp == cachedAgain);
Console.WriteLine(memoized());
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="22faa847b32c90ee22e4a46d"><a href="~/run-actions/22faa847b32c90ee22e4a46d/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/22faa847b32c90ee22e4a46d/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
