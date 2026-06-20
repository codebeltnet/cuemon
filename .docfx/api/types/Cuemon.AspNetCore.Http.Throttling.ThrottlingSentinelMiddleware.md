---
uid: Cuemon.AspNetCore.Http.Throttling.ThrottlingSentinelMiddleware
example:
- *content
---

The following example demonstrates how to register the <xref cref="Cuemon.AspNetCore.Http.Throttling.ThrottlingSentinelMiddleware"/> to enforce rate limiting in the ASP.NET Core pipeline.

```csharp
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

        namespace Cuemon.AspNetCore.Http.Throttling;

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
```
