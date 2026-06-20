---
uid: Cuemon.AspNetCore.Http.Headers.ApiKeySentinelMiddleware
example:
- *content
---

The following example demonstrates how to register and use `ApiKeySentinelMiddleware` in the ASP.NET Core pipeline.

```csharp
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

        namespace Cuemon.AspNetCore.Http.Headers;

        public static class ApiKeySentinelMiddlewareExample
        {
            public static async Task DemonstrateAsync()
            {
                var context = new DefaultHttpContext();
        var options = Options.Create(new ApiKeySentinelOptions
        {
            AllowedKeys = new List<string> { "secret-key" }
        });

        context.Request.Headers[options.Value.HeaderName] = "secret-key";

        var middleware = new ApiKeySentinelMiddleware(
            httpContext =>
            {
                httpContext.Response.StatusCode = StatusCodes.Status200OK;
                return Task.CompletedTask;
            },
            options);

        await middleware.InvokeAsync(context);
        Console.WriteLine(context.Response.StatusCode);
            }
        }
```
