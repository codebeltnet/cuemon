---
uid: Cuemon.Extensions.AspNetCore.Mvc.Filters.Cacheable.CacheableAsyncResultFilterExtensions
example:
- *content
---

The following example configures the same cache-filter collection patterns exercised by the unit tests: use the convenience methods for the default pipeline, or switch to the generic overloads when you need explicit ordering and option control.

```csharp
using System;
using Cuemon.AspNetCore.Mvc.Filters.Cacheable;
using Cuemon.Extensions.AspNetCore.Mvc.Filters.Cacheable;

namespace Cuemon.Extensions.AspNetCore.Mvc.Filters.Cacheable.DocExamples;

public sealed class CacheableAsyncResultFilterExtensionsExample
{
    public HttpCacheableOptions CreateDefaultProfile()
    {
        var options = new HttpCacheableOptions();
        options.Filters.AddLastModifiedHeader();
        options.Filters.AddEntityTagHeader();
        return options;
    }

    public HttpCacheableOptions CreateCustomProfile()
    {
        var options = new HttpCacheableOptions();
        options.Filters.AddFilter<HttpEntityTagHeaderFilter, HttpEntityTagHeaderOptions>(entityTag =>
        {
            entityTag.UseEntityTagResponseParser = true;
        });
        options.Filters.InsertFilter<HttpLastModifiedHeaderFilter>(0);
        return options;
    }

    public void Describe()
    {
        var defaultProfile = CreateDefaultProfile();
        var customProfile = CreateCustomProfile();

        Console.WriteLine($"{defaultProfile.Filters.Count} default filters, {customProfile.Filters.Count} custom filters.");
    }
}
```
