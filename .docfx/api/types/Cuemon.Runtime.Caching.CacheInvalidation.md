---
uid: Cuemon.Runtime.Caching.CacheInvalidation
example:
- *content
---

The following example demonstrates how to use <see cref="CacheInvalidation"/> to define eviction policies for cache entries.

```csharp
// Program.cs
using System;
using Cuemon.Runtime.Caching; // for CacheInvalidation

// Absolute expiration at a specific UTC time
var absolute = new CacheInvalidation(new DateTime(2025, 12, 31, 23, 59, 59, DateTimeKind.Utc));
Console.WriteLine(absolute.UseAbsoluteExpiration); // True
Console.WriteLine(absolute.AbsoluteExpiration);    // 12/31/2025 23:59:59
Console.WriteLine(absolute.UseSlidingExpiration);  // False
Console.WriteLine(absolute.UseDependency);         // False

// Sliding expiration (entry expires after 30 minutes of inactivity)
var sliding = new CacheInvalidation(TimeSpan.FromMinutes(30));
Console.WriteLine(sliding.UseSlidingExpiration); // True
Console.WriteLine(sliding.SlidingExpiration);    // 00:30:00
Console.WriteLine(sliding.UseAbsoluteExpiration); // False
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="4123c6922ad7b0b26087495b"><a href="~/run-actions/4123c6922ad7b0b26087495b/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/4123c6922ad7b0b26087495b/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
