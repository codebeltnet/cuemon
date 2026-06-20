---
uid: Cuemon.Resilience.TransientFaultEvidence
example:
- *content
---

`TransientFaultEvidence` captures retry attempt details, recovery wait times, latency, and method signature information for transient fault handling scenarios. This example creates evidence with `attempts = 3`, `RecoveryWaitTime = 2s`, `TotalRecoveryWaitTime = 5s`, `Latency = 1500ms`, and a `MethodSignature` for `PaymentService.ProcessPayment`. It also demonstrates equality comparison between two identical evidence instances, and a minimal-info creation with `attempts = 1` and `Latency = 200ms` for simple scenarios. Console output displays the evidence's `ToString` representation, individual property values, equality results, and hash code consistency.

```csharp
using System;
using Cuemon.Reflection;
using Cuemon.Resilience;

namespace MyApp.Resilience
{
    public class TransientFaultEvidenceExamples
    {
        public static void CreateWithMethodSignature()
        {
            var descriptor = new MethodSignature(
                "MyApp.Services.PaymentService",
                "ProcessPayment",
                new[] { "orderId", "amount" },
                new object[] { "ORD-12345", 99.99m }
            );

            var evidence = new TransientFaultEvidence(
                attempts: 3,
                recoveryWaitTime: TimeSpan.FromSeconds(2),
                totalRecoveryWaitTime: TimeSpan.FromSeconds(5),
                latency: TimeSpan.FromMilliseconds(1500),
                descriptor: descriptor
            );

            Console.WriteLine(evidence.ToString());
            Console.WriteLine("Attempts:           {0}", evidence.Attempts);
            Console.WriteLine("Last recovery wait: {0}", evidence.RecoveryWaitTime);
            Console.WriteLine("Total recovery wait: {0}", evidence.TotalRecoveryWaitTime);
            Console.WriteLine("Latency:            {0}", evidence.Latency);
            Console.WriteLine("Descriptor:         {0}", evidence.Descriptor);
        }

        public static void EqualityComparison()
        {
            var desc = new MethodSignature("App.MyClass", "DoWork", new[] { "id" }, new object[] { 42 });

            var evidence1 = new TransientFaultEvidence(2, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2), TimeSpan.FromMilliseconds(500), desc);
            var evidence2 = new TransientFaultEvidence(2, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2), TimeSpan.FromMilliseconds(500), desc);

            Console.WriteLine("Equals: {0}", evidence1.Equals(evidence2));
            Console.WriteLine("HashCode same: {0}", evidence1.GetHashCode() == evidence2.GetHashCode());
        }

        public static void CreateWithMinimalInfo()
        {
            var descriptor = new MethodSignature("App.Service", "Execute", Array.Empty<string>(), Array.Empty<object>());

            var evidence = new TransientFaultEvidence(
                attempts: 1,
                recoveryWaitTime: TimeSpan.Zero,
                totalRecoveryWaitTime: TimeSpan.Zero,
                latency: TimeSpan.FromMilliseconds(200),
                descriptor: descriptor
            );

            Console.WriteLine("Attempts: {0}", evidence.Attempts);
        }
    }
}
```
