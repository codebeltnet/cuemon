---
uid: Cuemon.Threading.AsyncFuncFactory
example:
- *content
---

`AsyncFuncFactory` provides factory methods for creating deferred asynchronous functions with zero or more typed arguments and a return value, executed via `ExecuteMethodAsync`. This example creates a function that returns `42` and another that sums two integers (`3` and `4`) after a 10ms delay, then invokes each with `CancellationToken.None`. Key setup includes using `AsyncFuncFactory.Create` with a lambda and passing arguments separately so the factory manages lifetime and error propagation. Console output displays `"Result: 42"` and `"Sum: 7"`, confirming both the parameterless and parameterized async function patterns.

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
