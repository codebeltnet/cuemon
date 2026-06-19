---
uid: Cuemon.Runtime.Caching.SlimMemoryCacheOptions
example:
- *content
---

The following example demonstrates how to configure <see cref="SlimMemoryCacheOptions" /> for sweep intervals and cache-key generation.

```csharp
using System;
using Cuemon.Runtime.Caching;

namespace MyApp.Examples;

public static class SlimMemoryCacheOptionsExample
{
    public static void Demonstrate()
    {
        var options = new SlimMemoryCacheOptions
        {
            EnableCleanup = false,
            FirstSweep = TimeSpan.FromSeconds(10),
            SucceedingSweep = TimeSpan.FromSeconds(30),
            KeyProvider = (key, ns) => key.Length + (ns == CacheEntry.NoScope ? 0 : ns.Length)
        };

        options.ValidateOptions();

        Console.WriteLine(options.EnableCleanup);
        Console.WriteLine(options.FirstSweep);
        Console.WriteLine(options.KeyProvider("session", "docs"));
    }
}
```
