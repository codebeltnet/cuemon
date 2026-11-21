---
uid: Cuemon.AspNetCore.Mvc.Filters.Cacheable.HttpLastModifiedHeaderFilter
example:
- *content
---

The following example shows how <xref cref="Cuemon.AspNetCore.Mvc.Filters.Cacheable.HttpLastModifiedHeaderFilter"/> can be added to a cacheable pipeline and inspected directly.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Mvc.Filters.Cacheable;

var filter = new HttpLastModifiedHeaderFilter();
var options = new HttpCacheableOptions();
options.Filters.Add(filter);

Console.WriteLine(filter.Options.HasLastModifiedProvider);
Console.WriteLine(options.Filters.Count);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="64c3e2821ec0270032fb6fde"><a href="~/run-actions/64c3e2821ec0270032fb6fde/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/64c3e2821ec0270032fb6fde/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
