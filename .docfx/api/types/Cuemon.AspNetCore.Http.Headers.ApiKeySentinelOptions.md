---
uid: Cuemon.AspNetCore.Http.Headers.ApiKeySentinelOptions
example:
- *content
---

The following example demonstrates configuring API key sentinel options.

```csharp
using System;
using System.Collections.Generic;

        namespace Cuemon.AspNetCore.Http.Headers;

        public static class ApiKeySentinelOptionsExample
        {
            public static void Demonstrate()
            {
                var options = new ApiKeySentinelOptions
        {
            AllowedKeys = new List<string> { "secret-key" }
        };

        options.ValidateOptions();
        using var response = options.ResponseHandler("wrong-key");
        var message = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        Console.WriteLine(message);
            }
        }
```
