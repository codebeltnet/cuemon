---
uid: Cuemon.AspNetCore.Http.Headers.ApiKeySentinelMiddleware
example:
- *content
---

The following example demonstrates how to register and use `ApiKeySentinelMiddleware` in the ASP.NET Core pipeline.

```csharp
// Program.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Cuemon.AspNetCore.Http.Headers;

await ApiKeySentinelMiddlewareExample.DemonstrateAsync();

namespace Cuemon.AspNetCore.Http.Headers
{
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
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="d8524252ae56be0b5b4972cb"><a href="~/run-actions/d8524252ae56be0b5b4972cb/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/d8524252ae56be0b5b4972cb/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
