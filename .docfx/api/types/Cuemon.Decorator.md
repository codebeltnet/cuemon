---
uid: Cuemon.Decorator
example:
- *content
---

```csharp
using System;
using Cuemon;

namespace MyApp.Wrapping;

public class DecoratorExample
{
    public void Demonstrate()
    {
        var wrappedString = Decorator.Enclose("Hello, World!");
        Console.WriteLine(wrappedString.Inner); // "Hello, World!"
        Console.WriteLine(wrappedString.ArgumentName); // ""

        var withArg = Decorator.EncloseToExpose("test", argumentName: "myArg");
        Console.WriteLine(withArg.ArgumentName); // "myArg"

        var syntactic = Decorator.Syntactic<DateTime>();
        Console.WriteLine(syntactic.Inner); // "01-01-0001 00:00:00" (default(DateTime))

        var raw = Decorator.RawEnclose<string>(null);
        Console.WriteLine(raw.Inner is null); // True
    }
}
```
