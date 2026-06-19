---
uid: Cuemon.Runtime.Caching.SlimMemoryCache
example:
- *content
---

The following example demonstrates how to use `SlimMemoryCache` to store and retrieve cached values with various expiration strategies.

```csharp
using System;
using Cuemon.Runtime.Caching;

namespace MyApp.Examples;

public class Example
{
    public void Run()
    {

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

        // Update an existing entry using the indexer
        cache["config"] = new { Theme = "Light", Locale = "en-US" };
        Console.WriteLine($"Updated config: {cache["config"]}");

        // Check if entry exists and remove it
        if (cache.Contains("config"))
        {
            cache.Remove("config");
            Console.WriteLine("Config removed.");

        // Count entries within a namespace
        cache.Add("item1", 100, DateTime.MaxValue, "data");
        cache.Add("item2", 200, DateTime.MaxValue, "data");
        Console.WriteLine($"Items in 'data' namespace: {cache.Count("data")}");

        // Remove all entries from a namespace
        cache.RemoveAll("data");
        Console.WriteLine($"After removal: {cache.Count("data")}");

        cache.Dispose();

}}}
}

```
