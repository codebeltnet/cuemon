---
uid: Cuemon.Collections.Generic.ReferenceComparer`1
example:
- *content
---

The following example demonstrates how to compare objects by their inheritance depth using `ReferenceComparer`. It shows how objects with deeper inheritance chains are considered greater, including null-value comparisons.

```csharp
using System;
using System.Collections.Generic;
using Cuemon.Collections.Generic;

namespace MyApp.Collections;

public class ReferenceComparerExample
{
    public void Demonstrate()
    {
        // ReferenceComparer<T> compares objects by their inheritance depth.
        // Objects with a deeper inheritance chain are considered "greater."

        // Compare two strings (string derives from object → depth 2)
        string hello = "hello";
        string world = "world";
        IComparer<object> comparer = ReferenceComparer<object>.Default;

        int result1 = comparer.Compare(hello, world);
        Console.WriteLine($"string vs string: {result1}"); // 0 (same depth)

        // Compare a string with an Exception (deeper hierarchy)
        var exception = new InvalidOperationException();
        int result2 = comparer.Compare(hello, exception);
        Console.WriteLine($"string vs Exception: {result2}"); // -1 (string is shallower)

        // Compare an ArgumentNullException (depth 4) with Exception (depth 3)
        var argEx = new ArgumentNullException();
        int result3 = comparer.Compare(argEx, exception);
        Console.WriteLine($"ArgumentNullException vs Exception: {result3}"); // 1 (deeper)

        // Compare with null values
        int result4 = comparer.Compare(hello, null);
        Console.WriteLine($"string vs null: {result4}"); // 1 (non-null is greater)

        int result5 = comparer.Compare(null, hello);
        Console.WriteLine($"null vs string: {result5}"); // -1 (null is lesser)

        int result6 = comparer.Compare(null, null);
        Console.WriteLine($"null vs null: {result6}"); // 0 (both null)

}
}

```
