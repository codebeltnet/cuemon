---
uid: Cuemon.AspNetCore.Http.TooManyRequestsException
example:
- *content
---

The following example demonstrates how to use <xref cref="Cuemon.AspNetCore.Http.TooManyRequestsException"/> to signal HTTP 429 Too Many Requests responses. It creates instances with default and custom messages, wraps an inner exception, and resolves by status code via `TryParse`. A rate-limit simulation checks request count against a threshold, then sets the `Retry-After` header. Each variation outputs status code, message, and header values to the console, illustrating rate-limit enforcement patterns.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Http;

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
Console.WriteLine(withInner.InnerException?.Message); // Request quota exceeded.

// Use TryParse from the base class to resolve by status code
if (HttpStatusCodeException.TryParse(429, "Slow down!", out var parsed))
{
    Console.WriteLine(parsed.Message);        // Slow down!
    Console.WriteLine(parsed.StatusCode);     // 429
}

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

}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="4a3f1a622982da3e2b42c470"><a href="~/run-actions/4a3f1a622982da3e2b42c470/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/4a3f1a622982da3e2b42c470/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
