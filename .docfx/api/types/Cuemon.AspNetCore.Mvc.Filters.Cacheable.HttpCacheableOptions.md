---
uid: Cuemon.AspNetCore.Mvc.Filters.Cacheable.HttpCacheableOptions
example:
- *content
---

The following example configures <xref cref="Cuemon.AspNetCore.Mvc.Filters.Cacheable.HttpCacheableOptions"/> with a custom `Cache-Control` header and manually adds both cacheable filters used by <xref cref="Cuemon.AspNetCore.Mvc.Filters.Cacheable.HttpCacheableFilter"/>.

```csharp
using System;
using Cuemon.AspNetCore.Mvc.Filters.Cacheable;
using Microsoft.Net.Http.Headers;

namespace MyApp.Examples;

public static class HttpCacheableOptionsExample
{
    public static void Demonstrate()
    {
        var options = new HttpCacheableOptions
        {
            CacheControl = new CacheControlHeaderValue
            {
                MaxAge = TimeSpan.FromMinutes(15),
                Public = true,
                MustRevalidate = false
            }
        };

        options.Filters.Add(new HttpEntityTagHeaderFilter(o => o.UseEntityTagResponseParser = true));
        options.Filters.Add(new HttpLastModifiedHeaderFilter());

        Console.WriteLine(options.UseCacheControl);
        Console.WriteLine(options.Filters.Count);
        Console.WriteLine(options.CacheControl.MaxAge);
    }
}
```
