---
uid: Cuemon.AspNetCore.Http.Headers.UserAgentSentinelMiddleware
example:
- *content
---

The following example demonstrates how to register the <xref cref="Cuemon.AspNetCore.Http.Headers.UserAgentSentinelMiddleware"/> to require a specific User-Agent header on incoming requests.

```csharp
// Program.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Cuemon.AspNetCore.Http.Headers;

await UserAgentSentinelMiddlewareExample.DemonstrateAsync();

namespace Cuemon.AspNetCore.Http.Headers
{
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
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="3425b34190ca70c78e09169b"><a href="~/run-actions/3425b34190ca70c78e09169b/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/3425b34190ca70c78e09169b/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
