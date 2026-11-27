---
uid: Cuemon.AspNetCore.Configuration.CacheBustingOptions
example:
- *content
---

The following example demonstrates how to configure cache-busting options for static resources.

```csharp
// Program.cs
using System;
using Cuemon;
using Cuemon.Configuration;
using Cuemon.AspNetCore.Configuration;

        var options = new CacheBustingOptions
{
    PreferredCasing = CasingMethod.UpperCase
};

Console.WriteLine(options.PreferredCasing);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="d171c7f0056936b799e08a85"><a href="~/run-actions/d171c7f0056936b799e08a85/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/d171c7f0056936b799e08a85/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
