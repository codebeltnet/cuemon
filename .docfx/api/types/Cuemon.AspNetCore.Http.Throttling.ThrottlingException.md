---
uid: Cuemon.AspNetCore.Http.Throttling.ThrottlingException
example:
- *content
---

The following example demonstrates how a <xref cref="Cuemon.AspNetCore.Http.Throttling.ThrottlingException"/> is thrown when a request rate limit has been exceeded.

```csharp
using System;
using Cuemon.AspNetCore.Http.Throttling;

namespace MyApp.Examples;

public class ThrottlingExceptionExample
{
    public void Demonstrate()
    {
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
    }
}
```
