---
uid: Cuemon.Runtime.Caching.CachingManager
example:
- *content
---

The following example demonstrates how to get or add a cached value using `CachingManager.Cache`. It checks for an existing key, adds a value with a 5-minute expiration if not found, and verifies the cached instance is reused on subsequent accesses.

```csharp
using System;
using Cuemon.Runtime.Caching;

namespace Cuemon.Runtime.Caching;

public class CachingManagerExample
{
    public void Demonstrate()
    {
        var cache = CachingManager.Cache;
        var key = "myKey";
        var value = cache.Get(key);
        if (value == null)
        {
            cache.Add(key, DateTime.UtcNow, TimeSpan.FromMinutes(5));
        }
        value = cache.Get(key);
        Console.WriteLine($"Cached value: {value}");

        var sameValue = cache.Get(key);
        Console.WriteLine($"Same instance? {value == sameValue}");
    }
}
```
