---
uid: Cuemon.Resilience.TransientOperationOptions
example:
- *content
---

The following example demonstrates how to configure <see cref="TransientOperationOptions" /> for retry attempts, transient-fault detection, and latency limits.

```csharp
using System;
using System.Net.Http;
using Cuemon.Resilience;

namespace MyApp.Examples;

public static class TransientOperationOptionsExample
{
    public static void Demonstrate()
    {
        var options = new TransientOperationOptions
        {
            RetryAttempts = 3,
            RetryStrategy = attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
            DetectionStrategy = exception => exception is HttpRequestException,
            MaximumAllowedLatency = TimeSpan.FromSeconds(30)
        };

        options.ValidateOptions();

        Console.WriteLine(options.RetryAttempts);
        Console.WriteLine(options.EnableRecovery);
        Console.WriteLine(options.MaximumAllowedLatency);
        Console.WriteLine(options.DetectionStrategy(new HttpRequestException("Network timeout simulated.")));
    }
}
```
