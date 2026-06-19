---
uid: Cuemon.AspNetCore.Http.Headers.ChecksumBuilderDecoratorExtensions
example:
- *content
---

The following example demonstrates how to create an `EntityTagHeaderValue` from a `ChecksumBuilder` using the decorator pattern.

```csharp
using System;
using Cuemon;
using Cuemon.Data.Integrity;
using Cuemon.Security;

        namespace Cuemon.AspNetCore.Http.Headers;

        public static class ChecksumBuilderDecoratorExtensionsExample
        {
            public static void Demonstrate()
            {
                var builder = new ChecksumBuilder(() => HashFactory.CreateFnv128());
        var entityTag = Decorator.Enclose(builder).ToEntityTagHeaderValue(isWeak: true);

        Console.WriteLine(entityTag.ToString());
            }
        }
```
