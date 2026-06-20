---
uid: Cuemon.Extensions.AspNetCore.Http.Throttling.ApplicationBuilderExtensions
example:
- *content
---

The following example demonstrates how to add a request rate limiting middleware to an ASP.NET Core application pipeline.

```csharp
using System;
using Cuemon;
using Cuemon.AspNetCore.Http.Throttling;
using Cuemon.Extensions.AspNetCore.Http.Throttling;
using Microsoft.AspNetCore.Builder;

namespace Examples;

public class StartupPipeline
{
    public void Configure(IApplicationBuilder app)
    {
        app.UseThrottlingSentinel(o =>
        {
            o.Quota = new ThrottleQuota(100, 1, TimeUnit.Minutes);
            o.ContextResolver = ctx => ctx.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        });

        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

}
}

```
