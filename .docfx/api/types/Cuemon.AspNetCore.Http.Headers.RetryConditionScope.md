---
uid: Cuemon.AspNetCore.Http.Headers.RetryConditionScope
example:
- *content
---

The following example demonstrates how to use `RetryConditionScope` to choose the format of a Retry-After header. It compares delta-seconds and absolute-date scopes and prints the resulting header value.

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
