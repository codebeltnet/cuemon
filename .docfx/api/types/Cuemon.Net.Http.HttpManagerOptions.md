---
uid: Cuemon.Net.Http.HttpManagerOptions
example:
- *content
---

The following example demonstrates how to configure <see cref="HttpManagerOptions" /> before passing them to <see cref="HttpManager" />.

```csharp
using System;
using System.Net.Http;
using Cuemon.Net.Http;

namespace MyApp.Examples;

public static class HttpManagerOptionsExample
{
    public static void Demonstrate()
    {
        var options = new HttpManagerOptions
        {
            Timeout = TimeSpan.FromSeconds(10),
            HandlerFactory = () => new HttpClientHandler(),
            DefaultRequestHeaders =
            {
                ["X-Correlation-Id"] = "docs-123"
            }
        };

        options.ValidateOptions();

        Console.WriteLine(options.Timeout.TotalSeconds);
        Console.WriteLine(options.DefaultRequestHeaders["X-Correlation-Id"]);
    }
}
```
