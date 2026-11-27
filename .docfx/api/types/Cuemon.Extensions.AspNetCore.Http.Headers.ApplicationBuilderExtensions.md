---
uid: Cuemon.Extensions.AspNetCore.Http.Headers.ApplicationBuilderExtensions
example:
- *content
---

The following example demonstrates how to add correlation, request, validation, and cache headers to an ASP.NET Core request pipeline and invoke it to observe the resulting HTTP status and response headers.

```csharp
// Program.cs
using System;
using System.Linq;
using System.Net.Http;
using Cuemon.AspNetCore.Http.Headers;
using Cuemon.Extensions.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder();
await using var app = builder.Build();
app.Urls.Add("http://127.0.0.1:0");

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
app.Run(context => context.Response.WriteAsync("ok"));

await app.StartAsync();
try
{
    using var client = new HttpClient { BaseAddress = new Uri(app.Urls.First()) };
    client.DefaultRequestHeaders.UserAgent.ParseAdd("Cuemon-Agent");
    client.DefaultRequestHeaders.Add("X-Test-Key", "known-key");

    using var response = await client.GetAsync("/");
    Console.WriteLine($"Status: {(int)response.StatusCode}");
    Console.WriteLine($"Has X-Correlation-ID: {response.Headers.Contains("X-Correlation-ID")}");
    Console.WriteLine($"Has X-Request-ID: {response.Headers.Contains("X-Request-ID")}");
    Console.WriteLine($"Cache-Control: {response.Headers.CacheControl}");
    Console.WriteLine($"Has Expires: {response.Content.Headers.Expires.HasValue}");
    Console.WriteLine($"Vary: {string.Join(", ", response.Headers.Vary)}");
}
finally
{
    await app.StopAsync();
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="f4f3396aa15690f231f724a8"><a href="~/run-actions/f4f3396aa15690f231f724a8/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/f4f3396aa15690f231f724a8/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
