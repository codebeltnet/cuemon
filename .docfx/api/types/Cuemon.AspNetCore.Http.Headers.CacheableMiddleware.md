---
uid: Cuemon.AspNetCore.Http.Headers.CacheableMiddleware
example:
- *content
---

The following example demonstrates how to register and execute `CacheableMiddleware` in the ASP.NET Core pipeline and observe the resulting `Cache-Control` and `Expires` response headers.

```csharp
// Program.cs
using System;
using System.IO;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Builder;
using Cuemon.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
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

app.Run(httpContext => httpContext.Response.WriteAsync("Cached response"));

var context = new DefaultHttpContext { RequestServices = provider };
context.Response.Body = new MemoryStream();

await app.Build()(context);

Console.WriteLine($"Cache-Control: {context.Response.Headers[HeaderNames.CacheControl]}");
Console.WriteLine($"Expires present: {context.Response.Headers.ContainsKey(HeaderNames.Expires)}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="e221c631d806e51cd959850b"><a href="~/run-actions/e221c631d806e51cd959850b/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/e221c631d806e51cd959850b/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
