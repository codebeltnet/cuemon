---
uid: Cuemon.ExceptionDecoratorExtensions
example:
- *content
---

`ExceptionDecoratorExtensions` provides extension methods on `Decorator.Enclose` for flattening deeply nested exception hierarchies into a flat sequence via the `Flatten` method. This example creates a three-level exception chain (`InvalidOperationException` → `ArgumentException` → `TimeoutException`) and an `AggregateException` containing two root-level exceptions. Key steps include wrapping each exception with `Decorator.Enclose`, calling `Flatten()`, and materializing the result with `ToList()`. Console output shows the flattened chain order (`InvalidOperationException -> ArgumentException -> TimeoutException`) and the aggregate exception count, confirming the full hierarchy is unwound without losing any exceptions.

```csharp
using System;
using System.Linq;
using Cuemon;

namespace Contoso.Diagnostics;

public sealed class ExceptionDecoratorExtensionsExample
{
    public static void Run()
    {
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
    }
}
```
