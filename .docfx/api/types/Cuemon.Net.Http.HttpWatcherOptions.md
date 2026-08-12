---
uid: Cuemon.Net.Http.HttpWatcherOptions
example:
- *content
---

The following example demonstrates how to configure <see cref="HttpWatcherOptions" /> before creating an <see cref="HttpWatcher" />.

```csharp
using System;
using System.Net.Http;
using Cuemon.Net.Http;
using Cuemon.Security;

namespace MyApp.Examples;

public static class HttpWatcherOptionsExample
{
    public static void Demonstrate()
    {
        var options = new HttpWatcherOptions
        {
            ClientFactory = () => new HttpClient(new HttpClientHandler(), false),
            HashFactory = () => new CyclicRedundancyCheck64(),
            ReadResponseBody = true,
            Period = TimeSpan.FromSeconds(5)
        };

        options.ValidateOptions();

        Console.WriteLine(options.ReadResponseBody);
        Console.WriteLine(options.Period.TotalSeconds);
        Console.WriteLine($"HTTP client factory configured: {options.ClientFactory is not null}");
    }
}
```
