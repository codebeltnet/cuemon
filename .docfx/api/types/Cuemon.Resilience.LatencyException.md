---
uid: Cuemon.Resilience.LatencyException
example:
- *content
---

The following example demonstrates how to wrap a timeout-related failure in a <see cref="LatencyException" />.

```csharp
using System;
using Cuemon.Resilience;

namespace MyApp.Examples;

public static class LatencyExceptionExample
{
    public static void Demonstrate()
    {
        var timeout = new TimeoutException("The database query timed out after 10 seconds.");
        var exception = new LatencyException("Order processing exceeded the configured latency threshold.", timeout);

        Console.WriteLine(exception.Message);
        Console.WriteLine(exception.InnerException?.GetType().Name);
    }
}
```
