---
uid: Cuemon.AspNetCore.Mvc.TooManyRequestsResult
example:
- *content
---

The following example demonstrates how to return a <xref cref="Cuemon.AspNetCore.Mvc.TooManyRequestsResult"/> from a controller action to produce a 429 HTTP response.

```csharp
using System;
using Cuemon.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Examples;

public class RateLimitController : Controller
{
    [HttpGet("/api/rate-limited")]
    public IActionResult GetData()
    {
        var result = new TooManyRequestsResult();
        Console.WriteLine(result.StatusCode); // 429
        return result;

}
}

```
