---
uid: Cuemon.Threading.AsyncRunOptions
example:
- *content
---

The following example demonstrates how to use <see cref="AsyncRunOptions"/> to configure timeout and retry delay for an asynchronous operation.

```csharp
using System;
using System.Threading;
using System.Threading.Tasks;
using Cuemon.Threading; // for AsyncRunOptions

namespace MyApp.Examples;

public class AsyncRunOptionsExample
{
    public async Task DemonstrateAsync()
    {
        var options = new AsyncRunOptions
        {
            Timeout = TimeSpan.FromSeconds(30),
            Delay = TimeSpan.FromMilliseconds(500)
        };
        Console.WriteLine(options.Timeout); // 00:00:30
        Console.WriteLine(options.Delay);   // 00:00:00.5000000

        // Use with cancellation support
        var withCancellation = new AsyncRunOptions
        {
            Timeout = TimeSpan.FromSeconds(10),
            CancellationToken = new CancellationTokenSource(5000).Token
        };

        // Defaults: timeout 5s, delay 100ms
        var defaults = new AsyncRunOptions();
        Console.WriteLine(defaults.Timeout); // 00:00:05
        Console.WriteLine(defaults.Delay);   // 00:00:00.1000000

}
}

```
