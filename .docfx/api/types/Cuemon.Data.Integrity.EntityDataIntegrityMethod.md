---
uid: Cuemon.Data.Integrity.EntityDataIntegrityMethod
example:
- *content
---

The following example demonstrates how to use the <see cref="EntityDataIntegrityMethod"/> enum to specify how a checksum should be computed for data integrity validation.

```csharp
// Program.cs
using System;
using Cuemon.Data.Integrity; // for EntityDataIntegrityMethod

// Unaltered - the checksum is left as-is (default)
EntityDataIntegrityMethod method = EntityDataIntegrityMethod.Unaltered;
Console.WriteLine(method); // Unaltered

// Combined - the checksum is computed from all inputs combined
method = EntityDataIntegrityMethod.Combined;
Console.WriteLine(method); // Combined

// Timestamp - the checksum is generated from date-time inputs
method = EntityDataIntegrityMethod.Timestamp;
Console.WriteLine(method); // Timestamp

// Switch on the method to determine behavior
switch (method)
{
    case EntityDataIntegrityMethod.Unaltered:
        Console.WriteLine("Checksum unchanged.");
        break;
    case EntityDataIntegrityMethod.Combined:
        Console.WriteLine("Checksum computed from all data.");
        break;
    case EntityDataIntegrityMethod.Timestamp:
        Console.WriteLine("Checksum based on timestamp.");
        break;

}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="a8ed9e4532b234819fc65f92"><a href="~/run-actions/a8ed9e4532b234819fc65f92/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/a8ed9e4532b234819fc65f92/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
