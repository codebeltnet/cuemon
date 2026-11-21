---
uid: Cuemon.Extensions.AspNetCore.Mvc.Filters.FilterCollectionExtensions
example:
- *content
---

The following example demonstrates how to use the <xref:Cuemon.Extensions.AspNetCore.Mvc.Filters.FilterCollectionExtensions> extension methods to register common ASP.NET Core MVC filters.

```csharp
// Program.cs
using System;
using Cuemon.Extensions.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

var services = new ServiceCollection();
services.AddLogging();
services.AddControllers(options =>
{
    options.Filters.AddHttpCacheable();
    options.Filters.AddFaultDescriptor();
    options.Filters.AddServerTiming();
    options.Filters.AddUserAgentSentinel();
    options.Filters.AddThrottlingSentinel();
    options.Filters.AddApiKeySentinel();
});

using var provider = services.BuildServiceProvider();
var options = provider.GetRequiredService<IOptions<MvcOptions>>().Value;
Console.WriteLine(options.Filters.Count);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="76c72a695fb0fd1607163e3d"><a href="~/run-actions/76c72a695fb0fd1607163e3d/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/76c72a695fb0fd1607163e3d/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
