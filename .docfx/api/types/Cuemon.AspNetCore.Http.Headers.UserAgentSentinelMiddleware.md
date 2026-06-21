---
uid: Cuemon.AspNetCore.Http.Headers.UserAgentSentinelMiddleware
example:
- *content
---

The following example demonstrates how to register the <xref cref="Cuemon.AspNetCore.Http.Headers.UserAgentSentinelMiddleware"/> to require a specific User-Agent header on incoming requests.

```csharp
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

        namespace Cuemon.AspNetCore.Http.Headers;

        public static class UserAgentSentinelMiddlewareExample
        {
            public static async Task DemonstrateAsync()
            {
                var context = new DefaultHttpContext();
        var options = Options.Create(new UserAgentSentinelOptions
        {
            RequireUserAgentHeader = true,
            ValidateUserAgentHeader = true,
            AllowedUserAgents = new List<string> { "Cuemon-Agent" }
        });

        context.Request.Headers[HeaderNames.UserAgent] = "Cuemon-Agent";

        var middleware = new UserAgentSentinelMiddleware(
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
