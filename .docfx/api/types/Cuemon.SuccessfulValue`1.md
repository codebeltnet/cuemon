---
uid: Cuemon.SuccessfulValue`1
example:
- *content
---

The following example demonstrates how to use `SuccessfulValue<TResult>` to represent a typed operation that completed successfully, pairing a result value with a success signal.

```csharp
// Program.cs
using System;
using Cuemon;
using Contoso.Configuration;

SuccessfulValueOfTResultExample.Run();

namespace Contoso.Configuration
{
    public sealed class SuccessfulValueOfTResultExample
    {
        public static void Run()
        {
            ConditionalValue<int> outcome = ParsePort("443");

            Console.WriteLine($"Succeeded: {outcome.Succeeded}");
            Console.WriteLine($"Port: {outcome.Result}");
        }

        private static ConditionalValue<int> ParsePort(string text)
        {
            if (int.TryParse(text, out int port))
            {
                return new SuccessfulValue<int>(port);
            }

            return new UnsuccessfulValue<int>(new FormatException("The port number is invalid."));
        }
    }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="19924051bce122ddccb0bb7d"><a href="~/run-actions/19924051bce122ddccb0bb7d/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/19924051bce122ddccb0bb7d/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
