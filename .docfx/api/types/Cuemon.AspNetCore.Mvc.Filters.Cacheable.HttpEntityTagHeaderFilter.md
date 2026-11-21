---
uid: Cuemon.AspNetCore.Mvc.Filters.Cacheable.HttpEntityTagHeaderFilter
example:
- *content
---

The following example shows how <xref cref="Cuemon.AspNetCore.Mvc.Filters.Cacheable.HttpEntityTagHeaderFilter"/> is added to a cacheable filter pipeline and configured to fall back to parsing the response body.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Mvc.Filters.Cacheable;

var filter = new HttpEntityTagHeaderFilter(o => o.UseEntityTagResponseParser = true);
var options = new HttpCacheableOptions();
options.Filters.Add(filter);

Console.WriteLine(filter.Options.UseEntityTagResponseParser);
Console.WriteLine(options.Filters.Count);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="40fe571a68ed6d1303ff9d33"><a href="~/run-actions/40fe571a68ed6d1303ff9d33/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/40fe571a68ed6d1303ff9d33/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
