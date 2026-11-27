---
uid: Cuemon.AspNetCore.Hosting.HostingEnvironmentOptions
example:
- *content
---

The following example demonstrates how to configure hosting environment options.

```csharp
// Program.cs
using System;
using Microsoft.Extensions.Hosting;
using Cuemon.AspNetCore.Hosting;

        var options = new HostingEnvironmentOptions
{
    HeaderName = "X-Environment",
    SuppressHeaderPredicate = environment => environment.IsProduction()
};

options.ValidateOptions();
Console.WriteLine(options.HeaderName);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="0589060aa19ff1221c8e4938"><a href="~/run-actions/0589060aa19ff1221c8e4938/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/0589060aa19ff1221c8e4938/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
