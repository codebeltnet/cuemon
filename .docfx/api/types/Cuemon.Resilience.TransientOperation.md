---
uid: Cuemon.Resilience.TransientOperation
example:
- *content
---

The following example shows how to execute an HTTP request with retry logic using `TransientOperation`. It configures three retry attempts with exponential backoff and prints the response length on success.

```csharp
using System;
using System.Net.Http;
using Cuemon.Resilience;

namespace Cuemon.Resilience;

public class TransientOperationExample
{
    public void Demonstrate()
    {
        var result = TransientOperation.WithFunc(() =>
        {
            using var client = new HttpClient();
            return client.GetStringAsync("https://example.com").Result;
        }, options =>
        {
            options.RetryAttempts = 3;
            options.RetryStrategy = attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt));
        });

        Console.WriteLine($"Response length: {result.Length}");
    }
}
```
