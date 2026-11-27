---
uid: Cuemon.AspNetCore.Mvc.Filters.Diagnostics.ServerTimingFilter
example:
- *content
---

The following example requires `Cuemon.AspNetCore.Mvc` and sets up a `ServiceCollection` with `IServerTiming`, `ServerTimingOptions` (`UseTimeMeasureProfiler = true`), `IHostEnvironment`, and `ILogger` for <xref cref="Cuemon.AspNetCore.Mvc.Filters.Diagnostics.ServerTimingFilter"/>. It prepares an MVC `ActionExecutingContext` and `ActionExecutedContext`, then invokes `OnActionExecuting` and `OnActionExecuted` around an asynchronous delay. As the outcome, the response emits a `Server-Timing` header with the recorded action duration.

```csharp
// Program.cs
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.AspNetCore.Mvc.Filters.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyApp.Examples;

await ServerTimingFilterExample.DemonstrateAsync();

namespace MyApp.Examples
{
    public static class ServerTimingFilterExample
    {
        public static async Task DemonstrateAsync()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IServerTiming, ServerTiming>();
            using var provider = services.BuildServiceProvider();

            var options = Options.Create(new ServerTimingOptions
            {
                UseTimeMeasureProfiler = true,
                SuppressHeaderPredicate = _ => false
            });

            using var loggerFactory = LoggerFactory.Create(_ => { });
            var filter = new ServerTimingFilter(
                options,
                new SampleHostEnvironment(),
                loggerFactory.CreateLogger<ServerTimingFilter>());

            var httpContext = new DefaultHttpContext { RequestServices = provider };
            httpContext.Request.Scheme = "https";
            httpContext.Request.Host = new HostString("api.example.com");
            httpContext.Request.Path = "/orders";

            var actionDescriptor = new ControllerActionDescriptor
            {
                ControllerTypeInfo = typeof(ServerTimingFilterExample).GetTypeInfo(),
                MethodInfo = typeof(ServerTimingFilterExample).GetMethod(nameof(SampleAction))!
            };
            var actionContext = new ActionContext(httpContext, new RouteData(), actionDescriptor);
            var filters = new List<IFilterMetadata> { filter };

            var executingContext = new ActionExecutingContext(actionContext, filters, new Dictionary<string, object?>(), controller: new object());
            filter.OnActionExecuting(executingContext);
            await Task.Delay(25);
            var executedContext = new ActionExecutedContext(actionContext, filters, executingContext.Controller);
            filter.OnActionExecuted(executedContext);

            Console.WriteLine($"{ServerTiming.HeaderName}: {httpContext.Response.Headers[ServerTiming.HeaderName]}");
        }

        public static void SampleAction()
        {
        }

        private sealed class SampleHostEnvironment : IHostEnvironment
        {
            public string EnvironmentName { get; set; } = Environments.Development;

            public string ApplicationName { get; set; } = nameof(ServerTimingFilterExample);

            public string ContentRootPath { get; set; } = AppContext.BaseDirectory;

            public IFileProvider ContentRootFileProvider { get; set; } = new NullFileProvider();
        }
    }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="f9f877b1d59859b205bfb04c"><a href="~/run-actions/f9f877b1d59859b205bfb04c/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/f9f877b1d59859b205bfb04c/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
