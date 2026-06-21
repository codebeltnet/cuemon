---
uid: Cuemon.Collections.Generic.CollectionDecoratorExtensions
example:
- *content
---

`CollectionDecoratorExtensions` provides extension methods on `Decorator.Enclose` for bulk-adding elements to `ICollection<T>` instances using `AddRange`. This example wraps a `List<string>` with `"apple", "banana"` and calls `AddRange` with individual `"cherry", "date", "elderberry"` arguments and an array `["fig", "grape"]` to insert them in a single operation. Key setup includes calling `Decorator.Enclose(list).AddRange(...)` to access the non-common extension methods. Console output lists all entries after both bulk insertions, confirming that `AddRange` accepts both parameterized and array overloads.

```csharp
using System;
using System.Collections.Generic;
using Cuemon;
using Cuemon.Collections.Generic;

namespace MyApp.Examples
{
    public class CollectionDecoratorExtensionsExample
    {
        public static void Demonstrate()
        {
            var list = new List<string> { "apple", "banana" };

            // Use Decorator to access non-common extension methods
            Decorator.Enclose(list).AddRange("cherry", "date", "elderberry");
            Decorator.Enclose(list).AddRange(new[] { "fig", "grape" });

            Console.WriteLine(string.Join(", ", list));

}}
}

```
