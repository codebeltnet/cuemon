---
uid: Cuemon.Net.Http.HttpWatcher
example:
- *content
---

The following example demonstrates how to configure an <see cref="HttpWatcher" /> for a remote URI and inspect its active monitoring settings.

```csharp
using System;
using Cuemon.Net.Http;

namespace MyApp.Examples;

public static class HttpWatcherExample
{
    public static void Demonstrate()
    {
        var watcher = new HttpWatcher(new Uri("https://example.com/feed"), options =>
        {
            options.ReadResponseBody = true;
            options.Period = TimeSpan.FromSeconds(10);
        });

        Console.WriteLine(watcher.Location);
        Console.WriteLine(watcher.ReadResponseBody);
        Console.WriteLine(watcher.HashFactory != null);
    }
}
```
