---
uid: Cuemon.Extensions.AspNetCore.Data.Integrity.CacheValidatorExtensions
example:
- *content
---

The following example demonstrates how to turn assembly-backed cache validators into ETag headers for weak and strong validation scenarios.

```csharp
using System;
using Cuemon.Data.Integrity;
using Cuemon.Extensions.AspNetCore.Data.Integrity;
using Microsoft.Net.Http.Headers;

namespace DocfxExamples;

public class CacheValidatorExtensionsExample
{
    public static void Demonstrate()
    {
        CacheValidator weakValidator = CacheValidatorFactory.CreateValidator(typeof(CacheValidatorExtensionsExample).Assembly);
        EntityTagHeaderValue weakEntityTag = weakValidator.ToEntityTagHeaderValue();

        CacheValidator strongValidator = CacheValidatorFactory.CreateValidator(
            typeof(CacheValidatorExtensionsExample).Assembly,
            setup: options => options.BytesToRead = int.MaxValue);

        EntityTagHeaderValue strongEntityTag = strongValidator.ToEntityTagHeaderValue();

        Console.WriteLine(weakEntityTag);
        Console.WriteLine(strongEntityTag);
    }
}
```
