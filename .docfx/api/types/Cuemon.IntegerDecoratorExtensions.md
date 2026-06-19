---
uid: Cuemon.IntegerDecoratorExtensions
example:
- *content
---

The following example shows how to extend `int` with `IntegerDecoratorExtensions` methods to clamp integer values to a minimum bound via the decorator pattern.

```csharp
using System;
using Cuemon;

namespace MyApp.Numeric
{
    public class IntegerDecoratorExtensionsExample
    {
        public void Demonstrate()
        {
            // Wrap an int with Decorator.Enclose to access the Max extension
            int value = 42;
            int minimum = 100;

            // Returns the larger of the wrapped value and the specified minimum
            int result = Decorator.Enclose(value).Max(minimum);
            Console.WriteLine(result); // Output: 100

            // When the wrapped value is larger than the minimum
            value = 500;
            result = Decorator.Enclose(value).Max(minimum);
            Console.WriteLine(result); // Output: 500

            // Works with any int expression
            result = Decorator.Enclose(-10).Max(0);
            Console.WriteLine(result); // Output: 0

}}
}

```
