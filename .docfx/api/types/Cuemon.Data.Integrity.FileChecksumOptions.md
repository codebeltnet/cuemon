---
uid: Cuemon.Data.Integrity.FileChecksumOptions
example:
- *content
---

The following example shows how to configure `FileChecksumOptions` and create cache validators with different integrity methods for a file. It demonstrates default settings, combined validation, and strong validation with a limited byte read.

```csharp
using System;
using System.IO;
using Cuemon.Data.Integrity;

namespace MyApp.Data
{
    public sealed class FileChecksumOptionsExample
    {
        public void Demonstrate()
        {
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
        }
    }
}
```
