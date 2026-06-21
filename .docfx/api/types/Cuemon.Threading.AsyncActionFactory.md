---
uid: Cuemon.Threading.AsyncActionFactory
example:
- *content
---

`AsyncActionFactory` provides factory methods for creating deferred asynchronous actions with zero or more typed arguments, executed via `ExecuteMethodAsync`. This example creates an action that prints `"Async operation executed"` and another with a string argument that logs `"Hello from async action"` after a 10ms delay, then invokes each with `CancellationToken.None`. Key setup includes using `AsyncActionFactory.Create` with a lambda and passing arguments separately so the factory handles lifetime and error propagation. Console output confirms both actions execute successfully, with the parameterized action receiving and printing the supplied message.

```csharp
using System;
using System.Threading;
using System.Threading.Tasks;
using Cuemon.Threading;

namespace Cuemon.Threading;

public class AsyncActionFactoryExample
{
    public async Task DemonstrateAsync()
    {
        var factory = AsyncActionFactory.Create(ct =>
        {
            Console.WriteLine("Async operation executed");
            return Task.CompletedTask;
        });
        await factory.ExecuteMethodAsync(CancellationToken.None);

        var factoryWithArg = AsyncActionFactory.Create(async (string msg, CancellationToken ct) =>
        {
            await Task.Delay(10, ct);
            Console.WriteLine(msg);
        }, "Hello from async action");
        await factoryWithArg.ExecuteMethodAsync(CancellationToken.None);
    }
}
```
