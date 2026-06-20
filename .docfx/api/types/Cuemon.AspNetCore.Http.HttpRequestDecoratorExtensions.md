---
uid: Cuemon.AspNetCore.Http.HttpRequestDecoratorExtensions
example:
- *content
---

The following example demonstrates how to inspect HTTP request state using the <xref:Cuemon.AspNetCore.Http.HttpRequestDecoratorExtensions> class accessed through the <xref:Cuemon.Decorator> class.

```csharp
using System;
using Cuemon;
using Cuemon.Data.Integrity;
using Cuemon.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace Cuemon.AspNetCore.Http;

public static class HttpRequestDecoratorExtensionsExample
{
    public static void Demonstrate()
    {
        var context = new DefaultHttpContext();
        context.Request.Method = HttpMethods.Get;

        var builder = new ChecksumBuilder(() => HashFactory.CreateFnv128());
        context.Request.Headers[HeaderNames.IfNoneMatch] =
            string.Concat("\"", builder.Checksum.ToHexadecimalString(), "\"");

        var request = Decorator.Enclose(context.Request);
        var canServeFromCache = request.IsGetOrHeadMethod() && request.IsClientSideResourceCached(builder);

        Console.WriteLine(canServeFromCache);
    }
}
```
