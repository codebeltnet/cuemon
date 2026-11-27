---
uid: Cuemon.Diagnostics.ExceptionDescriptorOptions
example:
- *content
---

The following example demonstrates how to configure <see cref="ExceptionDescriptorOptions"/> to control which sensitive details are included in serialized exception descriptors.

```csharp
// Program.cs
using System;
using Cuemon.Diagnostics; // for ExceptionDescriptorOptions, FaultSensitivityDetails

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
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="4753838b822eca025c49605f"><a href="~/run-actions/4753838b822eca025c49605f/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/4753838b822eca025c49605f/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
