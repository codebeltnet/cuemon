---
uid: Cuemon.Extensions.AspNetCore.Configuration.AssemblyCacheBustingOptions
example:
- *content
---

The following example demonstrates how to configure `AssemblyCacheBustingOptions` for a reproducible cache-busting version that is derived from a known assembly and hash algorithm.

```csharp
using System;
using Cuemon;
using Cuemon.Extensions.AspNetCore.Configuration;
using Cuemon.Security.Cryptography;
using Microsoft.Extensions.Options;

namespace DocfxExamples;

public class AssemblyCacheBustingOptionsExample
{
    public static void Demonstrate()
    {
        var options = Options.Create(new AssemblyCacheBustingOptions
        {
            Assembly = typeof(AssemblyCacheBustingOptionsExample).Assembly,
            Algorithm = UnkeyedCryptoAlgorithm.Sha256,
            PreferredCasing = CasingMethod.UpperCase,
            ReadByteForByteChecksum = true
        });

        var cacheBusting = new AssemblyCacheBusting(options);

        Console.WriteLine(cacheBusting.Version);
    }
}
```
