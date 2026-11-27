---
uid: Cuemon.AspNetCore.Http.Headers.CorrelationIdentifierMiddleware
example:
- *content
---

The following example demonstrates how to register `CorrelationIdentifierMiddleware` in the ASP.NET Core pipeline with a custom header name and correlation token. It then reads the correlation ID from an endpoint to confirm the middleware is working.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Http.Headers;
using Cuemon.Messaging;
using Cuemon.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddLogging();
services.Configure<CorrelationIdentifierOptions>(options =>
{
    options.HeaderName = HttpHeaderNames.XCorrelationId;
    options.Token = new CorrelationToken();
});

using var provider = services.BuildServiceProvider();
var app = new ApplicationBuilder(provider);
app.UseMiddleware<CorrelationIdentifierMiddleware>();
app.Run(async context =>
{
    var correlationId = context.Items[CorrelationIdentifierMiddleware.HttpContextItemsKey];
    await context.Response.WriteAsync($"Correlation ID: {correlationId}");
});

var pipeline = app.Build();
var httpContext = new DefaultHttpContext { RequestServices = provider };
await pipeline(httpContext);

Console.WriteLine(httpContext.Items[CorrelationIdentifierMiddleware.HttpContextItemsKey]);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="7d6c0122aad2dd26533ce63e"><a href="~/run-actions/7d6c0122aad2dd26533ce63e/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/7d6c0122aad2dd26533ce63e/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
