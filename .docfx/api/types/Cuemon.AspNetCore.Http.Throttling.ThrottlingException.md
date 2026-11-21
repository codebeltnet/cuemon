---
uid: Cuemon.AspNetCore.Http.Throttling.ThrottlingException
example:
- *content
---

The following example demonstrates how a <xref cref="Cuemon.AspNetCore.Http.Throttling.ThrottlingException"/> is thrown when a request rate limit has been exceeded.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Http.Throttling;

try
{
    // Simulate a rate-limit violation
    var resetTime = DateTime.UtcNow.AddMinutes(5);
    throw new ThrottlingException(
        "API rate limit exceeded.",
        rateLimit: 100,
        delta: TimeSpan.FromMinutes(5),
        reset: resetTime);
}
catch (ThrottlingException ex)
{
    Console.WriteLine($"Message: {ex.Message}");                        // API rate limit exceeded.
    Console.WriteLine($"RateLimit: {ex.RateLimit}");                     // 100
    Console.WriteLine($"Delta: {ex.Delta.TotalMinutes} minutes");       // 5
    Console.WriteLine($"Reset: {ex.Reset}");                            // UTC reset time
    Console.WriteLine($"StatusCode: {ex.StatusCode}");                  // 429
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="f4356a48652e2d33aeb3429c"><a href="~/run-actions/f4356a48652e2d33aeb3429c/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/f4356a48652e2d33aeb3429c/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
