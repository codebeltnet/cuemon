---
uid: Cuemon.Resilience.AsyncTransientOperationOptions
example:
- *content
---

The following example demonstrates how to configure retry options for an asynchronous transient operation with exponential backoff.

```csharp
using System;
using System.Threading.Tasks;
using Cuemon.Resilience;

namespace Examples;

public class AsyncTransientOperationExample
{
    public async Task ExecuteWithRetryAsync()
    {
        // Direct instantiation of AsyncTransientOperationOptions
        var transientOptions = new AsyncTransientOperationOptions
        {
            RetryAttempts = 3,
            MaximumAllowedLatency = TimeSpan.FromSeconds(30)
        };
        transientOptions.RetryStrategy = currentAttempt => TimeSpan.FromSeconds(Math.Pow(2, currentAttempt));
        transientOptions.DetectionStrategy = exception => exception is TimeoutException;

        var result = await TransientOperation.WithFuncAsync(async ct =>
        {
            return await Task.FromResult(42);
        }, o =>
        {
            o.RetryAttempts = 3;
            o.RetryStrategy = currentAttempt => TimeSpan.FromSeconds(Math.Pow(2, currentAttempt));
            o.MaximumAllowedLatency = TimeSpan.FromSeconds(30);
            o.DetectionStrategy = exception => exception is TimeoutException;
        });
        // result == 42 after up to 3 retries with exponential backoff

}
}

```
