---
uid: Cuemon.Decorator`1
example:
- *content
---

The following example demonstrates how to use `Decorator<T>` to wrap a value and access it through the decorator pattern.

```csharp
using System;
using Cuemon;

namespace MyApp.Wrapping;

public class DecoratorOfTExample
{
    public void Demonstrate()
    {
        var numbers = new[] { 10, 20, 30 };
        var decorator = Decorator.Enclose(numbers);

        // Access the wrapped inner value through the Inner property
        int[] inner = decorator.Inner;
        Console.WriteLine(string.Join(", ", inner)); // "10, 20, 30"

        // ArgumentName is set automatically when using EncloseToExpose
        var withArgName = Decorator.EncloseToExpose(numbers);
        Console.WriteLine(withArgName.ArgumentName); // "numbers"

        // Syntactic sugar for type-level decoration
        var syntactic = Decorator.Syntactic<string>();
        Console.WriteLine(syntactic.Inner); // null (default)

}
}

```
