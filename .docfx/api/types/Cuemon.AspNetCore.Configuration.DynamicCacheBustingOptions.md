---
uid: Cuemon.AspNetCore.Configuration.DynamicCacheBustingOptions
example:
- *content
---

The following example demonstrates configuration options for dynamic cache busting.

```csharp
using System;
using Cuemon.Configuration;
using Microsoft.Extensions.Options;

        namespace Cuemon.AspNetCore.Configuration;

        public static class DynamicCacheBustingOptionsExample
        {
            public static void Demonstrate()
            {
                var options = new DynamicCacheBustingOptions
        {
            PreferredCasing = CasingMethod.UpperCase,
            PreferredLength = 6,
            TimeToLive = TimeSpan.FromMinutes(5)
        };

        var cacheBusting = new DynamicCacheBusting(Options.Create(options));
        Console.WriteLine(cacheBusting.Version);
            }
        }
```
