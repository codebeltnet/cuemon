---
uid: Cuemon.IntegerDecoratorExtensions
example:
- *content
---

`IntegerDecoratorExtensions` provides extension methods on `Decorator.Enclose` for clamping integer values to a minimum bound using the `Max` extension method. This example wraps `int` values of `42`, `500`, and `-10`, then calls `Max` with a minimum threshold of `100` or `0`. Key setup includes comparing the wrapped value against the minimum and returning the larger of the two. Console output shows `100` for `42.Max(100)`, `500` for `500.Max(100)` (pass-through), and `0` for `(-10).Max(0)`.

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
