---
uid: Cuemon.AspNetCore.Http.Headers.RequestIdentifierMiddleware
example:
- *content
---

The following example demonstrates how to register the <xref cref="Cuemon.AspNetCore.Http.Headers.RequestIdentifierMiddleware"/> in the ASP.NET Core pipeline to add a unique Request-ID header to every response.

```csharp
using System;
using System.Threading.Tasks;
using Cuemon.Messaging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

        namespace Cuemon.AspNetCore.Http.Headers;

        public static class RequestIdentifierMiddlewareExample
        {
            public static async Task DemonstrateAsync()
            {
                var context = new DefaultHttpContext();
        var options = Options.Create(new RequestIdentifierOptions
        {
            Token = new RequestToken("req-42")
        });

        var middleware = new RequestIdentifierMiddleware(
            httpContext => httpContext.Response.WriteAsync("Hello"),
            options);

        await middleware.InvokeAsync(context);
        Console.WriteLine(context.Response.Headers[options.Value.HeaderName].ToString());
            }
        }
```
