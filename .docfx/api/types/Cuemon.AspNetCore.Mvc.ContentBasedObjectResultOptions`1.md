---
uid: Cuemon.AspNetCore.Mvc.ContentBasedObjectResultOptions`1
example:
- *content
---

The following example demonstrates how to configure <xref cref="Cuemon.AspNetCore.Mvc.ContentBasedObjectResultOptions{T}"/> to provide a checksum provider for generating ETags.

```csharp
using System;
using System.Security.Cryptography;
using System.Text;
using Cuemon.AspNetCore.Mvc;

namespace MyApp.Examples;

public class ContentBasedObjectResultOptionsExample
{
    public void Demonstrate()
    {
        var options = new ContentBasedObjectResultOptions<string>
        {
            ChecksumProvider = value => SHA256.HashData(Encoding.UTF8.GetBytes(value)),
            WeakChecksumProvider = _ => false // use strong ETag
        };

        // Validate that the required properties are configured
        options.ValidateOptions();

        Console.WriteLine("ChecksumProvider is configured.");

}
}

```
