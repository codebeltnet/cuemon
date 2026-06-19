---
uid: Cuemon.Runtime.Caching.CachingManager
example:
- *content
---

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
