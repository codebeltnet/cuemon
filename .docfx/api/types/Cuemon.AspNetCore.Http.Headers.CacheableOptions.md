---
uid: Cuemon.AspNetCore.Http.Headers.CacheableOptions
example:
- *content
---

The following example shows how to configure `CacheableOptions` with cache-control and expiration headers. After validation, it prints whether each header type is enabled.

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
