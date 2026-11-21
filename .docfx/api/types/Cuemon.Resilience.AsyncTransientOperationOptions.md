---
uid: Cuemon.Resilience.AsyncTransientOperationOptions
example:
- *content
---

The following example demonstrates how to configure retry options for an asynchronous transient operation with exponential backoff.

```csharp
// Program.cs
using System;
using System.Threading.Tasks;
using Cuemon.Resilience;
using Examples;

await new AsyncTransientOperationExample().ExecuteWithRetryAsync();

namespace Examples
{
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
            Console.WriteLine(result);

    }
    }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="4ec088491a5fbb3c8b47249d"><a href="~/run-actions/4ec088491a5fbb3c8b47249d/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/4ec088491a5fbb3c8b47249d/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
