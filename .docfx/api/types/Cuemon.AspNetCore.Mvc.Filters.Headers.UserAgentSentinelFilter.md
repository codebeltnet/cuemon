---
uid: Cuemon.AspNetCore.Mvc.Filters.Headers.UserAgentSentinelFilter
example:
- *content
---

The following example configures <xref cref="Cuemon.AspNetCore.Mvc.Filters.Headers.UserAgentSentinelFilter"/> to require a known `User-Agent` header and then creates the filter directly from those options.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Http.Headers;
using Cuemon.AspNetCore.Mvc.Filters.Headers;
using Microsoft.Extensions.Options;

var options = Options.Create(new UserAgentSentinelOptions
{
    RequireUserAgentHeader = true,
    ValidateUserAgentHeader = true
});
options.Value.AllowedUserAgents.Add("Cuemon-Agent");

var filter = new UserAgentSentinelFilter(options);

Console.WriteLine(filter.Options.RequireUserAgentHeader);
Console.WriteLine(filter.Options.AllowedUserAgents.Count);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="640e025c7e4424e39f1b7268"><a href="~/run-actions/640e025c7e4424e39f1b7268/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/640e025c7e4424e39f1b7268/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
