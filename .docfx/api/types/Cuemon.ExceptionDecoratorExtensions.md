---
uid: Cuemon.ExceptionDecoratorExtensions
example:
- *content
---

The following example shows how to extend `Exception` with `ExceptionDecoratorExtensions` methods to flatten nested exception hierarchies into a flat sequence.

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
