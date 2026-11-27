---
uid: Cuemon.Data.Integrity.FileIntegrityOptions
example:
- *content
---

The following example demonstrates how to use <see cref="FileIntegrityOptions"/> to configure file integrity checksum computation.

```csharp
// Program.cs
using System;
using System.IO;
using Cuemon.Data.Integrity;
using Cuemon.Security;
using MyApp.Examples;

new FileIntegrityOptionsExample().Demonstrate();

namespace MyApp.Examples
{
    public sealed class FileIntegrityOptionsExample
    {
        public void Demonstrate()
        {
            var defaults = new FileIntegrityOptions();
            Console.WriteLine($"Default bytes to read: {defaults.BytesToRead}");

            var path = Path.Combine(AppContext.BaseDirectory, "sample.dat");
            File.WriteAllText(path, "Hello, World!");

            try
            {
                IDataIntegrity integrity = DataIntegrityFactory.CreateIntegrity(new FileInfo(path), options =>
                {
                    options.BytesToRead = 8;
                    options.IntegrityConverter = (file, bytes) => new FilePreviewIntegrity(file.Name, bytes);
                });

                Console.WriteLine($"Checksum: {integrity.Checksum.ToHexadecimalString()}");
            }
            finally
            {
                if (File.Exists(path)) { File.Delete(path); }
            }
        }

        private sealed class FilePreviewIntegrity : IDataIntegrity
        {
            public FilePreviewIntegrity(string fileName, byte[] bytes)
            {
                FileName = fileName;
                Checksum = HashFactory.CreateFnv128().ComputeHash(bytes);
            }

            public string FileName { get; }

            public HashResult Checksum { get; }
        }
    }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="df2e30c119abdf2a5e231072"><a href="~/run-actions/df2e30c119abdf2a5e231072/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/df2e30c119abdf2a5e231072/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
