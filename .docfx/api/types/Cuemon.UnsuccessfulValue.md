---
uid: Cuemon.UnsuccessfulValue
example:
- *content
---

The following example demonstrates how to use `UnsuccessfulValue` to represent a void operation that failed with an exception, providing a consistent way to signal failure without throwing.

```csharp
// Program.cs
using System;
using Cuemon;
using Contoso.Connections;

UnsuccessfulValueExample.Run();

namespace Contoso.Connections
{
    public sealed class UnsuccessfulValueExample
    {
        public static void Run()
        {
            ConditionalValue outcome = OpenConnection(null);

            Console.WriteLine($"Succeeded: {outcome.Succeeded}");
            Console.WriteLine($"Failure: {outcome.Failure?.GetType().Name}");
        }

        private static ConditionalValue OpenConnection(string connectionString)
        {
            try
            {
                Validator.ThrowIfNullOrWhitespace(connectionString);
                return new SuccessfulValue();
            }
            catch (Exception ex)
            {
                return new UnsuccessfulValue(ex);
            }
        }
    }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="06e32e8efa4c20a5ae0f945f"><a href="~/run-actions/06e32e8efa4c20a5ae0f945f/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/06e32e8efa4c20a5ae0f945f/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
