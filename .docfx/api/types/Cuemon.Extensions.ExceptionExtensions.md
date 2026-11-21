---
uid: Cuemon.Extensions.ExceptionExtensions
example:
- *content
---

`ExceptionExtensions.Flatten` unwinds deeply nested exception hierarchies into a flat `IEnumerable<Exception>` while preserving insertion order. This example constructs a four-level exception chain starting with an `InvalidOperationException("First")` containing `AmbiguousMatchException`, `OutOfMemoryException`, and an inner `AggregateException` with an `AccessViolationException`. Key setup includes building the nested exception tree and calling `Flatten()` to produce a flat sequence. Console output shows the count of `4` and each caller-facing exception message in order.

```csharp
// Program.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cuemon.Extensions;

var exception = new InvalidOperationException(
    "First",
    new AmbiguousMatchException(
        "Second",
        new OutOfMemoryException(
            "Third",
            new AggregateException(new AccessViolationException("Fourth")))));

IEnumerable<Exception> flattened = exception.Flatten();

Console.WriteLine(flattened.Count());
foreach (var ex in flattened)
{
    Console.WriteLine(ex.Message);
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="abb649a7ebe9bd6f8bc0cde2"><a href="~/run-actions/abb649a7ebe9bd6f8bc0cde2/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/abb649a7ebe9bd6f8bc0cde2/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
