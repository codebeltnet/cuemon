---
uid: Cuemon.AspNetCore.Http.HttpResponseDecoratorExtensions
example:
- *content
---

The following example demonstrates how to add ETag and Last-Modified HTTP response headers using the <xref:Cuemon.AspNetCore.Http.HttpResponseDecoratorExtensions> class accessed through the <xref:Cuemon.Decorator> class.

```csharp
using System;
using System.Text;
using Cuemon;
using Cuemon.AspNetCore.Http;
using Cuemon.Data.Integrity;
using Cuemon.Security;
using Microsoft.AspNetCore.Http;

namespace MyApp.Examples;

public class HttpResponseDecoratorExtensionsExample
{
    public void AddCachingHeaders(HttpResponse response, HttpRequest request)
    {
        // Add an ETag header based on content integrity
        var builder = new ChecksumBuilder(() => new FowlerNollVo64());
        builder.CombineWith(Encoding.UTF8.GetBytes("content-data"));
        Decorator.Enclose(response).AddOrUpdateEntityTagHeader(request, builder);

        // Add a Last-Modified header
        var lastModified = new DateTime(2025, 6, 1, 12, 0, 0, DateTimeKind.Utc);
        Decorator.Enclose(response).AddOrUpdateLastModifiedHeader(request, lastModified);

}
}

```
