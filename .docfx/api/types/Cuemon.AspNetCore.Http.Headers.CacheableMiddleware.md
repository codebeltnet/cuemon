---
uid: Cuemon.AspNetCore.Http.Headers.CacheableMiddleware
example:
- *content
---

The following example demonstrates how to register and use `CacheableMiddleware` in the ASP.NET Core pipeline.

```csharp
// Program.cs
using System;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Builder;
using Cuemon.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

var services = new ServiceCollection();
services.AddLogging();
using var provider = services.BuildServiceProvider();
var app = new ApplicationBuilder(provider);

MiddlewareBuilderFactory.UseConfigurableMiddleware<CacheableMiddleware, CacheableOptions>(app, options =>
{
    options.CacheControl = new CacheControlHeaderValue
    {
        Public = true,
        MaxAge = TimeSpan.FromDays(1)
    };
    options.Expires = new ExpiresHeaderValue(TimeSpan.FromDays(1));
});

var cacheOptions = Options.Create(new CacheableOptions
{
    CacheControl = new CacheControlHeaderValue { Public = true, MaxAge = TimeSpan.FromDays(1) }
});
var middleware = new CacheableMiddleware(_ => Task.CompletedTask, cacheOptions);
Console.WriteLine($"CacheControl enabled: {cacheOptions.Value.UseCacheControl}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="8accb88a0490330792c15a53"><a href="~/run-actions/8accb88a0490330792c15a53/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/8accb88a0490330792c15a53/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
