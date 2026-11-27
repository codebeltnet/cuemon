---
uid: Cuemon.Threading.ParallelFactory
example:
- *content
---

The following example demonstrates basic parallel loops using `ParallelFactory.For` and `ParallelFactory.ForEach`. Each iteration prints its index or item to the console, illustrating how the factory distributes work across threads.

```csharp
// Program.cs
using System;
using Cuemon.Threading;

ParallelFactory.For(0, 5, i =>
{
    Console.WriteLine($"Processing iteration {i}");
});

var items = new[] { "apple", "banana", "cherry" };
ParallelFactory.ForEach(items, item =>
{
    Console.WriteLine($"Processing {item}");
});
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="108e4b7459222a698ecba7d7"><a href="~/run-actions/108e4b7459222a698ecba7d7/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/108e4b7459222a698ecba7d7/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
