---
uid: Cuemon.Resilience.TransientFaultEvidence
example:
- *content
---

The following example demonstrates how to create TransientFaultEvidence instances to capture retry attempt details, recovery wait times, and latency information for transient fault handling.

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
