---
uid: Cuemon.AspNetCore.Mvc.Filters.Cacheable.HttpEntityTagHeaderFilter
example:
- *content
---

The following example shows how <xref cref="Cuemon.AspNetCore.Mvc.Filters.Cacheable.HttpEntityTagHeaderFilter"/> is added to a cacheable filter pipeline and configured to fall back to parsing the response body.

```csharp
using System;
using Cuemon.AspNetCore.Mvc.Filters.Cacheable;
namespace MyApp.Examples;

public static class HttpEntityTagHeaderFilterExample
{
    public static void Demonstrate()
    {
        var filter = new HttpEntityTagHeaderFilter(o => o.UseEntityTagResponseParser = true);
        var options = new HttpCacheableOptions();
        options.Filters.Add(filter);

        Console.WriteLine(filter.Options.UseEntityTagResponseParser);
        Console.WriteLine(options.Filters.Count);
    }
}
```
