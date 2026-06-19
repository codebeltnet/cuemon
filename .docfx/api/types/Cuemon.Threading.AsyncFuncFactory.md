---
uid: Cuemon.Threading.AsyncFuncFactory
example:
- *content
---

```csharp
using System;
using System.Threading;
using System.Threading.Tasks;
using Cuemon.Threading;

namespace Cuemon.Threading;

public class AsyncFuncFactoryExample
{
    public async Task DemonstrateAsync()
    {
        var factory = AsyncFuncFactory.Create(ct =>
        {
            return Task.FromResult(42);
        });
        var result = await factory.ExecuteMethodAsync(CancellationToken.None);
        Console.WriteLine($"Result: {result}");

        var factoryWithArg = AsyncFuncFactory.Create(async (int a, int b, CancellationToken ct) =>
        {
            await Task.Delay(10, ct);
            return a + b;
        }, 3, 4);
        var sum = await factoryWithArg.ExecuteMethodAsync(CancellationToken.None);
        Console.WriteLine($"Sum: {sum}");
    }
}
```
