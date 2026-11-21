---
uid: Cuemon.AspNetCore.Builder.MiddlewareBuilderFactory
example:
- *content
---

The following example demonstrates how to register a custom middleware in the application pipeline using `MiddlewareBuilderFactory`.

```csharp
// Program.cs
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Builder;
using Cuemon.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using MyApp.Examples;

MiddlewareBuilderFactoryExample.Demonstrate();

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
            Console.WriteLine($"{context.Request.Path} took {sw.ElapsedMilliseconds} ms");
        }
    }
    public static class MiddlewareBuilderFactoryExample
    {
        public static void Demonstrate()
        {
            var builder = WebApplication.CreateBuilder().Build();

            MiddlewareBuilderFactory.UseMiddleware<TimingMiddleware>(builder);
            Console.WriteLine("TimingMiddleware registered.");
        }
    }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="12ca8aa62e2f7567385910d7"><a href="~/run-actions/12ca8aa62e2f7567385910d7/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/12ca8aa62e2f7567385910d7/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
