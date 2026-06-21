---
uid: Cuemon.AspNetCore.Mvc.Filters.Cacheable.HttpLastModifiedHeaderFilter
example:
- *content
---

The following example shows how <xref cref="Cuemon.AspNetCore.Mvc.Filters.Cacheable.HttpLastModifiedHeaderFilter"/> can be added to a cacheable pipeline and inspected directly.

```csharp
using System;
using Cuemon.AspNetCore.Mvc.Filters.Cacheable;
namespace MyApp.Examples;

public static class HttpLastModifiedHeaderFilterExample
{
    public static void Demonstrate()
    {
        var filter = new HttpLastModifiedHeaderFilter();
        var options = new HttpCacheableOptions();
        options.Filters.Add(filter);

        Console.WriteLine(filter.Options.HasLastModifiedProvider);
        Console.WriteLine(options.Filters.Count);
    }
}
```
