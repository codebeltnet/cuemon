---
uid: Cuemon.Runtime.Caching.SlimMemoryCache
example:
- *content
---

`SlimMemoryCache` provides in-memory caching with absolute and sliding expiration, namespace grouping, and automatic cleanup. This example creates a cache with cleanup enabled (`FirstSweep = 30s`, `SucceedingSweep = 10s`), adds a config entry with absolute expiration, a session entry with sliding expiration under the `"sessions"` namespace, and updates the config entry via the indexer. It demonstrates safe retrieval with `TryGet`, key-based removal with `Remove`, namespace-based counting with `Count("data")`, and bulk namespace removal with `RemoveAll("data")`. Console output confirms the cached values, update status, removal success, and namespace counts before and after removal.

```csharp
// Program.cs
using System;
using Cuemon.Runtime.Caching;

        // Create a cache with automatic cleanup
        var cache = new SlimMemoryCache(o =>
        {
            o.EnableCleanup = true;
            o.FirstSweep = TimeSpan.FromSeconds(30);
            o.SucceedingSweep = TimeSpan.FromSeconds(10);
        });

        // Add an entry with absolute expiration
        cache.Add("config", new { Theme = "Dark", Locale = "en-US" }, DateTime.UtcNow.AddMinutes(5));

        // Add an entry with sliding expiration (resets on each access)
        cache.Add("session", new { UserId = 42, Role = "Admin" }, TimeSpan.FromMinutes(20), "sessions");

        Console.WriteLine($"Config: {cache["config"]}");

        // Use TryGet for safe retrieval
        if (cache.TryGet("session", "sessions", out var session))
        {
            Console.WriteLine($"Session: {session}");
        }

        // Update an existing entry using the indexer
        cache["config"] = new { Theme = "Light", Locale = "en-US" };
        Console.WriteLine($"Updated config: {cache["config"]}");

        // Check if entry exists and remove it
        if (cache.Contains("config"))
        {
            cache.Remove("config");
            Console.WriteLine("Config removed.");
        }

        // Count entries within a namespace
        cache.Add("item1", 100, DateTime.MaxValue, "data");
        cache.Add("item2", 200, DateTime.MaxValue, "data");
        Console.WriteLine($"Items in 'data' namespace: {cache.Count("data")}");

        // Remove all entries from a namespace
        cache.RemoveAll("data");
        Console.WriteLine($"After removal: {cache.Count("data")}");

        cache.Dispose();
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="a1af386558a6a097c106aa3e"><a href="~/run-actions/a1af386558a6a097c106aa3e/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/a1af386558a6a097c106aa3e/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
