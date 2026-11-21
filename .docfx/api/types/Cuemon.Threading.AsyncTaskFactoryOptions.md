---
uid: Cuemon.Threading.AsyncTaskFactoryOptions
example:
- *content
---

The following example demonstrates how to configure `AsyncTaskFactoryOptions` to control task creation options and scheduler when using `AdvancedParallelFactory.For`.

```csharp
// Program.cs
using System;
using System.Threading.Tasks;
using Cuemon;
using Cuemon.Threading;

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
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="2971220cd0c6bfed6f7cd376"><a href="~/run-actions/2971220cd0c6bfed6f7cd376/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/2971220cd0c6bfed6f7cd376/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
