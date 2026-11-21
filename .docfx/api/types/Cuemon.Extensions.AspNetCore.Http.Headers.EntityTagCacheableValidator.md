---
uid: Cuemon.Extensions.AspNetCore.Http.Headers.EntityTagCacheableValidator
example:
- *content
---

The following example demonstrates how to register <xref cref="Cuemon.Extensions.AspNetCore.Http.Headers.EntityTagCacheableValidator"/> as a cacheable validator in the ASP.NET Core pipeline.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Http.Headers;
using Cuemon.Extensions.AspNetCore.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

var services = new ServiceCollection();
services.Configure<CacheableOptions>(options =>
{
    options.Validators.Add(new EntityTagCacheableValidator());
});

using var provider = services.BuildServiceProvider();
var options = provider.GetRequiredService<IOptions<CacheableOptions>>().Value;

Console.WriteLine(options.Validators.Count);
Console.WriteLine(options.Validators[0] is EntityTagCacheableValidator);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="00a579ead39a5f214ec75d09"><a href="~/run-actions/00a579ead39a5f214ec75d09/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/00a579ead39a5f214ec75d09/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
