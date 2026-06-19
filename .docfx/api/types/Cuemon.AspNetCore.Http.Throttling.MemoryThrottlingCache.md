---
uid: Cuemon.AspNetCore.Http.Throttling.MemoryThrottlingCache
example:
- *content
---

```csharp
using System;

namespace Cuemon.AspNetCore.Http.Throttling;

public static class MemoryThrottlingCacheExample
{
    public static void Demonstrate()
    {
        var cache = new MemoryThrottlingCache();
var request = new ThrottleRequest(new ThrottleQuota(10, TimeSpan.FromMinutes(1)));

cache.TryAdd("client-1", request);
Console.WriteLine(cache["client-1"].Total);
    }
}
```
