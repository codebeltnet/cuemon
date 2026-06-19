---
uid: Cuemon.Collections.Generic.DynamicEqualityComparer
example:
- *content
---

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using Cuemon.Collections.Generic;

namespace MyApp.Examples;

public static class DynamicEqualityComparerExample
{
    public static void Demonstrate()
    {
        string[] words = ["Hello", "world", "hello", "World", "HELLO"];

        // Create a case-insensitive equality comparer for strings
        IEqualityComparer<string> caseInsensitive = DynamicEqualityComparer.Create<string>(
            hashCalculator: s => StringComparer.OrdinalIgnoreCase.GetHashCode(s),
            equalityComparer: (x, y) => string.Equals(x, y, StringComparison.OrdinalIgnoreCase));

        string[] distinct = words.Distinct(caseInsensitive).ToArray();
        Console.WriteLine(string.Join(", ", distinct));
    }
}
```
