---
uid: System.Runtime.CompilerServices.CallerArgumentExpressionAttribute
example:
- *content
---

The following example demonstrates `CallerArgumentExpressionAttribute` in a validation helper. `Validate` captures the source expression of its `condition` argument, so when `value > 0` is passed the console prints "Assertion passed: value > 0".

```csharp
using System;
using System.Runtime.CompilerServices;

namespace System.Runtime.CompilerServices;

public class CallerArgumentExpressionAttributeExample
{
    public void Demonstrate()
    {
        var attr = new CallerArgumentExpressionAttribute("condition");
        Console.WriteLine(attr.ParameterName); // "condition"

        var value = 42;
        Validate(value > 0);
    }

    public void Validate(bool condition, [CallerArgumentExpression("condition")] string expression = null)
    {
        if (!condition)
        {
            Console.WriteLine($"Assertion failed: {expression}");
        }
        else
        {
            Console.WriteLine($"Assertion passed: {expression}");
        }
    }
}
```
