---
uid: Cuemon.AspNetCore.Mvc.Filters.Cacheable.HttpCacheableFilter
example:
- *content
---

The following example shows how to compose <xref cref="Cuemon.AspNetCore.Mvc.Filters.Cacheable.HttpCacheableFilter"/> from packet-local options and cache validators.

```csharp
using System;
using Cuemon.AspNetCore.Mvc.Filters.Cacheable;
using Microsoft.Extensions.Options;

namespace MyApp.Examples;

public static class HttpCacheableFilterExample
{
    public static void Demonstrate()
    {
        var options = new HttpCacheableOptions();
        options.Filters.Add(new HttpEntityTagHeaderFilter(io => io.UseEntityTagResponseParser = true));
        options.Filters.Add(new HttpLastModifiedHeaderFilter());

        var filter = new HttpCacheableFilter(Options.Create(options));

        Console.WriteLine(filter.Options.Filters.Count);
        Console.WriteLine(filter.Options.UseCacheControl);
    }
}
```
