---
uid: Cuemon.AspNetCore.Diagnostics.ServerTimingMiddleware
example:
- *content
---

The following example demonstrates how to register and use `ServerTimingMiddleware` to emit `Server-Timing` performance metrics in the response header.

```csharp
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

        namespace Cuemon.AspNetCore.Diagnostics;

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

        Console.WriteLine(context.Response.Headers[ServerTiming.HeaderName].ToString());
            }
    private sealed class SampleHostEnvironment : IHostEnvironment
    {
        public string ApplicationName { get; set; } = "Docs";
        public IFileProvider ContentRootFileProvider { get; set; } = new NullFileProvider();
        public string ContentRootPath { get; set; } = ".";
        public string EnvironmentName { get; set; } = Environments.Development;
    }
        }
```
