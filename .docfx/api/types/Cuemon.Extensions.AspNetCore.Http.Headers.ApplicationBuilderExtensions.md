---
uid: Cuemon.Extensions.AspNetCore.Http.Headers.ApplicationBuilderExtensions
example:
- *content
---

The following example demonstrates how to add correlation, request, validation, and cache headers to an ASP.NET Core request pipeline.

```csharp
using System;
using Cuemon.AspNetCore.Http.Headers;
using Cuemon.Extensions.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace DocfxExamples;

public class HeaderPipelineExample
{
    public static void Configure(IApplicationBuilder app)
    {
        app.UseCorrelationIdentifier(options => options.HeaderName = "X-Correlation-ID");
        app.UseRequestIdentifier(options => options.HeaderName = "X-Request-ID");
        app.UseUserAgentSentinel(options =>
        {
            options.RequireUserAgentHeader = true;
            options.ValidateUserAgentHeader = true;
            options.AllowedUserAgents.Add("Cuemon-Agent");
        });
        app.UseApiKeySentinel(options =>
        {
            options.HeaderName = "X-Test-Key";
            options.AllowedKeys.Add("known-key");
        });
        app.UseCacheControl(options =>
        {
            options.CacheControl.MaxAge = TimeSpan.FromHours(1);
            options.CacheControl.Public = true;
            options.Expires = new ExpiresHeaderValue(TimeSpan.FromHours(1));
        });
        app.UseVaryAccept();
        app.Run(context => context.Response.WriteAsync("ok"));
    }
}
```
