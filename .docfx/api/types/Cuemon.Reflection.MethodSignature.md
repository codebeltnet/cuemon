---
uid: Cuemon.Reflection.MethodSignature
example:
- *content
---

The following example shows how to capture lightweight method metadata for logging or retry evidence.

```csharp
// Program.cs
using System;
using Cuemon.Reflection;
using MyApp.Examples;

MethodSignatureExample.Demonstrate();

namespace MyApp.Examples
{
    public static class MethodSignatureExample
    {
        public static void Demonstrate()
        {
            var signature = new MethodSignature(
                typeof(PaymentGateway).FullName ?? nameof(PaymentGateway),
                nameof(PaymentGateway.Authorize),
                new[] { typeof(string).Name, typeof(decimal).Name },
                new object[] { "INV-42", 19.95m });

            Console.WriteLine(signature.ToString());
            Console.WriteLine(string.Join(", ", signature.Parameters ?? Array.Empty<string>()));
            Console.WriteLine(signature.Arguments?.Length ?? 0);
        }
    }
    public sealed class PaymentGateway
    {
        public void Authorize(string orderId, decimal amount)
        {
        }
    }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="c421cdd9e84a346c1187a2b3"><a href="~/run-actions/c421cdd9e84a346c1187a2b3/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/c421cdd9e84a346c1187a2b3/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
