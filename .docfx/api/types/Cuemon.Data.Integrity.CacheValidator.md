---
uid: Cuemon.Data.Integrity.CacheValidator
example:
- *content
---

The following example demonstrates how to create a `CacheValidator` to represent cacheable data with integrity validation, combining timestamps and content checksums.

```csharp
// Program.cs
using System;
using Cuemon.Data.Integrity;
using Cuemon.Security;

var entity = new EntityInfo(
    DateTime.UtcNow.AddHours(-2),
    DateTime.UtcNow.AddMinutes(-30)
);

var validator = new CacheValidator(entity, () => HashFactory.CreateFnv128(), EntityDataIntegrityMethod.Combined);

Console.WriteLine($"Created (UTC):  {validator.Created:O}");
Console.WriteLine($"Modified (UTC): {validator.Modified:O}");
Console.WriteLine($"Validation:     {validator.Validation}");
Console.WriteLine($"Method:         {validator.Method}");
Console.WriteLine($"Checksum:       {validator.Checksum.ToHexadecimalString()}");

// Combine with additional data
validator.CombineWith(BitConverter.GetBytes(67890L));
Console.WriteLine($"Combined:       {validator}");

// Get the most significant from a sequence
var v1 = new CacheValidator(new EntityInfo(new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)), () => HashFactory.CreateFnv128());
var v2 = new CacheValidator(new EntityInfo(DateTime.UtcNow.AddDays(-1)), () => HashFactory.CreateFnv128());
var mostSignificant = CacheValidator.GetMostSignificant(v1, v2);

Console.WriteLine($"Most significant created: {mostSignificant.Created:O}");

// Use assembly reference point
CacheValidator.AssemblyReference = typeof(CacheValidator).Assembly;
var referencePoint = CacheValidator.ReferencePoint;
Console.WriteLine($"Reference point: {referencePoint}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="d79b7496c0557321609a26ad"><a href="~/run-actions/d79b7496c0557321609a26ad/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/d79b7496c0557321609a26ad/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
