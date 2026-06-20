---
uid: Cuemon.AspNetCore.Http.Throttling.ThrottleRequest
example:
- *content
---

The following example demonstrates how to use <xref cref="Cuemon.AspNetCore.Http.Throttling.ThrottleRequest"/> to track HTTP request usage and quota in a throttling scenario.

```csharp
using System;

namespace Cuemon.AspNetCore.Http.Throttling;

public static class ThrottleRequestExample
{
    public static void Demonstrate()
    {
        var request = new ThrottleRequest(new ThrottleQuota(10, TimeSpan.FromMinutes(1)));
request.IncrementTotal();

Console.WriteLine(request.Total);
    }
}
```
