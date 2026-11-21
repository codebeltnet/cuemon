---
uid: Cuemon.Data.Integrity.CacheValidatorFactory
example:
- *content
---

The following example demonstrates how to create a <see cref="CacheValidator"/> from a file using <see cref="CacheValidatorFactory"/>.

```csharp
// Program.cs
using System;
using System.IO;
using Cuemon.Data.Integrity;
using Cuemon.Security;

var file = new FileInfo(Path.GetTempFileName());
try
{
    File.WriteAllText(file.FullName, "Hello, world!");

    CacheValidator validator = CacheValidatorFactory.CreateValidator(file);
    Console.WriteLine($"Created (UTC):  {validator.Created:O}");
    Console.WriteLine($"Modified (UTC): {validator.Modified:O}");
    Console.WriteLine($"Validation:     {validator.Validation}");
    Console.WriteLine($"Method:         {validator.Method}");
    Console.WriteLine($"Checksum:       {validator.Checksum.ToHexadecimalString()}");

    // Create validator using a custom hash algorithm
    CacheValidator shaValidator = CacheValidatorFactory.CreateValidator(
        file,
        () => HashFactory.CreateFnv128());
    Console.WriteLine($"SHA-256: {shaValidator.Checksum.ToHexadecimalString()}");
}
finally
{
    file.Delete();
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="b69cffc3764e21a4fcae313a"><a href="~/run-actions/b69cffc3764e21a4fcae313a/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/b69cffc3764e21a4fcae313a/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
