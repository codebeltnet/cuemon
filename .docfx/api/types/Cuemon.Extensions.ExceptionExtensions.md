---
uid: Cuemon.Extensions.ExceptionExtensions
example:
- *content
---

The following example demonstrates flattening nested exception hierarchies using the <xref:Cuemon.Extensions.ExceptionExtensions.Flatten(System.Exception)> extension method.

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
