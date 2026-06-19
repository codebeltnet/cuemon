---
uid: Cuemon.Threading.AsyncActionFactory`1
example:
- *content
---

The following example demonstrates how to create and execute an <xref:Cuemon.Threading.AsyncActionFactory`1> for deferred asynchronous work with typed arguments.

```csharp
using System;
using System.Threading;
using System.Threading.Tasks;
using Cuemon;
using Cuemon.Threading;

namespace MyApp.Examples;

public static class AsyncActionFactoryExample
{
    public static async Task DemonstrateAsync()
    {
        var factory = AsyncActionFactory.Create<string, int>(
            async (channel, retryCount, cancellationToken) =>
            {
                await Task.Delay(10, cancellationToken).ConfigureAwait(false);
                Console.WriteLine($"{channel}:{retryCount}");
            },
            "orders",
            3);

        await factory.ExecuteMethodAsync(CancellationToken.None);

        var clone = (AsyncActionFactory<MutableTuple<string, int>>)factory.Clone();
        await clone.ExecuteMethodAsync(CancellationToken.None);
    }
}
```
