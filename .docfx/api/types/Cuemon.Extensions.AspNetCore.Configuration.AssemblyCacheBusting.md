---
uid: Cuemon.Extensions.AspNetCore.Configuration.AssemblyCacheBusting
example:
- *content
---

The following example demonstrates how to create an `AssemblyCacheBusting` instance to provide cache-busting version strings derived from the entry assembly.

```csharp
using System;
using Cuemon.Extensions.AspNetCore.Configuration;
using Cuemon.Security.Cryptography;
using Microsoft.Extensions.Options;

namespace MyApp.Examples;

public class AssemblyCacheBustingExample
{
    public void Demonstrate()
    {
        var options = Options.Create(new AssemblyCacheBustingOptions
        {
            Algorithm = UnkeyedCryptoAlgorithm.Sha256,
            ReadByteForByteChecksum = true
        });

        var cacheBusting = new AssemblyCacheBusting(options);
        string version = cacheBusting.Version;

        Console.WriteLine($"Cache-busting version: {version}");

}
}

```
