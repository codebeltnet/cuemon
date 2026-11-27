---
uid: Cuemon.Messaging.CorrelationToken
example:
- *content
---

The following example shows how to create one correlation identifier and pass it through related work.

```csharp
// Program.cs
using System;
using Cuemon.Messaging;
using MyApp.Examples;

CorrelationTokenExample.Demonstrate();

namespace MyApp.Examples
{
    public static class CorrelationTokenExample
    {
        public static void Demonstrate()
        {
            var generated = new CorrelationToken();
            var provided = new CorrelationToken("order-2026-0001");

            Console.WriteLine(generated.CorrelationId.Length == 32);
            Console.WriteLine(provided.ToString());
            Console.WriteLine(AttachToMessage("InventoryReserved", provided));
        }

        private static string AttachToMessage(string messageType, ICorrelationToken token)
        {
            return $"{messageType}:{token.CorrelationId}";
        }
    }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="0d874714a25636acf99ec6a2"><a href="~/run-actions/0d874714a25636acf99ec6a2/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/0d874714a25636acf99ec6a2/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
