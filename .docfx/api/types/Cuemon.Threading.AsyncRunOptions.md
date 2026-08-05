---
uid: Cuemon.Threading.AsyncRunOptions
example:
- *content
---

The following example demonstrates how to use <see cref="AsyncRunOptions"/> to configure timeout, retry delay, maximum attempts, and inherited cancellation for an asynchronous operation.

```csharp
using System;
using System.Threading;
using Cuemon.Threading;

namespace MyApp.Examples;

public class AsyncRunOptionsExample
{
    public void Demonstrate()
    {
        var options = new AsyncRunOptions
        {
            Timeout = TimeSpan.FromSeconds(30),
            Delay = TimeSpan.FromMilliseconds(500),
            MaximumAttempts = 3
        };
        Console.WriteLine(options.Timeout); // 00:00:30
        Console.WriteLine(options.Delay);   // 00:00:00.5000000
        Console.WriteLine(options.MaximumAttempts); // 3

        // Use with cancellation support
        var withCancellation = new AsyncRunOptions
        {
            Timeout = TimeSpan.FromSeconds(10),
            CancellationToken = new CancellationTokenSource(5000).Token
        };

        // Zero-delay retries require an explicit attempt limit
        var zeroDelay = new AsyncRunOptions
        {
            Timeout = TimeSpan.FromSeconds(1),
            Delay = TimeSpan.Zero,
            MaximumAttempts = 3
        };
        Console.WriteLine(zeroDelay.MaximumAttempts); // 3

        // Defaults: timeout 5s, delay 100ms
        var defaults = new AsyncRunOptions();
        Console.WriteLine(defaults.Timeout); // 00:00:05
        Console.WriteLine(defaults.Delay);   // 00:00:00.1000000
        Console.WriteLine(defaults.MaximumAttempts); // 0
        Console.WriteLine(withCancellation.CancellationToken.CanBeCanceled); // True
    }
}

```
