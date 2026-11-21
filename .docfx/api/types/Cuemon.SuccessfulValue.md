---
uid: Cuemon.SuccessfulValue
example:
- *content
---

The following example demonstrates how to use `SuccessfulValue` to represent a void operation that completed successfully, enabling consistent conditional-return patterns.

```csharp
// Program.cs
using System;
using Cuemon;
using Contoso.Startup;

SuccessfulValueExample.Run();

namespace Contoso.Startup
{
    public sealed class SuccessfulValueExample
    {
        public static void Run()
        {
            ConditionalValue outcome = WarmUpCache(dependenciesReady: true);

            Console.WriteLine($"Succeeded: {outcome.Succeeded}");
            Console.WriteLine($"Failure is null: {outcome.Failure is null}");
        }

        private static ConditionalValue WarmUpCache(bool dependenciesReady)
        {
            if (!dependenciesReady)
            {
                return new UnsuccessfulValue(new InvalidOperationException("Dependencies are missing."));
            }

            return new SuccessfulValue();
        }
    }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="937480803a2b64cf9a297384"><a href="~/run-actions/937480803a2b64cf9a297384/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/937480803a2b64cf9a297384/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
