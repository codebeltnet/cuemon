---
uid: Cuemon.AspNetCore.Configuration.CacheBustingOptions
example:
- *content
---

The following example demonstrates how to configure cache-busting options for static resources.

```csharp
using System;
using Cuemon.Configuration;

        namespace Cuemon.AspNetCore.Configuration;

        public static class CacheBustingOptionsExample
        {
            public static void Demonstrate()
            {
                var options = new CacheBustingOptions
        {
            PreferredCasing = CasingMethod.UpperCase
        };

        Console.WriteLine(options.PreferredCasing);
            }
        }
```
