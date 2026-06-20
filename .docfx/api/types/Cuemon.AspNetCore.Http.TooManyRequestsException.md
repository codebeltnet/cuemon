---
uid: Cuemon.AspNetCore.Http.TooManyRequestsException
example:
- *content
---

The following example demonstrates how to use <xref cref="Cuemon.AspNetCore.Http.TooManyRequestsException"/> to signal HTTP 429 Too Many Requests responses. It creates instances with default and custom messages, wraps an inner exception, and resolves by status code via `TryParse`. A rate-limit simulation checks request count against a threshold, then sets the `Retry-After` header. Each variation outputs status code, message, and header values to the console, illustrating rate-limit enforcement patterns.

```csharp
using System;
using Cuemon.AspNetCore.Http;

namespace MyApp.Examples;

public class TooManyRequestsExceptionExample
{
    public void Demonstrate()
    {
        // Create a TooManyRequestsException with default message
        var ex = new TooManyRequestsException();
        Console.WriteLine(ex.StatusCode);   // 429
        Console.WriteLine(ex.ReasonPhrase); // Too Many Requests
        Console.WriteLine(ex.Message);     // The allowed number of requests has been exceeded.

        // Create with a custom message
        var custom = new TooManyRequestsException("Rate limit: maximum 100 requests per minute.");
        Console.WriteLine(custom.Message);

        // Create with inner exception
        var inner = new InvalidOperationException("Request quota exceeded.");
        var withInner = new TooManyRequestsException("API rate limit reached.", inner);
        Console.WriteLine(withInner.InnerException?.GetType().Name); // InvalidOperationException

        // Use TryParse from the base class to resolve by status code
        if (HttpStatusCodeException.TryParse(429, "Slow down!", out var parsed))
        {
            Console.WriteLine(parsed.GetType().Name); // TooManyRequestsException
            Console.WriteLine(parsed.StatusCode);     // 429
            Console.WriteLine(parsed.Message);        // Slow down!

        // Simulate a rate-limit check using the RetryAfter header
        var requestCount = 101;
        var maxRequests = 100;
        if (requestCount > maxRequests)
        {
            var rateLimited = new TooManyRequestsException(
                $"Request #{requestCount} exceeds the limit of {maxRequests} requests per minute.");
            rateLimited.Headers["Retry-After"] = "60";
            Console.WriteLine(rateLimited.Message);     // Request #101 exceeds the limit...
            Console.WriteLine(rateLimited.Headers["Retry-After"]); // 60

}}}
}

```
