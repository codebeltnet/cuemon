---
uid: Cuemon.Extensions.ExceptionExtensions
example:
- *content
---

`ExceptionExtensions.Flatten` unwinds deeply nested exception hierarchies into a flat `IEnumerable<Exception>` while preserving insertion order. This example constructs a four-level exception chain starting with an `InvalidOperationException("First")` containing `AmbiguousMatchException`, `OutOfMemoryException`, and an inner `AggregateException` with an `AccessViolationException`. Key setup includes building the nested exception tree and calling `Flatten()` to produce a flat sequence. Console output shows the count of `4` and each exception type name in order: `InvalidOperationException`, `AmbiguousMatchException`, `OutOfMemoryException`, `AccessViolationException`.

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cuemon.Extensions;

namespace MyApp.Examples;

public static class ExceptionExtensionsExample
{
    public static void Demonstrate()
    {
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
            Console.WriteLine(ex.GetType().Name);
        }
    }
}

```
