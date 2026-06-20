---
uid: Cuemon.AspNetCore.Mvc.TooManyRequestsObjectResult
example:
- *content
---

The following example demonstrates how to return a <xref cref="Cuemon.AspNetCore.Mvc.TooManyRequestsObjectResult"/> with a descriptive error object to produce a 429 HTTP response.

```csharp
using Cuemon.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Examples;

public class RateLimitController : Controller
{
    [HttpGet("/api/rate-limited")]
    public IActionResult GetData()
    {
        return new TooManyRequestsObjectResult(new
        {
            error = "API rate limit exceeded. Please try again later.",
            retryAfter = 60
        });

}
}

```
