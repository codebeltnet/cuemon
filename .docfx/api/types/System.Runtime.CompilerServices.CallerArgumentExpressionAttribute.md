---
uid: System.Runtime.CompilerServices.CallerArgumentExpressionAttribute
example:
- *content
---

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
