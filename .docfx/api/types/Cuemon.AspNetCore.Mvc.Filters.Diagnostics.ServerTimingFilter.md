---
uid: Cuemon.AspNetCore.Mvc.Filters.Diagnostics.ServerTimingFilter
example:
- *content
---

The following example shows the constructor dependencies used when creating <xref cref="Cuemon.AspNetCore.Mvc.Filters.Diagnostics.ServerTimingFilter"/> directly.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.AspNetCore.Mvc.Filters.Diagnostics;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyApp.Examples;

ServerTimingFilterExample.Demonstrate();

namespace MyApp.Examples
{
    public static class ServerTimingFilterExample
    {
        public static void Demonstrate()
        {
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

            Console.WriteLine(filter.Options.UseTimeMeasureProfiler);
            Console.WriteLine(filter.Options.SuppressHeaderPredicate is not null);
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
<div class="docfx-run-actions" data-example-id="07cb1b2ab9859c94b3bcb30f"><a href="~/run-actions/07cb1b2ab9859c94b3bcb30f/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/07cb1b2ab9859c94b3bcb30f/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
