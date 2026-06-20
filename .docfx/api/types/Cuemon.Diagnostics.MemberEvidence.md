---
uid: Cuemon.Diagnostics.MemberEvidence
example:
- *content
---

The following example demonstrates how to retrieve a <see cref="Cuemon.Diagnostics.MemberEvidence"/> instance from an <see cref="Cuemon.Diagnostics.ExceptionDescriptor"/> that has been enriched with embedded insights. It throws an `ArgumentNullException` enriched via `ExceptionInsights.Embed` with the current method and runtime arguments, then catches and extracts the descriptor. The `MemberEvidence` is read from the descriptor's evidence dictionary, and its member signature and runtime parameter count are written to the console, showing how to capture diagnostic context at the point of failure.

```csharp
using System;
using System.Linq;
using System.Reflection;
using Cuemon;
using Cuemon.Collections.Generic;
using Cuemon.Diagnostics;

namespace MyApp.Examples;

public class MemberEvidenceExample
{
    public void Demonstrate()
    {
        try
        {
            throw ExceptionInsights.Embed(
                new ArgumentNullException("value", "Value cannot be null."),
                MethodBase.GetCurrentMethod(),
                Arguments.ToArray("value"),
                SystemSnapshots.CaptureAll);
        }
        catch (Exception ex)
        {
            var descriptor = ExceptionDescriptor.Extract(ex);
            if (descriptor.Evidence.TryGetValue("Thrower", out var thrower) &&
                thrower is MemberEvidence evidence)
            {
                Console.WriteLine($"Signature: {evidence.MemberSignature}");

                Console.WriteLine($"Runtime parameters: {evidence.RuntimeParameters.Count}");
                foreach (var kvp in evidence.RuntimeParameters)
                {
                    Console.WriteLine($"  {kvp.Key} = {kvp.Value}");
                }
            }
        }
    }
}
```
