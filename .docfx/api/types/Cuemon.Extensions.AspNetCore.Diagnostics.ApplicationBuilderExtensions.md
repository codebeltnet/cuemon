---
uid: Cuemon.Extensions.AspNetCore.Diagnostics.ApplicationBuilderExtensions
example:
- *content
---

The following example demonstrates how to add diagnostics middleware to an ASP.NET Core pipeline using the <xref:Cuemon.Extensions.AspNetCore.Diagnostics.ApplicationBuilderExtensions> class, then execute requests to observe the `Server-Timing` header and fault descriptor exception handling.

```csharp
// Program.cs
using System;
using System.Linq;
using System.Net.Http;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.AspNetCore.Http;
using Cuemon.Extensions.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    EnvironmentName = Environments.Development
});
builder.Services.AddLogging();
builder.Services.AddServerTiming();
builder.Services.AddFaultDescriptorOptions();

await using var app = builder.Build();
app.Urls.Add("http://127.0.0.1:0");

app.UseServerTiming();
app.UseFaultDescriptorExceptionHandler();
app.Run(async context =>
{
    if (context.Request.Path == "/error")
    {
        throw new NotFoundException("Resource was not found.");
    }

    context.RequestServices.GetRequiredService<IServerTiming>().AddServerTiming("db", TimeSpan.FromMilliseconds(12));
    await context.Response.WriteAsync("ok");
});

await app.StartAsync();
try
{
    using var client = new HttpClient { BaseAddress = new Uri(app.Urls.First()) };
    using var okResponse = await client.GetAsync("/ok");
    Console.WriteLine($"Server-Timing: {string.Join(", ", okResponse.Headers.GetValues(ServerTiming.HeaderName))}");

    using var errorResponse = await client.GetAsync("/error");
    Console.WriteLine($"Error status: {(int)errorResponse.StatusCode} {errorResponse.StatusCode}");
    Console.WriteLine($"Error body: {await errorResponse.Content.ReadAsStringAsync()}");
}
finally
{
    await app.StopAsync();
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="56f4ba307e3b51b7d8c2391b"><a href="~/run-actions/56f4ba307e3b51b7d8c2391b/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/56f4ba307e3b51b7d8c2391b/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
