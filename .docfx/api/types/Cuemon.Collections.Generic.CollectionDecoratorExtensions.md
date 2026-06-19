---
uid: Cuemon.Collections.Generic.CollectionDecoratorExtensions
example:
- *content
---

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
