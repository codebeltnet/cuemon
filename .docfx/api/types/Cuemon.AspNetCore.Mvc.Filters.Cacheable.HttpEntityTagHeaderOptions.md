---
uid: Cuemon.AspNetCore.Mvc.Filters.Cacheable.HttpEntityTagHeaderOptions
example:
- *content
---

The following example uses <xref cref="Cuemon.AspNetCore.Mvc.Filters.Cacheable.HttpEntityTagHeaderOptions"/> the same way the unit tests exercise its default delegates: by applying an ETag from entity integrity data and then from a response-body fallback parser.

```csharp
using System;
using System.IO;
using System.Text;
using Cuemon.AspNetCore.Mvc.Filters.Cacheable;
using Cuemon.Data.Integrity;
using Cuemon.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace MyApp.Examples;

public static class HttpEntityTagHeaderOptionsExample
{
    public static void Demonstrate()
    {
        var options = new HttpEntityTagHeaderOptions
        {
            UseEntityTagResponseParser = true
        };

        var context = new DefaultHttpContext();
        context.Request.Method = HttpMethods.Get;

        options.EntityTagProvider(new SampleEntityDataIntegrity(), context);

        using var body = new MemoryStream(Encoding.UTF8.GetBytes("payload"));
        options.EntityTagResponseParser(body, context.Request, context.Response);

        Console.WriteLine(options.HasEntityTagProvider);
        Console.WriteLine(options.HasEntityTagResponseParser);
        Console.WriteLine(context.Response.Headers.ContainsKey(HeaderNames.ETag));
    }

    private sealed class SampleEntityDataIntegrity : IEntityDataIntegrity
    {
        public HashResult Checksum => new HashResult(new byte[] { 1, 2, 3 });

        public EntityDataIntegrityValidation Validation => EntityDataIntegrityValidation.Strong;
    }
}
```
