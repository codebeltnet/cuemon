---
uid: Cuemon.Extensions.Data.Integrity.FileInfoExtensions
example:
- *content
---

The following example demonstrates generating a <see cref="CacheValidator"/> from a file using the [GetCacheValidator](https://docs.cuemon.net/api/extensions/dotnet/Cuemon.Extensions.Data.Integrity.FileInfoExtensions.html#Cuemon_Extensions_Data_Integrity_FileInfoExtensions_GetCacheValidator_System_IO_FileInfo_System_Func_Cuemon_Security_Hash__System_Action_Cuemon_Data_Integrity_FileChecksumOptions__) extension method.

```csharp
using System;
using System.IO;
using System.Text;
using Cuemon.Data.Integrity;
using Cuemon.Extensions.Data.Integrity;

namespace MyApp.Examples;

public class FileInfoExtensionsExample
{
    public static void Main()
    {
        // Create a temporary file to work with
        string tempFile = Path.GetTempFileName();
        File.WriteAllText(tempFile, "Hello, World!", Encoding.UTF8);

        try
        {
            var fileInfo = new FileInfo(tempFile);

            // Generate a CacheValidator with default FNV-1a/128 hashing
            CacheValidator validator = fileInfo.GetCacheValidator();

            Console.WriteLine($"File: {fileInfo.Name}");
            Console.WriteLine($"Created (UTC): {validator.Created}");
            Console.WriteLine($"Modified (UTC): {validator.Modified}");
            Console.WriteLine($"Checksum (hex): {validator.Checksum.ToHexadecimalString()}");
            Console.WriteLine($"Validation: {validator.Validation}");

            // Combine with an additional semantic checksum
            validator.CombineWith(Encoding.UTF8.GetBytes("additional-context"));
            Console.WriteLine($"Combined checksum: {validator.Checksum.ToHexadecimalString()}");
        }
        finally
        {
            File.Delete(tempFile);
        }
    }
}
```
