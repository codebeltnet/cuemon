---
uid: Cuemon.Diagnostics.ExceptionDescriptorOptions
example:
- *content
---

The following example demonstrates how to configure <see cref="ExceptionDescriptorOptions"/> to control which sensitive details are included in serialized exception descriptors.

```csharp
using System;
using Cuemon.Diagnostics; // for ExceptionDescriptorOptions, FaultSensitivityDetails

namespace MyApp.Examples;

public class ExceptionDescriptorOptionsExample
{
    public void Demonstrate()
    {
        // Create options that include only the stack trace
        var options = new ExceptionDescriptorOptions
        {
            SensitivityDetails = FaultSensitivityDetails.StackTrace
        };
        Console.WriteLine(options.SensitivityDetails); // StackTrace

        // Create options that include stack trace and exception data
        var verbose = new ExceptionDescriptorOptions
        {
            SensitivityDetails =
                FaultSensitivityDetails.StackTrace |
                FaultSensitivityDetails.Data
        };

        // Verify flags are set
        bool hasStack = verbose.SensitivityDetails.HasFlag(FaultSensitivityDetails.StackTrace);
        bool hasData = verbose.SensitivityDetails.HasFlag(FaultSensitivityDetails.Data);
        Console.WriteLine(hasStack); // True
        Console.WriteLine(hasData);  // True

        // Default is none
        var defaults = new ExceptionDescriptorOptions();
        Console.WriteLine(defaults.SensitivityDetails); // None

}
}

```
