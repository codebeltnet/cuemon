---
uid: Cuemon.AspNetCore.Diagnostics.FaultDescriptorOptions
example:
- *content
---

The following example demonstrates how to configure fault descriptor options for structured error responses.

```csharp
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.Diagnostics;

namespace MyApp.Examples;

public class FaultDescriptorOptionsExample
{
    public void Demonstrate()
    {
        var options = new FaultDescriptorOptions
        {
            SensitivityDetails = FaultSensitivityDetails.FailureWithStackTrace,
            RootHelpLink = new System.Uri("https://example.com/errors")
        };

}
}

```
