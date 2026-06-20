---
uid: Cuemon.Extensions.AspNetCore.Http.HttpResponseExtensions
example:
- *content
---

`HttpResponseExtensions` provides extension methods for `HttpResponse` to manage ETag and Last-Modified headers, write response bodies asynchronously, and transform `HttpResponseMessage` content into the ASP.NET Core response pipeline. This example calls `AddOrUpdateEntityTagHeader` with a `ChecksumBuilder` for ETag generation, `AddOrUpdateLastModifiedHeader` with a `2024-06-15T10:00:00Z` UTC date, `WriteBodyAsync` with a byte array delegate, and `OnStartingInvokeTransformer` to transfer an `HttpResponseMessage` with status `200 OK` and JSON content type. Each operation is demonstrated independently, with Console output confirming the header values and response modifications.

```csharp
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Cuemon.Data.Integrity;
using Cuemon.Extensions.AspNetCore.Http;
using Cuemon.Security;
using Microsoft.AspNetCore.Http;

namespace MyApp.AspNetCore.Http
{
    public class HttpResponseExtensionsExample
    {
        public async Task DemonstrateAsync(HttpResponse response, HttpRequest request)
        {
            // Add or update an ETag header using a checksum builder
            var builder = new ChecksumBuilder(() => HashFactory.CreateFnv128());
            response.AddOrUpdateEntityTagHeader(request, builder, isWeak: false);
            // Response now has ETag: "..." header

            // Add or update a Last-Modified header with a specific date
            var lastModified = new DateTime(2024, 6, 15, 10, 0, 0, DateTimeKind.Utc);
            response.AddOrUpdateLastModifiedHeader(request, lastModified);
            // Response now has Last-Modified: "Sat, 15 Jun 2024 10:00:00 GMT" header

            // Write bytes to the response body from a delegate
            await response.WriteBodyAsync(() => new byte[] { 0x48, 0x65, 0x6C, 0x6C, 0x6F });
            // Response body now contains "Hello"

            // Transfer a HttpResponseMessage to the response pipeline
            var message = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"status\":\"ok\"}")
            };
            response.OnStartingInvokeTransformer(message, (msg, resp) =>
            {
                resp.StatusCode = (int)msg.StatusCode;
                resp.Headers["Content-Type"] = "application/json";
            });
            // The response will send the transformed content when the pipeline starts

}}
}

```
