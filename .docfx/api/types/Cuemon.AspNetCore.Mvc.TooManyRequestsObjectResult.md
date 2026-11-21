---
uid: Cuemon.AspNetCore.Mvc.TooManyRequestsObjectResult
example:
- *content
---

The following example demonstrates how to return a <xref cref="Cuemon.AspNetCore.Mvc.TooManyRequestsObjectResult"/> with a descriptive error object to produce a 429 HTTP response.

```csharp
// Program.cs
using Cuemon.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using System;
using MyApp.Examples;

Console.WriteLine(new RateLimitController().GetData());

namespace MyApp.Examples
{
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
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="874cda674becce3e326a75b8"><a href="~/run-actions/874cda674becce3e326a75b8/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/874cda674becce3e326a75b8/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
