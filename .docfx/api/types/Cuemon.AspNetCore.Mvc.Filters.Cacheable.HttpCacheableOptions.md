---
uid: Cuemon.AspNetCore.Mvc.Filters.Cacheable.HttpCacheableOptions
example:
- *content
---

The following example configures <xref cref="Cuemon.AspNetCore.Mvc.Filters.Cacheable.HttpCacheableOptions"/> with a custom `Cache-Control` header and manually adds both cacheable filters used by <xref cref="Cuemon.AspNetCore.Mvc.Filters.Cacheable.HttpCacheableFilter"/>.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Mvc.Filters.Cacheable;
using Microsoft.Net.Http.Headers;

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
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="441806c905cfc583ffaa10bf"><a href="~/run-actions/441806c905cfc583ffaa10bf/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/441806c905cfc583ffaa10bf/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
