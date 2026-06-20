---
uid: Cuemon.AspNetCore.Http.Throttling.MemoryThrottlingCache
example:
- *content
---

The following example shows how to create a `MemoryThrottlingCache`, add a throttle request for a client, and retrieve the request count from the cache.

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
