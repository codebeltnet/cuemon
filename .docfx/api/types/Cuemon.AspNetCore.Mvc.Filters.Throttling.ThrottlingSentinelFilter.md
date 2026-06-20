---
uid: Cuemon.AspNetCore.Mvc.Filters.Throttling.ThrottlingSentinelFilter
example:
- *content
---

The following example constructs <xref cref="Cuemon.AspNetCore.Mvc.Filters.Throttling.ThrottlingSentinelFilter"/> directly with a packet-local throttling cache and options object.

```csharp
using System;
using Cuemon;
using Cuemon.AspNetCore.Http.Throttling;
using Cuemon.AspNetCore.Mvc.Filters.Throttling;
using Microsoft.Extensions.Options;

namespace MyApp.Examples;

public static class ThrottlingSentinelFilterExample
{
    public static void Demonstrate()
    {
        var options = Options.Create(new ThrottlingSentinelOptions
        {
            ContextResolver = _ => "developer-workstation",
            Quota = new ThrottleQuota(10, 5, TimeUnit.Seconds)
        });

        var filter = new ThrottlingSentinelFilter(options, new MemoryThrottlingCache());

        Console.WriteLine(filter.Options.Quota.RateLimit);
        Console.WriteLine(filter.Options.UseRetryAfterHeader);
    }
}
```
