---
uid: Cuemon.AspNetCore.Mvc.Filters.Cacheable.HttpCacheableFilter
example:
- *content
---

The following example shows how to compose <xref cref="Cuemon.AspNetCore.Mvc.Filters.Cacheable.HttpCacheableFilter"/> from packet-local options and cache validators.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Mvc.Filters.Cacheable;
using Microsoft.Extensions.Options;

var options = new HttpCacheableOptions();
options.Filters.Add(new HttpEntityTagHeaderFilter(io => io.UseEntityTagResponseParser = true));
options.Filters.Add(new HttpLastModifiedHeaderFilter());

var filter = new HttpCacheableFilter(Options.Create(options));

Console.WriteLine(filter.Options.Filters.Count);
Console.WriteLine(filter.Options.UseCacheControl);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="4ccbaf005cae866a5640e9a9"><a href="~/run-actions/4ccbaf005cae866a5640e9a9/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/4ccbaf005cae866a5640e9a9/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
