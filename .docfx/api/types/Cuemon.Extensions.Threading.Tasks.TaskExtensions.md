---
uid: Cuemon.Extensions.Threading.Tasks.TaskExtensions
example:
- *content
---

The following example demonstrates how to await a task with or without flowing the captured synchronization context.

```csharp
using System;
using System.Threading.Tasks;
using Cuemon.Extensions.Threading.Tasks;

namespace MyApp.Examples
{
    public static class TaskExtensionsExample
    {
        public static async Task DemonstrateAsync()
        {
            await Task.Delay(10).ContinueWithCapturedContext();
            await Task.Delay(10).ContinueWithSuppressedContext();

            var captured = await Task.FromResult(42).ContinueWithCapturedContext();
            var suppressed = await Task.FromResult(42).ContinueWithSuppressedContext();

            Console.WriteLine(captured);
            Console.WriteLine(suppressed);
        }
    }
}
```
