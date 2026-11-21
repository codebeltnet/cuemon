---
uid: Cuemon.Extensions.AspNetCore.Http.HttpResponseExtensions
example:
- *content
---

`HttpResponseExtensions` provides extension methods for `HttpResponse` to manage ETag and Last-Modified headers, write response bodies asynchronously, and transform `HttpResponseMessage` content into the ASP.NET Core response pipeline. This example calls `AddOrUpdateEntityTagHeader` with a `ChecksumBuilder` for ETag generation, `AddOrUpdateLastModifiedHeader` with a `2024-06-15T10:00:00Z` UTC date, `WriteBodyAsync` with a byte array delegate, and `OnStartingInvokeTransformer` to register a start callback that copies a trace header from an `HttpResponseMessage` with status `200 OK`. Each operation is demonstrated independently, with Console output confirming the header values and response modifications.

```csharp
// Program.cs
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Cuemon.Data.Integrity;
using Cuemon.Extensions.AspNetCore.Http;
using Cuemon.Security;
using Microsoft.AspNetCore.Http;

var context = new DefaultHttpContext();
var request = context.Request;
var response = context.Response;
response.Body = new MemoryStream();

var builder = new ChecksumBuilder(() => HashFactory.CreateFnv128());
builder.CombineWith(Encoding.UTF8.GetBytes("Hello"));
response.AddOrUpdateEntityTagHeader(request, builder, isWeak: false);

var lastModified = new DateTime(2024, 6, 15, 10, 0, 0, DateTimeKind.Utc);
response.AddOrUpdateLastModifiedHeader(request, lastModified);

using var message = new HttpResponseMessage(HttpStatusCode.OK);
message.Headers.Add("X-Trace", "demo");
response.OnStartingInvokeTransformer(message, (source, target) =>
{
    target.Headers["X-Transferred"] = string.Join(string.Empty, source.Headers.GetValues("X-Trace"));
});

await response.WriteBodyAsync(() => new byte[] { 0x48, 0x65, 0x6C, 0x6C, 0x6F });
await response.StartAsync();

Console.WriteLine(response.Headers.ETag.ToString());
Console.WriteLine(response.Headers.LastModified.ToString());
Console.WriteLine(response.Headers["X-Transferred"].ToString());
response.Body.Position = 0;
using var reader = new StreamReader(response.Body);
Console.WriteLine(await reader.ReadToEndAsync());
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="83b52d978d24338c0c1eab2e"><a href="~/run-actions/83b52d978d24338c0c1eab2e/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/83b52d978d24338c0c1eab2e/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
