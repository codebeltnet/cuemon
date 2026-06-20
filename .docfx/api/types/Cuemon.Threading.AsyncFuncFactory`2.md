---
uid: Cuemon.Threading.AsyncFuncFactory`2
example:
- *content
---

`AsyncFuncFactory<TArg1, TArg2, TResult>` encapsulates a deferred asynchronous function with typed arguments and return value, with cloning support for safe concurrent use. This example creates a factory using `AsyncFuncFactory.Create` with a lambda that takes `"Hello"` and `"World"` and returns their combined length, then calls `ExecuteMethodAsync` to compute the result. Key steps include checking `HasDelegate` and `DelegateInfo` properties, and cloning the factory via `(AsyncFuncFactory<MutableTuple<string, string>, int>)factory.Clone()` to execute independently. Console output confirms `HasDelegate` is `True`, the combined length is `10`, and the cloned result is also `10`.

```csharp
using System;
using System.Threading;
using System.Threading.Tasks;
using Cuemon;
using Cuemon.Threading;

namespace MyApp.Examples
{
    public class AsyncFuncFactoryExample
    {
        public async Task DemonstrateAsync()
        {
            // Create an AsyncFuncFactory that encapsulates a function taking
            // two string arguments and returning an int.
            var factory = AsyncFuncFactory.Create(
                (string a, string b, CancellationToken ct) =>
                {
                    ct.ThrowIfCancellationRequested();
                    return Task.FromResult(a.Length + b.Length);
                },
                "Hello",
                "World"
            );

            Console.WriteLine($"Has delegate: {factory.HasDelegate}");                        // True
            Console.WriteLine($"Delegate info: {factory.DelegateInfo?.Name}");                 // <Main>b__0_0 or similar

            // Execute the wrapped function asynchronously.
            var result = await factory.ExecuteMethodAsync(CancellationToken.None);
            Console.WriteLine($"Total length: {result}");  // 10

            // Clone the factory for safe concurrent use.
            var clone = (AsyncFuncFactory<MutableTuple<string, string>, int>)factory.Clone();
            var clonedResult = await clone.ExecuteMethodAsync(CancellationToken.None);
            Console.WriteLine($"Cloned result: {clonedResult}");  // 10

}}
}

```
