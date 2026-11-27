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
using Microsoft.AspNetCore.Http.Features;
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
            var responseFeature = new CapturingHttpResponseFeature();
            var context = new DefaultHttpContext();
            context.Features.Set<IHttpResponseFeature>(responseFeature);

            var middleware = new ServerTimingMiddleware(async httpContext =>
            {
                await responseFeature.FireOnStartingAsync();
                await httpContext.Response.WriteAsync("Hello");
            });

            using var loggerFactory = LoggerFactory.Create(builder => { });
            var serverTiming = new ServerTiming();
            serverTiming.AddServerTiming("db", TimeSpan.FromMilliseconds(12), "SQL query");

            await middleware.InvokeAsync(
                context,
                loggerFactory.CreateLogger<ServerTimingMiddleware>(),
                new SampleHostEnvironment(),
                serverTiming,
                Options.Create(new ServerTimingOptions { SuppressHeaderPredicate = _ => false }));

            Console.WriteLine($"{ServerTiming.HeaderName}: {context.Response.Headers[ServerTiming.HeaderName]}");
        }

        private sealed class CapturingHttpResponseFeature : HttpResponseFeature
        {
            private Func<Task>? _onStarting;

            public override void OnStarting(Func<object, Task> callback, object state)
            {
                var prior = _onStarting;
                _onStarting = async () =>
                {
                    await callback(state);
                    if (prior != null) { await prior(); }
                };
            }

            public Task FireOnStartingAsync() => _onStarting?.Invoke() ?? Task.CompletedTask;
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
<div class="docfx-run-actions" data-example-id="83744415bc1daefa96848599"><a href="~/run-actions/83744415bc1daefa96848599/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/83744415bc1daefa96848599/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
