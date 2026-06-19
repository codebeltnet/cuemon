---
uid: Cuemon.AspNetCore.Http.Headers.CacheableMiddleware
example:
- *content
---

The following example demonstrates how to register and use `CacheableMiddleware` in the ASP.NET Core pipeline.

```csharp
using System;
using Cuemon.AspNetCore.Builder;
using Cuemon.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace MyApp.Examples;

public class CacheableMiddlewareExample
{
    public void Configure(IApplicationBuilder app)
    {
        // Standard registration via MiddlewareBuilderFactory (recommended)
        MiddlewareBuilderFactory.UseConfigurableMiddleware<CacheableMiddleware, CacheableOptions>(app, options =>
        {
            options.CacheControl = new CacheControlHeaderValue
            {
                Public = true,
                MaxAge = TimeSpan.FromDays(1)
            };
            options.Expires = new ExpiresHeaderValue(TimeSpan.FromDays(1));
        });
    }

    public void DirectInstantiation(RequestDelegate next, IOptions<CacheableOptions> options)
    {
        // Direct usage of the CacheableMiddleware type
        var middleware = new CacheableMiddleware(next, options);
        Console.WriteLine($"Middleware created (CacheControl: {options.Value.UseCacheControl})");
    }
}
```
