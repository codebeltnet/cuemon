---
uid: Cuemon.AspNetCore.Builder.MiddlewareBuilderFactory
example:
- *content
---

The following example demonstrates how to register a custom middleware in the application pipeline using `MiddlewareBuilderFactory` and execute a request through the built pipeline.

```csharp
// Program.cs
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Cuemon.AspNetCore;
using Cuemon.AspNetCore.Builder;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MyApp.Examples;

await MiddlewareBuilderFactoryExample.DemonstrateAsync();

namespace MyApp.Examples
{
    public class TimingMiddleware : Middleware
    {
        public TimingMiddleware(RequestDelegate next) : base(next) { }

        public override async Task InvokeAsync(HttpContext context)
        {
            var sw = Stopwatch.StartNew();
            await Next(context);
            sw.Stop();
            context.Response.Headers["X-Elapsed-Ms"] = sw.ElapsedMilliseconds.ToString();
            Console.WriteLine($"Executed {context.Request.Path} in {sw.ElapsedMilliseconds} ms");
        }
    }

    public static class MiddlewareBuilderFactoryExample
    {
        public static async Task DemonstrateAsync()
        {
            var services = new ServiceCollection();
            using var provider = services.BuildServiceProvider();
            var app = new ApplicationBuilder(provider);

            MiddlewareBuilderFactory.UseMiddleware<TimingMiddleware>(app);
            app.Run(httpContext => httpContext.Response.WriteAsync("OK"));

            var pipeline = app.Build();
            var context = new DefaultHttpContext { RequestServices = provider };
            context.Request.Path = "/orders";

            await pipeline(context);

            Console.WriteLine($"X-Elapsed-Ms header present: {context.Response.Headers.ContainsKey("X-Elapsed-Ms")}");
        }
    }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="4a4dfca26363384f61e2f78c"><a href="~/run-actions/4a4dfca26363384f61e2f78c/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/4a4dfca26363384f61e2f78c/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
