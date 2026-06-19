---
uid: Cuemon.Collections.Generic.DynamicComparer
example:
- *content
---

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using Cuemon.Collections.Generic;

namespace MyApp.Examples;

public static class DynamicComparerExample
{
    public static void Demonstrate()
    {
        string[] fruits = ["apple", "pear", "banana", "kiwi"];

        // Create a dynamic comparer that sorts by string length
        IComparer<string> lengthComparer = DynamicComparer.Create<string>((x, y) =>
            x.Length.CompareTo(y.Length));

        Array.Sort(fruits, lengthComparer);
        Console.WriteLine(string.Join(", ", fruits));

        // Create a comparer that sorts descending
        IComparer<string> descendingComparer = DynamicComparer.Create<string>((x, y) =>
            string.Compare(y, x, StringComparison.Ordinal));

        Array.Sort(fruits, descendingComparer);
        Console.WriteLine(string.Join(", ", fruits));
    }
}
```
