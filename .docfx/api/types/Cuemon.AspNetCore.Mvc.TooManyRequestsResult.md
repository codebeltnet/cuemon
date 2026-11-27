---
uid: Cuemon.AspNetCore.Mvc.TooManyRequestsResult
example:
- *content
---

The following example demonstrates how to return a <xref cref="Cuemon.AspNetCore.Mvc.TooManyRequestsResult"/> from a controller action to produce a 429 HTTP response.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using MyApp.Examples;

Console.WriteLine(new RateLimitController().GetData());

namespace MyApp.Examples
{
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
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="6fe6f772f176efcecb3061cb"><a href="~/run-actions/6fe6f772f176efcecb3061cb/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/6fe6f772f176efcecb3061cb/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
