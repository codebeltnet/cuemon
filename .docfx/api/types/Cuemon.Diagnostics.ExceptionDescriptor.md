---
uid: Cuemon.Diagnostics.ExceptionDescriptor
example:
- *content
---

The following example demonstrates how to create <see cref="ExceptionDescriptor"/> and enrich it with contextual evidence.

```csharp
// Program.cs
using System;
using Cuemon.Diagnostics;

var descriptor = new ExceptionDescriptor(
    new InvalidOperationException("Order 12345 has already been processed."),
    code: "OrderAlreadyProcessed",
    message: "The order cannot be modified because it has already been processed.",
    helpLink: new Uri("https://docs.example.com/errors/order-already-processed"));

descriptor.AddEvidence("OrderId", 12345, value => value);
descriptor.AddEvidence("UserId", "alice@example.com", value => value);

Console.WriteLine(descriptor.Code);
Console.WriteLine(descriptor.Message);
Console.WriteLine(descriptor.Evidence.Count);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="626bc27d58574044529a1e6f"><a href="~/run-actions/626bc27d58574044529a1e6f/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/626bc27d58574044529a1e6f/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
