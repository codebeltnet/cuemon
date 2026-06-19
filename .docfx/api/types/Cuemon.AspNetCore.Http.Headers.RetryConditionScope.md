---
uid: Cuemon.AspNetCore.Http.Headers.RetryConditionScope
example:
- *content
---

```csharp
using System;
using System.Net.Http.Headers;

namespace Cuemon.AspNetCore.Http.Headers;

public static class RetryConditionScopeExample
{
    public static void Demonstrate()
    {
        var scope = RetryConditionScope.DeltaSeconds;
        string retryAfter = scope == RetryConditionScope.DeltaSeconds
            ? new RetryConditionHeaderValue(TimeSpan.FromSeconds(30)).ToString()
            : new RetryConditionHeaderValue(DateTimeOffset.UtcNow.AddMinutes(1)).ToString();

        Console.WriteLine(retryAfter);
    }
}
```
