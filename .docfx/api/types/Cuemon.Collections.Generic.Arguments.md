---
uid: Cuemon.Collections.Generic.Arguments
example:
- *content
---

The following example demonstrates how to use the `Arguments` class to create arrays and enumerables from argument lists. It shows array concatenation, single-element yielding, and object-based overloads.

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using Cuemon.Collections.Generic;

namespace MyApp.Examples;

public static class ArgumentsExample
{
    public static void Demonstrate()
    {
        int[] first = Arguments.ToArrayOf(1, 2, 3);
        int[] second = Arguments.ToArrayOf(4, 5, 6);

        // Concat two arrays
        int[] combined = Arguments.Concat(first, second);
        Console.WriteLine(string.Join(", ", combined));

        // Yield a single element
        IEnumerable<int> yielded = Arguments.Yield(42);
        Console.WriteLine(yielded.First());

        // Convert to IEnumerable
        IEnumerable<string> enumerable = Arguments.ToEnumerableOf("a", "b", "c");
        Console.WriteLine(string.Concat(enumerable));

        // Object overloads
        object[] objs = Arguments.ToArray(1, "two", 3.0);
        IEnumerable<object> objEnumerable = Arguments.ToEnumerable(true, false);
    }
}
```
