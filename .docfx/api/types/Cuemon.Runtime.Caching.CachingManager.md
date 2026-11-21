---
uid: Cuemon.Runtime.Caching.CachingManager
example:
- *content
---

The following example demonstrates how to get or add a cached value using `CachingManager.Cache`. It checks for an existing key, adds a value with a 5-minute expiration if not found, and verifies the cached instance is reused on subsequent accesses.

```csharp
// Program.cs
using System;
using Cuemon.Runtime.Caching;

var cache = CachingManager.Cache;
var key = "myKey";
var value = cache.Get(key);
if (value == null)
{
    cache.Add(key, DateTime.UtcNow, TimeSpan.FromMinutes(5));
}
value = cache.Get(key);
Console.WriteLine($"Cached value: {value}");

var sameValue = cache.Get(key);
Console.WriteLine($"Same instance? {value == sameValue}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="7137e1b3c53277027819065b"><a href="~/run-actions/7137e1b3c53277027819065b/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/7137e1b3c53277027819065b/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
