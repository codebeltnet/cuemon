---
uid: Cuemon.AspNetCore.Mvc.Filters.Throttling.ThrottlingSentinelFilter
example:
- *content
---

The following example requires `Cuemon.AspNetCore.Mvc` and sets up <xref cref="Cuemon.AspNetCore.Mvc.Filters.Throttling.ThrottlingSentinelFilter"/> with `ThrottlingSentinelOptions` (`ThrottleQuota(2, 5, TimeUnit.Seconds)`) and `MemoryThrottlingCache`. It invokes `OnActionExecutionAsync` three consecutive times for the same client context. As the outcome, the first two requests succeed with decrementing `RateLimit-Remaining` headers (`1`, then `0`), and the third throws `ThrottlingException` (HTTP 429).

```csharp
// Program.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cuemon;
using Cuemon.AspNetCore.Http.Throttling;
using Cuemon.AspNetCore.Mvc.Filters.Throttling;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;

var options = Options.Create(new ThrottlingSentinelOptions
{
    ContextResolver = _ => "developer-workstation",
    Quota = new ThrottleQuota(2, 5, TimeUnit.Seconds)
});

var filter = new ThrottlingSentinelFilter(options, new MemoryThrottlingCache());

for (int i = 1; i <= 3; i++)
{
    var httpContext = new DefaultHttpContext();
    var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
    var executingContext = new ActionExecutingContext(actionContext, new List<IFilterMetadata>(), new Dictionary<string, object>(), new object());

    try
    {
        await filter.OnActionExecutionAsync(executingContext, () =>
            Task.FromResult(new ActionExecutedContext(actionContext, new List<IFilterMetadata>(), new object())));
        Console.WriteLine($"Request {i}: allowed (RateLimit-Remaining={httpContext.Response.Headers["RateLimit-Remaining"]})");
    }
    catch (ThrottlingException ex)
    {
        Console.WriteLine($"Request {i}: throttled ({(int)ex.StatusCode}, {ex.ReasonPhrase})");
    }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="5d0e583d15fed9ee1730ddb0"><a href="~/run-actions/5d0e583d15fed9ee1730ddb0/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/5d0e583d15fed9ee1730ddb0/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
