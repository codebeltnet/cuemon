---
uid: Cuemon.AspNetCore.Mvc.Filters.Throttling.ThrottlingSentinelFilter
example:
- *content
---

The following example constructs <xref cref="Cuemon.AspNetCore.Mvc.Filters.Throttling.ThrottlingSentinelFilter"/> directly with a packet-local throttling cache and options object.

```csharp
// Program.cs
using System;
using Cuemon;
using Cuemon.AspNetCore.Http.Throttling;
using Cuemon.AspNetCore.Mvc.Filters.Throttling;
using Microsoft.Extensions.Options;

var options = Options.Create(new ThrottlingSentinelOptions
{
    ContextResolver = _ => "developer-workstation",
    Quota = new ThrottleQuota(10, 5, TimeUnit.Seconds)
});

var filter = new ThrottlingSentinelFilter(options, new MemoryThrottlingCache());

Console.WriteLine(filter.Options.Quota.RateLimit);
Console.WriteLine(filter.Options.UseRetryAfterHeader);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="1d56ab970eff694bc48ec6bf"><a href="~/run-actions/1d56ab970eff694bc48ec6bf/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/1d56ab970eff694bc48ec6bf/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
