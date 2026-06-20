---
uid: Cuemon.Threading.AsyncTaskFactoryOptions
example:
- *content
---

The following example demonstrates how to configure `AsyncTaskFactoryOptions` to control task creation options and scheduler when using `AdvancedParallelFactory.For`.

```csharp
using System;
using System.Threading.Tasks;
using Cuemon;
using Cuemon.Threading;

namespace MyApp.Examples;

public class AsyncTaskFactoryOptionsExample
{
    public void Demonstrate()
    {
        // Direct instantiation of AsyncTaskFactoryOptions
        var factoryOptions = new AsyncTaskFactoryOptions
        {
            CreationOptions = TaskCreationOptions.None,
            PartitionSize = 2
        };

        var rules = new ForLoopRuleset<int>(0, 10, 1);

        AdvancedParallelFactory.For(rules, i =>
        {
            Console.WriteLine($"Processing item {i}");
        }, o =>
        {
            o.CreationOptions = TaskCreationOptions.None;
            o.Scheduler = TaskScheduler.Default;
            o.PartitionSize = 2;
        });

}
}

```
