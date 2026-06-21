---
uid: Cuemon.AspNetCore.Mvc.Filters.Cacheable.HttpLastModifiedHeaderOptions
example:
- *content
---

The following example applies the default <xref cref="Cuemon.AspNetCore.Mvc.Filters.Cacheable.HttpLastModifiedHeaderOptions.LastModifiedProvider"/> to a timestamped response model.

```csharp
using System;
using Cuemon.AspNetCore.Mvc.Filters.Cacheable;
using Cuemon.Data.Integrity;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace MyApp.Examples;

public static class HttpLastModifiedHeaderOptionsExample
{
    public static void Demonstrate()
    {
        var options = new HttpLastModifiedHeaderOptions();
        var context = new DefaultHttpContext();
        context.Request.Method = HttpMethods.Get;

        var timestamp = new SampleEntityDataTimestamp(
            DateTime.Parse("2024-01-01T00:00:00Z"),
            DateTime.Parse("2024-01-02T00:00:00Z"));

        options.LastModifiedProvider(timestamp, context);

        Console.WriteLine(options.HasLastModifiedProvider);
        Console.WriteLine(context.Response.Headers[HeaderNames.LastModified].ToString());
    }

    private sealed class SampleEntityDataTimestamp : IEntityDataTimestamp
    {
        public SampleEntityDataTimestamp(DateTime created, DateTime? modified)
        {
            Created = created;
            Modified = modified;
        }

        public DateTime Created { get; }

        public DateTime? Modified { get; }
    }
}
```
