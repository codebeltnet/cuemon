---
uid: Cuemon.Extensions.AspNetCore.Hosting.ApplicationBuilderExtensions
example:
- *content
---

The following example demonstrates how to add the hosting environment HTTP header middleware to an ASP.NET Core application pipeline and invoke it to observe the configured `X-Environment` response header.

```csharp
// Program.cs
using System;
using System.Linq;
using System.Net.Http;
using Cuemon.Extensions.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    EnvironmentName = Environments.Development
});
await using var app = builder.Build();
app.Urls.Add("http://127.0.0.1:0");

app.UseHostingEnvironment(options =>
{
    options.HeaderName = "X-Environment";
});
app.Run(context => context.Response.WriteAsync("ok"));

await app.StartAsync();
try
{
    using var client = new HttpClient { BaseAddress = new Uri(app.Urls.First()) };
    using var response = await client.GetAsync("/");
    Console.WriteLine($"X-Environment: {string.Join(", ", response.Headers.GetValues("X-Environment"))}");
}
finally
{
    await app.StopAsync();
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="16b52fc6fbe590f6a9b6a7dd"><a href="~/run-actions/16b52fc6fbe590f6a9b6a7dd/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/16b52fc6fbe590f6a9b6a7dd/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
