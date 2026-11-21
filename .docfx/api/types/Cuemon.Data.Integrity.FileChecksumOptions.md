---
uid: Cuemon.Data.Integrity.FileChecksumOptions
example:
- *content
---

The following example shows how to configure `FileChecksumOptions` and create cache validators with different integrity methods for a file. It demonstrates default settings, combined validation, and strong validation with a limited byte read.

```csharp
// Program.cs
using System;
using System.IO;
using Cuemon.Data.Integrity;

var defaults = new FileChecksumOptions();
Console.WriteLine($"Default method: {defaults.Method}");
Console.WriteLine($"Default bytes to read: {defaults.BytesToRead}");

var path = Path.Combine(AppContext.BaseDirectory, "payload.txt");
File.WriteAllText(path, "cuemon");

try
{
    var file = new FileInfo(path);

    var combinedValidator = CacheValidatorFactory.CreateValidator(file, setup: options =>
    {
        options.Method = EntityDataIntegrityMethod.Combined;
    });

    var strongValidator = CacheValidatorFactory.CreateValidator(file, setup: options =>
    {
        options.BytesToRead = 4;
    });

    Console.WriteLine($"Combined method: {combinedValidator.Method}");
    Console.WriteLine($"Strong validation: {strongValidator.Validation}");
}
finally
{
    if (File.Exists(path)) { File.Delete(path); }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="b0a50b9ab2c39935758cf8ff"><a href="~/run-actions/b0a50b9ab2c39935758cf8ff/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/b0a50b9ab2c39935758cf8ff/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
