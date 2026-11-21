---
uid: Cuemon.Extensions.AspNetCore.Http.Headers.ApplicationBuilderExtensions
example:
- *content
---

The following example demonstrates how to add correlation, request, validation, and cache headers to an ASP.NET Core request pipeline.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Http.Headers;
using Cuemon.Extensions.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;

var services = new ServiceCollection();
services.AddLogging();
services.AddOptions();
using var provider = services.BuildServiceProvider();

var app = new ApplicationBuilder(provider);
app.UseCorrelationIdentifier(options => options.HeaderName = "X-Correlation-ID");
app.UseRequestIdentifier(options => options.HeaderName = "X-Request-ID");
app.UseUserAgentSentinel(options =>
{
    options.RequireUserAgentHeader = true;
    options.ValidateUserAgentHeader = true;
    options.AllowedUserAgents.Add("Cuemon-Agent");
});
app.UseApiKeySentinel(options =>
{
    options.HeaderName = "X-Test-Key";
    options.AllowedKeys.Add("known-key");
});
app.UseCacheControl(options =>
{
    options.CacheControl.MaxAge = TimeSpan.FromHours(1);
    options.CacheControl.Public = true;
    options.Expires = new ExpiresHeaderValue(TimeSpan.FromHours(1));
});
app.UseVaryAccept();

Console.WriteLine("Correlation, request, agent, API key, cache-control and Vary middleware configured.");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="d94ab6c85c5cec15eb7839bc"><a href="~/run-actions/d94ab6c85c5cec15eb7839bc/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/d94ab6c85c5cec15eb7839bc/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
