---
uid: Cuemon.AspNetCore.Http.Headers.CacheableOptions
example:
- *content
---

```csharp
using System;
using Microsoft.Net.Http.Headers;

        namespace Cuemon.AspNetCore.Http.Headers;

        public static class CacheableOptionsExample
        {
            public static void Demonstrate()
            {
                var options = new CacheableOptions
        {
            CacheControl = new CacheControlHeaderValue
            {
                Public = true,
                MaxAge = TimeSpan.FromHours(12)
            },
            Expires = new ExpiresHeaderValue(TimeSpan.FromHours(12))
        };

        options.ValidateOptions();
        Console.WriteLine($"{options.UseCacheControl}:{options.UseExpires}");
            }
        }
```
