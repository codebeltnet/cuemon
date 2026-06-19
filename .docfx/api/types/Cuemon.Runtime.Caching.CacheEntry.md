---
uid: Cuemon.Runtime.Caching.CacheEntry
example:
- *content
---

The following example demonstrates how to register a cache entry with invalidation rules and inspect its expiration behavior.

```csharp
using System;
using Cuemon.Runtime.Caching;

namespace MyApp.Examples;

public static class CacheEntryExample
{
    public static void Demonstrate()
    {
        var cache = new SlimMemoryCache();
        var entry = new CacheEntry("session", "cached-value", "docs");

        cache.Add(entry, new CacheInvalidation(TimeSpan.FromSeconds(30)));

        Console.WriteLine(entry.CanExpire);
        Console.WriteLine(entry.HasExpired(entry.Accessed.AddSeconds(31)));
        Console.WriteLine(entry.ToString().Contains("Key=session"));
    }
}
```
