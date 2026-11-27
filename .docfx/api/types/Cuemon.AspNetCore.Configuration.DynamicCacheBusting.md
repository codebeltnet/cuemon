---
uid: Cuemon.AspNetCore.Configuration.DynamicCacheBusting
example:
- *content
---

The following example demonstrates how to use `DynamicCacheBusting` to resolve versioned static resource URLs at runtime.

```csharp
// Program.cs
using System;
using Cuemon;
using Cuemon.AspNetCore.Configuration;
using Microsoft.Extensions.Options;

var options = Options.Create(new DynamicCacheBustingOptions
{
    PreferredLength = 8,
    PreferredCharacters = Alphanumeric.LettersAndNumbers,
    TimeToLive = TimeSpan.FromHours(12)
});

var cacheBusting = new DynamicCacheBusting(options);

// Version is regenerated when the configured TimeToLive has elapsed.
string version = cacheBusting.Version;
Console.WriteLine(version);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="038bf45ed91c27c8a5b3ff04"><a href="~/run-actions/038bf45ed91c27c8a5b3ff04/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/038bf45ed91c27c8a5b3ff04/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
