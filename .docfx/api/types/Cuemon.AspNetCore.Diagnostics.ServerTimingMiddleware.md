---
uid: Cuemon.AspNetCore.Diagnostics.ServerTimingMiddleware
example:
- *content
---

The following example demonstrates how to register and use `ServerTimingMiddleware` to emit `Server-Timing` performance metrics in the response header.

```csharp
// Program.cs
using System;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

await ServerTimingMiddlewareExample.DemonstrateAsync();

namespace Cuemon.AspNetCore.Diagnostics
{
    public static class ServerTimingMiddlewareExample
    {
        public static async Task DemonstrateAsync()
        {
            var context = new DefaultHttpContext();
            var middleware = new ServerTimingMiddleware(httpContext => httpContext.Response.WriteAsync("Hello"));

            using var loggerFactory = LoggerFactory.Create(builder => { });
            var serverTiming = new ServerTiming();
            serverTiming.AddServerTiming("db", TimeSpan.FromMilliseconds(12), "SQL query");

            await middleware.InvokeAsync(
                context,
                loggerFactory.CreateLogger<ServerTimingMiddleware>(),
                new SampleHostEnvironment(),
                serverTiming,
                Options.Create(new ServerTimingOptions { SuppressHeaderPredicate = _ => false }));

            // The middleware writes this header when the response starts.
            Console.WriteLine($"{ServerTiming.HeaderName}: {string.Join(", ", serverTiming.Metrics)}");
        }

        private sealed class SampleHostEnvironment : IHostEnvironment
        {
            public string ApplicationName { get; set; } = "Docs";
            public IFileProvider ContentRootFileProvider { get; set; } = new NullFileProvider();
            public string ContentRootPath { get; set; } = ".";
            public string EnvironmentName { get; set; } = Environments.Development;
        }
    }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="93b775d0d3c01eed7fbeda60"><a href="~/run-actions/93b775d0d3c01eed7fbeda60/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/93b775d0d3c01eed7fbeda60/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
