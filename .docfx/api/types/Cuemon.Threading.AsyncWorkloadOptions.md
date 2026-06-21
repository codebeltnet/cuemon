---
uid: Cuemon.Threading.AsyncWorkloadOptions
example:
- *content
---

The following example demonstrates how to configure `AsyncWorkloadOptions` to control the partition size when processing items in parallel with `AdvancedParallelFactory.ForAsync`.

```csharp
using System;
using System.Threading;
using System.Threading.Tasks;
using Cuemon;
using Cuemon.Threading;

namespace MyApp.Examples;

public class AsyncWorkloadOptionsExample
{
    public async Task DemonstrateAsync()
    {
        // Direct instantiation of AsyncWorkloadOptions
        var workloadOptions = new AsyncWorkloadOptions
        {
            PartitionSize = 4
        };

        var rules = new ForLoopRuleset<int>(0, 20, 1);

        await AdvancedParallelFactory.ForAsync(rules, (i, ct) =>
        {
            Console.WriteLine($"Processing item {i}");
            return Task.CompletedTask;
        }, o =>
        {
            o.PartitionSize = 4;
        });

}
}

```
