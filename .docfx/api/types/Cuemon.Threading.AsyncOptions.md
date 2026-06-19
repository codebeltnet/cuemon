---
uid: Cuemon.Threading.AsyncOptions
example:
- *content
---

The following example demonstrates how to configure AsyncOptions to provide a cancellation token for asynchronous operations.

```csharp
using System;
using System.Threading;
using System.Threading.Tasks;
using Cuemon.Threading;

namespace Contoso.BackgroundJobs;

public sealed class AsyncOptionsExample
{
    public static async Task RunAsync()
    {
        var defaults = new AsyncOptions();
        Console.WriteLine(defaults.CancellationToken == CancellationToken.None);

        using var cts = new CancellationTokenSource();
        var options = new AsyncOptions
        {
            CancellationTokenProvider = () => cts.Token
        };

        cts.Cancel();

        try
        {
            await Task.Delay(10, options.CancellationToken);
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Cancelled as expected.");
        }
    }
}
```
