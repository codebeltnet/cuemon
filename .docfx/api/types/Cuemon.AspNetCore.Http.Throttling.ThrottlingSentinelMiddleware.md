---
uid: Cuemon.AspNetCore.Http.Throttling.ThrottlingSentinelMiddleware
example:
- *content
---

The following example demonstrates how to register the <xref cref="Cuemon.AspNetCore.Http.Throttling.ThrottlingSentinelMiddleware"/> to enforce rate limiting in the ASP.NET Core pipeline.

```csharp
// Program.cs
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Cuemon.AspNetCore.Http.Throttling;

await ThrottlingSentinelMiddlewareExample.DemonstrateAsync();

namespace Cuemon.AspNetCore.Http.Throttling
{
            public static class ThrottlingSentinelMiddlewareExample
            {
                public static async Task DemonstrateAsync()
                {
                    var context = new DefaultHttpContext();
            var options = Options.Create(new ThrottlingSentinelOptions
            {
                ContextResolver = _ => "client-1",
                Quota = new ThrottleQuota(10, TimeSpan.FromMinutes(1))
            });

            var middleware = new ThrottlingSentinelMiddleware(
                httpContext =>
                {
                    httpContext.Response.StatusCode = StatusCodes.Status200OK;
                    return Task.CompletedTask;
                },
                options);

            await middleware.InvokeAsync(context, new MemoryThrottlingCache());
            Console.WriteLine(context.Response.Headers[options.Value.RateLimitHeaderName].ToString());
                }
            }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="c95a7c9ff4e7da2d7d120bfb"><a href="~/run-actions/c95a7c9ff4e7da2d7d120bfb/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/c95a7c9ff4e7da2d7d120bfb/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
