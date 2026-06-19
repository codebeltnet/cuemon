---
uid: Cuemon.Extensions.Runtime.Caching.CacheEnumerableExtensions
example:
- *content
---

The following example demonstrates how to cache a generated value and memoize a delegate with <see cref="CacheEnumerableExtensions" />.

```csharp
using System;
using Cuemon.Extensions.Runtime.Caching;
using Cuemon.Runtime.Caching;

namespace MyApp.Examples;

public static class CacheEnumerableExtensionsExample
{
    public static void Demonstrate()
    {
        ICacheEnumerable<long> cache = new SlimMemoryCache();

        var timestamp = cache.GetOrAdd("current-time", TimeSpan.FromSeconds(30), () => DateTime.UtcNow.ToString("O"));
        var cachedAgain = cache.GetOrAdd("current-time", TimeSpan.FromSeconds(30), () => "should-not-be-used");

        var memoized = cache.Memoize(TimeSpan.FromSeconds(30), () => 42);

        Console.WriteLine(timestamp == cachedAgain);
        Console.WriteLine(memoized());
    }
}
```
