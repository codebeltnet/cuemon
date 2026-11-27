---
uid: Cuemon.ExceptionDecoratorExtensions
example:
- *content
---

`ExceptionDecoratorExtensions` provides extension methods on `Decorator.Enclose` for flattening nested inner exception hierarchies into a flat sequence via the `Flatten` method. This example creates a three-level exception chain (`InvalidOperationException` → `ArgumentException` → `TimeoutException`) and an `AggregateException` containing two inner exceptions. Key steps include wrapping each exception with `Decorator.Enclose`, calling `Flatten()`, and materializing the result with `ToList()`. Console output shows the flattened inner exception chain (`ArgumentException -> TimeoutException`) and the aggregate inner exception count (`2`).

```csharp
// Program.cs
using System;
using System.Linq;
using Cuemon;

Exception nested = new InvalidOperationException(
    "Request failed.",
    new ArgumentException("Endpoint is invalid.", new TimeoutException("The call timed out.")));

var flattened = Decorator.Enclose(nested).Flatten().ToList();

Exception aggregate = new AggregateException(
    new InvalidOperationException("Retry later."),
    new TimeoutException("The call timed out."));

var aggregateInner = Decorator.Enclose(aggregate).Flatten().ToList();

Console.WriteLine($"Nested chain: {string.Join(" -> ", flattened.Select(ex => ex.GetType().Name))}");
Console.WriteLine($"Aggregate count: {aggregateInner.Count}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="2975928484698884dfa22fb6"><a href="~/run-actions/2975928484698884dfa22fb6/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/2975928484698884dfa22fb6/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
