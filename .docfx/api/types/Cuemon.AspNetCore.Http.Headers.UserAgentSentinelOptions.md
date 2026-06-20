---
uid: Cuemon.AspNetCore.Http.Headers.UserAgentSentinelOptions
example:
- *content
---

The following example demonstrates how to configure <xref cref="Cuemon.AspNetCore.Http.Headers.UserAgentSentinelOptions"/> to restrict which User-Agent headers are allowed.

```csharp
using System;
using System.Collections.Generic;

        namespace Cuemon.AspNetCore.Http.Headers;

        public static class UserAgentSentinelOptionsExample
        {
            public static void Demonstrate()
            {
                var options = new UserAgentSentinelOptions
        {
            RequireUserAgentHeader = true,
            ValidateUserAgentHeader = true,
            AllowedUserAgents = new List<string> { "Cuemon-Agent" }
        };

        options.ValidateOptions();
        using var response = options.ResponseHandler("Unknown-Agent");
        var message = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        Console.WriteLine(message);
            }
        }
```
