---
uid: Cuemon.Threading.AsyncActionFactory
example:
- *content
---

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
