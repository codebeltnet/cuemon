---
uid: Cuemon.UnsuccessfulValue`1
example:
- *content
---

The following example demonstrates how to use `UnsuccessfulValue<TResult>` to represent a typed operation that failed, carrying both the exception and a default result value.

```csharp
// Program.cs
using System;
using Cuemon;
using Contoso.Calculations;

UnsuccessfulValueOfTResultExample.Run();

namespace Contoso.Calculations
{
    public sealed class UnsuccessfulValueOfTResultExample
    {
        public static void Run()
        {
            ConditionalValue<int> outcome = Divide(10, 0);

            Console.WriteLine($"Succeeded: {outcome.Succeeded}");
            Console.WriteLine($"Result: {outcome.Result}");
            Console.WriteLine($"Failure: {outcome.Failure?.GetType().Name}");
        }

        private static ConditionalValue<int> Divide(int dividend, int divisor)
        {
            if (divisor == 0)
            {
                return new UnsuccessfulValue<int>(new DivideByZeroException("Cannot divide by zero."), -1);
            }

            return new SuccessfulValue<int>(dividend / divisor);
        }
    }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="ef74236c0cf2bc9238bf142d"><a href="~/run-actions/ef74236c0cf2bc9238bf142d/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/ef74236c0cf2bc9238bf142d/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
