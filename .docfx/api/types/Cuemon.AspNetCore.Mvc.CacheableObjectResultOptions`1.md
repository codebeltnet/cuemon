---
uid: Cuemon.AspNetCore.Mvc.CacheableObjectResultOptions`1
example:
- *content
---

The following example demonstrates how to configure <xref cref="Cuemon.AspNetCore.Mvc.CacheableObjectResultOptions{T}"/> to enable HTTP caching (ETag and Last-Modified) for a response model.

```csharp
using System;
using System.Security.Cryptography;
using System.Text;
using Cuemon.AspNetCore.Mvc;

namespace MyApp.Examples;

public class CacheableObjectResultOptionsExample
{
    public void Demonstrate()
    {
        var options = new CacheableObjectResultOptions<string>
        {
            ChecksumProvider = value => SHA256.HashData(Encoding.UTF8.GetBytes(value)),
            WeakChecksumProvider = _ => false,
            TimestampProvider = _ => DateTime.UtcNow,
            ChangedTimestampProvider = _ => DateTime.UtcNow
        };

        options.ValidateOptions();

        var data = "hello-world";
        byte[] checksum = options.ChecksumProvider(data);
        Console.WriteLine($"Checksum length: {checksum.Length}"); // 32 bytes (SHA256)
        Console.WriteLine($"Created: {options.TimestampProvider(data)}");

}
}

```
