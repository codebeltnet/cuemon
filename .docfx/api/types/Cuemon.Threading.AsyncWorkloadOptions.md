---
uid: Cuemon.Threading.AsyncWorkloadOptions
example:
- *content
---

The following example demonstrates how to configure `AsyncWorkloadOptions` to control the partition size when processing items in parallel with `AdvancedParallelFactory.ForAsync`.

```csharp
// Program.cs
using System;
using System.Threading;
using System.Threading.Tasks;
using Cuemon;
using Cuemon.Threading;
using MyApp.Examples;

await new AsyncWorkloadOptionsExample().DemonstrateAsync();

namespace MyApp.Examples
{
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
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="f7ca66249506a7f49827f0f6"><a href="~/run-actions/f7ca66249506a7f49827f0f6/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/f7ca66249506a7f49827f0f6/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
