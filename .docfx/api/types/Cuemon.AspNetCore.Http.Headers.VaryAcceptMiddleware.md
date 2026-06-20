---
uid: Cuemon.AspNetCore.Http.Headers.VaryAcceptMiddleware
example:
- *content
---

The following example demonstrates how to register the <xref cref="Cuemon.AspNetCore.Http.Headers.VaryAcceptMiddleware"/> to append a `Vary: Accept` header to every response.

```csharp
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

        namespace Cuemon.AspNetCore.Http.Headers;

        public static class VaryAcceptMiddlewareExample
        {
            public static async Task DemonstrateAsync()
            {
                var context = new DefaultHttpContext();
        var middleware = new VaryAcceptMiddleware(httpContext => httpContext.Response.WriteAsync("Hello"));

        await middleware.InvokeAsync(context);
        Console.WriteLine(context.Response.Headers[HeaderNames.Vary].ToString());
            }
        }
```
