---
uid: System.Runtime.CompilerServices.CallerArgumentExpressionAttribute
example:
- *content
---

The following example demonstrates `CallerArgumentExpressionAttribute` in a validation helper. `Validate` captures the source expression of its `condition` argument, so when `value > 0` is passed the console prints "Assertion passed: value > 0".

```csharp
// Program.cs
using System;
using System.Runtime.CompilerServices;

new CallerArgumentExpressionAttributeExample().Demonstrate();

namespace System.Runtime.CompilerServices
{
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
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="a8659fe8339088b7b883c517"><a href="~/run-actions/a8659fe8339088b7b883c517/native.ps1" data-transport="native">View Native Script</a></div>
<!-- docfx-run-actions:end -->
