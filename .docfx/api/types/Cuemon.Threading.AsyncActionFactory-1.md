---
uid: Cuemon.Threading.AsyncActionFactory`1
example:
- *content
---

`AsyncActionFactory<T>` encapsulates a deferred asynchronous action with typed arguments, with support for cloning for safe concurrent execution. This example creates a factory using `AsyncActionFactory.Create<string, int>` with a lambda that takes a channel name (`"orders"`) and retry count (`3`), delays 10ms, and prints them, then calls `ExecuteMethodAsync` to run it. Key steps include cloning the factory via `(AsyncActionFactory<MutableTuple<string, int>>)factory.Clone()` and executing the clone separately. Console output displays `"orders:3"` for both the original and cloned execution, confirming that cloned factories produce identical results.

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
