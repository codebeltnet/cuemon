---
uid: Cuemon.IO.FileInfoOptions
example:
- *content
---

The following example demonstrates how to configure FileInfoOptions to control the number of bytes read from a file, useful for reading headers or file signatures.

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
