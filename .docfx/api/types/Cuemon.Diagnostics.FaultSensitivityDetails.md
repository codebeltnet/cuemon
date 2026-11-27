---
uid: Cuemon.Diagnostics.FaultSensitivityDetails
example:
- *content
---

The following example demonstrates how to use the <see cref="FaultSensitivityDetails"/> enum to control the level of sensitive details included when serializing an <see cref="Cuemon.Diagnostics.ExceptionDescriptor"/>.

```csharp
// Program.cs
using System;
using Cuemon.Diagnostics; // for FaultSensitivityDetails, ExceptionDescriptor, FaultResolver

// Create an exception descriptor with failure details
var descriptor = new ExceptionDescriptor(
    new InvalidOperationException("Something went wrong."),
    "ERR_OPERATION_FAILED",
    "The requested operation could not be completed.");

// FaultSensitivityDetails.None (default) - excludes all sensitive details
FaultSensitivityDetails details = FaultSensitivityDetails.None;
Console.WriteLine(details); // None

// Include the Failure (exception) property
details = FaultSensitivityDetails.Failure;
Console.WriteLine(details); // Failure

// Include both Failure and StackTrace
details = FaultSensitivityDetails.FailureWithStackTrace;
Console.WriteLine(details); // FailureWithStackTrace

// Include everything (development environments only)
details = FaultSensitivityDetails.All;
Console.WriteLine(details); // All

// Combine flags manually
details = FaultSensitivityDetails.Failure | FaultSensitivityDetails.Data;
Console.WriteLine(details); // FailureWithData

// Check if a specific flag is set
bool hasStackTrace = details.HasFlag(FaultSensitivityDetails.StackTrace);
Console.WriteLine(hasStackTrace); // False

bool hasFailure = details.HasFlag(FaultSensitivityDetails.Failure);
Console.WriteLine(hasFailure); // True
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="727d6497cd0b57293c802d3e"><a href="~/run-actions/727d6497cd0b57293c802d3e/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/727d6497cd0b57293c802d3e/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
