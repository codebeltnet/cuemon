---
uid: Cuemon.AspNetCore.Mvc.Filters.Headers.UserAgentSentinelFilter
example:
- *content
---

The following example configures <xref cref="Cuemon.AspNetCore.Mvc.Filters.Headers.UserAgentSentinelFilter"/> to require a known `User-Agent` header and then creates the filter directly from those options.

```csharp
using System;
using Cuemon.AspNetCore.Http.Headers;
using Cuemon.AspNetCore.Mvc.Filters.Headers;
using Microsoft.Extensions.Options;

namespace MyApp.Examples;

public static class UserAgentSentinelFilterExample
{
    public static void Demonstrate()
    {
        var options = Options.Create(new UserAgentSentinelOptions
        {
            RequireUserAgentHeader = true,
            ValidateUserAgentHeader = true
        });
        options.Value.AllowedUserAgents.Add("Cuemon-Agent");

        var filter = new UserAgentSentinelFilter(options);

        Console.WriteLine(filter.Options.RequireUserAgentHeader);
        Console.WriteLine(filter.Options.AllowedUserAgents.Count);
    }
}
```
