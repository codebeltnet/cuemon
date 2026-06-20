---
uid: Cuemon.CasingMethod
example:
- *content
---

The following example demonstrates how to use `CasingMethod` to control string casing transformations.

```csharp
using Cuemon;
using System;

namespace MyApp.Examples;

public class CasingMethodExample
{
    public void Demonstrate()
    {
        var lower = CasingMethod.LowerCase;
        var upper = CasingMethod.UpperCase;
        var title = CasingMethod.TitleCase;

        Console.WriteLine(lower); // outputs: LowerCase
        Console.WriteLine(upper); // outputs: UpperCase
        Console.WriteLine(title); // outputs: TitleCase

}
}

```
