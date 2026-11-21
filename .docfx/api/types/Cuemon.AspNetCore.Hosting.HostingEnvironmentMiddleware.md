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
            var context = new DefaultHttpContext();
            var environment = new SampleHostEnvironment();
            var options = Options.Create(new HostingEnvironmentOptions
            {
                HeaderName = "X-Environment",
                SuppressHeaderPredicate = _ => false
            });

            var middleware = new HostingEnvironmentMiddleware(
                httpContext => httpContext.Response.WriteAsync("Hello"),
                options);

            await middleware.InvokeAsync(context, environment);

            // The middleware writes this header when the response starts.
            Console.WriteLine($"{options.Value.HeaderName}: {environment.EnvironmentName}");
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
<div class="docfx-run-actions" data-example-id="af4d35fd754a2665db716636"><a href="~/run-actions/af4d35fd754a2665db716636/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/af4d35fd754a2665db716636/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
