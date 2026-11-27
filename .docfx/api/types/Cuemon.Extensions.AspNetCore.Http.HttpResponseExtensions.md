---
uid: Cuemon.Extensions.AspNetCore.Http.HttpResponseExtensions
example:
- *content
---

`HttpResponseExtensions` provides extension methods for `HttpResponse` to manage ETag and Last-Modified headers, write response bodies asynchronously, and transform `HttpResponseMessage` content into the ASP.NET Core response pipeline. This example hosts a minimal `WebApplication` endpoint that calls `AddOrUpdateEntityTagHeader` with a `ChecksumBuilder`, `AddOrUpdateLastModifiedHeader` with a `2024-06-15T10:00:00Z` UTC date, `OnStartingInvokeTransformer` to copy a trace header from an `HttpResponseMessage` when the response starts, and `WriteBodyAsync` to write `"Hello"`. An `HttpClient` request confirms the `ETag`, `Last-Modified`, and `X-Transferred` headers along with the response body.

```csharp
// Program.cs
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Cuemon.Data.Integrity;
using Cuemon.Extensions.AspNetCore.Http;
using Cuemon.Security;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder();
await using var app = builder.Build();
app.Urls.Add("http://127.0.0.1:0");

app.Run(async context =>
{
    var request = context.Request;
    var response = context.Response;

    var checksum = new ChecksumBuilder(() => HashFactory.CreateFnv128());
    checksum.CombineWith(Encoding.UTF8.GetBytes("Hello"));
    response.AddOrUpdateEntityTagHeader(request, checksum, isWeak: false);

    var lastModified = new DateTime(2024, 6, 15, 10, 0, 0, DateTimeKind.Utc);
    response.AddOrUpdateLastModifiedHeader(request, lastModified);

    using var message = new HttpResponseMessage(HttpStatusCode.OK);
    message.Headers.Add("X-Trace", "demo");
    response.OnStartingInvokeTransformer(message, (source, target) =>
    {
        target.Headers["X-Transferred"] = string.Join(string.Empty, source.Headers.GetValues("X-Trace"));
    });

    await response.WriteBodyAsync(() => new byte[] { 0x48, 0x65, 0x6C, 0x6C, 0x6F });
});

await app.StartAsync();
try
{
    using var client = new HttpClient { BaseAddress = new Uri(app.Urls.First()) };
    using var httpResponse = await client.GetAsync("/");
    Console.WriteLine(httpResponse.Headers.ETag?.ToString());
    Console.WriteLine(httpResponse.Content.Headers.LastModified?.ToString("R"));
    Console.WriteLine(string.Join(", ", httpResponse.Headers.GetValues("X-Transferred")));
    Console.WriteLine(await httpResponse.Content.ReadAsStringAsync());
}
finally
{
    await app.StopAsync();
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="5d949fddddc841f26493e5b5"><a href="~/run-actions/5d949fddddc841f26493e5b5/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/5d949fddddc841f26493e5b5/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
