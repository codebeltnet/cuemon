---
uid: Cuemon.Collections.Generic.EnumerableSizeComparer`1
example:
- *content
---

The following example demonstrates how to compare the sizes of enumerable collections using `EnumerableSizeComparer`. It shows comparisons between collections of different and equal sizes, including null-value handling.

```csharp
using System;
using System.Collections;
using System.Collections.Generic;
using Cuemon.Collections.Generic;

namespace MyApp.Examples
{
    public class EnumerableSizeComparerExample
    {
        public static void Demonstrate()
        {
            var shortList = new[] { 10, 20, 30 };
            var longList = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var equalList = new[] { "x", "y", "z" };

            var comparer = EnumerableSizeComparer<IEnumerable>.Default;

            int result1 = comparer.Compare(shortList, longList);  // -1 (shortList has fewer elements)
            int result2 = comparer.Compare(longList, shortList);  //  1 (longList has more elements)
            int result3 = comparer.Compare(shortList, equalList); //  0 (both have 3 elements)
            int result4 = comparer.Compare(null, longList);       // -1 (null is less than any non-null)
            int result5 = comparer.Compare(shortList, null);      //  1 (non-null is greater than null)

            Console.WriteLine($"shortList vs longList : {result1}");
            Console.WriteLine($"longList vs shortList : {result2}");
            Console.WriteLine($"shortList vs equalList: {result3}");
            Console.WriteLine($"null vs longList      : {result4}");
            Console.WriteLine($"shortList vs null     : {result5}");

}}
}

```
