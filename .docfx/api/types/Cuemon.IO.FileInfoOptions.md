---
uid: Cuemon.IO.FileInfoOptions
example:
- *content
---

`FileInfoOptions` controls the maximum number of bytes read from a file, useful for reading headers or file signatures. This example demonstrates default options where `BytesToRead = 0` means no limit, a 100-byte limit for reading file headers, and a 16-byte limit for file signature detection. A temporary file with 1000 `'A'` characters is created, then read with the 100-byte limit and the actual bytes read is reported. Console output shows the default `BytesToRead` value, the requested vs. actual bytes read, and confirms that `BytesToRead = 0` means no limit.

```csharp
using System;
using System.IO;
using Cuemon.IO;

namespace MyApp.IO
{
    public class FileInfoOptionsExample
    {
        public void Demonstrate()
        {
            // Default options: BytesToRead = 0 (read entire file)
            var defaultOptions = new FileInfoOptions();
            Console.WriteLine($"Default bytes to read: {defaultOptions.BytesToRead}"); // 0

            // Read only the first 100 bytes of a file
            var headerOptions = new FileInfoOptions
            {
                BytesToRead = 100
            };

            // Read only the first 16 bytes (e.g., for file signature detection)
            var signatureOptions = new FileInfoOptions
            {
                BytesToRead = 16
            };

            string tempFile = Path.GetTempFileName();
            try
            {
                File.WriteAllText(tempFile, new string('A', 1000));

                // Demonstrate how BytesToRead limits the data read
                using var fileStream = File.OpenRead(tempFile);
                byte[] buffer = new byte[headerOptions.BytesToRead];
                int bytesRead = fileStream.Read(buffer, 0, headerOptions.BytesToRead);
                Console.WriteLine($"Requested {headerOptions.BytesToRead} bytes, read {bytesRead} bytes");
            }
            finally
            {
                File.Delete(tempFile);
            }

            // BytesToRead of 0 means no limit
            var noLimitOptions = new FileInfoOptions
            {
                BytesToRead = 0
            };
            Console.WriteLine($"BytesToRead = 0 means no limit: {noLimitOptions.BytesToRead}");
        }
    }
}
```
