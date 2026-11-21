---
uid: Cuemon.Data.Integrity.EntityInfo
example:
- *content
---

The following example demonstrates various ways to construct `EntityInfo` instances with timestamps and optional checksums for data integrity. It shows creation-only, created-and-modified, validated, and local-time normalization scenarios, printing the resulting values.

```csharp
// Program.cs
using System;
using Cuemon.Data.Integrity;
using Cuemon.Security;

// Create EntityInfo with only a creation timestamp.
var entity = new EntityInfo(new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc));
Console.WriteLine("Created: {0:O}", entity.Created);
Console.WriteLine("Modified: {0}", entity.Modified.HasValue ? entity.Modified.Value.ToString("O") : "null");
Console.WriteLine("Has checksum: {0}", entity.Checksum.HasValue);
Console.WriteLine("Validation: {0}", entity.Validation);

// Create EntityInfo with creation and modification timestamps.
var modified = new EntityInfo(
    new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
    new DateTime(2025, 6, 15, 12, 30, 0, DateTimeKind.Utc));
Console.WriteLine("Modified entity: Created={0:O}, LastModified={1:O}",
    modified.Created, modified.Modified);

// Create EntityInfo with a checksum for data integrity validation.
// The checksum can be used to detect changes to the underlying data.
byte[] checksumBytes = { 0x1A, 0x2B, 0x3C, 0x4D };
var validated = new EntityInfo(
    new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
    null,
    checksumBytes,
    EntityDataIntegrityValidation.Strong);

Console.WriteLine("Validated entity: Checksum={0}, Validation={1}",
    validated.Checksum.ToHexadecimalString(),
    validated.Validation);

// Timestamps are always normalized to UTC.
var localTime = new EntityInfo(new DateTime(2025, 6, 1, 10, 0, 0, DateTimeKind.Local));
Console.WriteLine("UTC created: {0:O}", localTime.Created);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="c352476ee24c231643f772dd"><a href="~/run-actions/c352476ee24c231643f772dd/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/c352476ee24c231643f772dd/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
