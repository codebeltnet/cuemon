---
uid: Cuemon.AspNetCore.Configuration.DynamicCacheBustingOptions
example:
- *content
---

The following example demonstrates configuration options for dynamic cache busting.

```csharp
// Program.cs
using System;
using Cuemon;
using Cuemon.Configuration;
using Microsoft.Extensions.Options;
using Cuemon.AspNetCore.Configuration;

        var options = new DynamicCacheBustingOptions
{
    PreferredCasing = CasingMethod.UpperCase,
    PreferredLength = 6,
    TimeToLive = TimeSpan.FromMinutes(5)
};

var cacheBusting = new DynamicCacheBusting(Options.Create(options));
Console.WriteLine(cacheBusting.Version);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="e2801b6a393c7bda6967dec0"><a href="~/run-actions/e2801b6a393c7bda6967dec0/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/e2801b6a393c7bda6967dec0/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
