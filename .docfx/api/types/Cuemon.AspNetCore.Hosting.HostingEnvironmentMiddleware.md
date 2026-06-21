---
uid: Cuemon.AspNetCore.Hosting.HostingEnvironmentMiddleware
example:
- *content
---

The following example demonstrates how to register and use `HostingEnvironmentMiddleware` in the ASP.NET Core pipeline.

```csharp
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

        namespace Cuemon.AspNetCore.Hosting;

        public static class HostingEnvironmentMiddlewareExample
        {
            public static async Task DemonstrateAsync()
            {
                var context = new DefaultHttpContext();
        var middleware = new HostingEnvironmentMiddleware(
            httpContext => httpContext.Response.WriteAsync("Hello"),
            Options.Create(new HostingEnvironmentOptions
            {
                HeaderName = "X-Environment",
                SuppressHeaderPredicate = _ => false
            }));

        await middleware.InvokeAsync(context, new SampleHostEnvironment());
        Console.WriteLine(context.Response.Headers["X-Environment"].ToString());
            }
    private sealed class SampleHostEnvironment : IHostEnvironment
    {
        public string ApplicationName { get; set; } = "Docs";
        public IFileProvider ContentRootFileProvider { get; set; } = new NullFileProvider();
        public string ContentRootPath { get; set; } = ".";
        public string EnvironmentName { get; set; } = Environments.Staging;
    }
        }
```
