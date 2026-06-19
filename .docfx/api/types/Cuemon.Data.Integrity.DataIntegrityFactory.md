---
uid: Cuemon.Data.Integrity.DataIntegrityFactory
example:
- *content
---

The following example demonstrates how to create an <see cref="IDataIntegrity"/> implementation from a file using <see cref="DataIntegrityFactory"/>.

```csharp
using System;
using System.IO;
using Cuemon.Data.Integrity;
using Cuemon.Security;

namespace MyApp.Data;

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
```
