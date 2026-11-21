---
uid: Cuemon.Data.Integrity.DataIntegrityFactory
example:
- *content
---

The following example demonstrates how to create an <see cref="IDataIntegrity"/> implementation from a file using <see cref="DataIntegrityFactory"/>. It writes sample data to a temporary file, then calls `CreateIntegrity` with a custom converter that computes a CRC64 hash over the file content. The resulting checksum and file name are written to the console, showing how to verify file integrity with configurable hashing algorithms.

```csharp
// Program.cs
using System;
using System.IO;
using Cuemon.Data.Integrity;
using Cuemon.Security;
using MyApp.Data;

new DataIntegrityFactoryExample().Demonstrate();

namespace MyApp.Data
{
    public sealed class DataIntegrityFactoryExample
    {
        public void Demonstrate()
        {
            var file = new FileInfo(Path.GetTempFileName());
            try
            {
                File.WriteAllText(file.FullName, "Sample data for integrity check.");

                IDataIntegrity integrity = DataIntegrityFactory.CreateIntegrity(file, options =>
                {
                    options.BytesToRead = 1024;
                    options.IntegrityConverter = (fi, checksumBytes) =>
                    {
                        var hash = HashFactory.CreateCrc64().ComputeHash(checksumBytes);
                        return new DataIntegrity(fi, hash);
                    };
                });

                Console.WriteLine($"Integrity: {integrity}");
            }
            finally
            {
                file.Delete();
            }
        }
    }
    // Minimal IDataIntegrity implementation for demonstration
    public class DataIntegrity(FileInfo file, HashResult checksum) : IDataIntegrity
    {
        public FileInfo File { get; } = file;

        public HashResult Checksum { get; } = checksum;

        public override string ToString() => $"{File.Name}: {Checksum.ToHexadecimalString()}";
    }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="74147b4a00b44e13eb7cfba8"><a href="~/run-actions/74147b4a00b44e13eb7cfba8/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/74147b4a00b44e13eb7cfba8/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
