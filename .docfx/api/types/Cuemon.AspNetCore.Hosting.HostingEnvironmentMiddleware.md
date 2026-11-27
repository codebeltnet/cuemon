---
uid: Cuemon.AspNetCore.Hosting.HostingEnvironmentMiddleware
example:
- *content
---

The following example demonstrates how to register and use `HostingEnvironmentMiddleware` in the ASP.NET Core pipeline.

```csharp
// Program.cs
using System;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

await HostingEnvironmentMiddlewareExample.DemonstrateAsync();

namespace Cuemon.AspNetCore.Hosting
{
    public static class HostingEnvironmentMiddlewareExample
    {
        public static async Task DemonstrateAsync()
        {
            var responseFeature = new CapturingHttpResponseFeature();
            var context = new DefaultHttpContext();
            context.Features.Set<IHttpResponseFeature>(responseFeature);

            var environment = new SampleHostEnvironment();
            var options = Options.Create(new HostingEnvironmentOptions
            {
                HeaderName = "X-Environment",
                SuppressHeaderPredicate = _ => false
            });

            var middleware = new HostingEnvironmentMiddleware(
                async httpContext =>
                {
                    await responseFeature.FireOnStartingAsync();
                    await httpContext.Response.WriteAsync("Hello");
                },
                options);

            await middleware.InvokeAsync(context, environment);

            Console.WriteLine($"{options.Value.HeaderName}: {context.Response.Headers[options.Value.HeaderName]}");
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
            public string EnvironmentName { get; set; } = Environments.Staging;
        }
    }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="34d97dfc5c86f9f0cc712f72"><a href="~/run-actions/34d97dfc5c86f9f0cc712f72/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/34d97dfc5c86f9f0cc712f72/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
