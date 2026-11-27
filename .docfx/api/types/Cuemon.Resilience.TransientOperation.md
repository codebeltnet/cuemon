---
uid: Cuemon.Resilience.TransientOperation
example:
- *content
---

The following example shows how to execute an HTTP request with retry logic using `TransientOperation`. It configures three retry attempts with exponential backoff and prints the response length on success.

```csharp
// Program.cs
using System;
using System.Net.Http;
using Cuemon.Resilience;

new TransientOperationExample().Demonstrate();

namespace Cuemon.Resilience
{
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
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="90bb9fef9d2e4fdfb5b0f7cf"><a href="~/run-actions/90bb9fef9d2e4fdfb5b0f7cf/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/90bb9fef9d2e4fdfb5b0f7cf/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
