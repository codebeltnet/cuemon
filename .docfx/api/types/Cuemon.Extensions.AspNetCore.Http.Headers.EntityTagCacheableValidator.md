---
uid: Cuemon.Extensions.AspNetCore.Http.Headers.EntityTagCacheableValidator
example:
- *content
---

The following example demonstrates how to register <xref cref="Cuemon.Extensions.AspNetCore.Http.Headers.EntityTagCacheableValidator"/> in `CacheableOptions` and invoke `ProcessAsync` to compute and attach an `ETag` header on the HTTP response.

```csharp
// Program.cs
using System;
using System.IO;
using System.Text;
using Cuemon.AspNetCore.Http.Headers;
using Cuemon.Extensions.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

var services = new ServiceCollection();
services.Configure<CacheableOptions>(options =>
{
    options.Validators.Add(new EntityTagCacheableValidator());
});

using var provider = services.BuildServiceProvider();
var options = provider.GetRequiredService<IOptions<CacheableOptions>>().Value;

var context = new DefaultHttpContext { RequestServices = provider };
using var body = new MemoryStream(Encoding.UTF8.GetBytes("Hello world!"));
await options.Validators[0].ProcessAsync(context, body);

Console.WriteLine(options.Validators.Count);
Console.WriteLine(options.Validators[0] is EntityTagCacheableValidator);
Console.WriteLine($"ETag: {context.Response.Headers[HeaderNames.ETag]}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="cc1b6df0db90088c67381491"><a href="~/run-actions/cc1b6df0db90088c67381491/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/cc1b6df0db90088c67381491/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
