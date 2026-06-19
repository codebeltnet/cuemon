---
uid: Cuemon.AspNetCore.Hosting.HostingEnvironmentOptions
example:
- *content
---

The following example demonstrates how to configure hosting environment options.

```csharp
using System;
using Microsoft.Extensions.Hosting;

        namespace Cuemon.AspNetCore.Hosting;

        public static class HostingEnvironmentOptionsExample
        {
            public static void Demonstrate()
            {
                var options = new HostingEnvironmentOptions
        {
            HeaderName = "X-Environment",
            SuppressHeaderPredicate = environment => environment.IsProduction()
        };

        options.ValidateOptions();
        Console.WriteLine(options.HeaderName);
            }
        }
```
