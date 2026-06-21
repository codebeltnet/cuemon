---
uid: Cuemon.AspNetCore.Diagnostics.HttpExceptionDescriptor
example:
- *content
---

The following example demonstrates creating an HTTP exception descriptor with diagnostic evidence.

```csharp
using System;
using Cuemon.AspNetCore.Http;

        namespace Cuemon.AspNetCore.Diagnostics;

        public static class HttpExceptionDescriptorExample
        {
            public static void Demonstrate()
            {
                var descriptor = new HttpExceptionDescriptor(new BadRequestException())
        {
            Instance = new Uri("urn:request:42"),
            RequestId = "req-42",
            CorrelationId = "corr-42"
        };

        Console.WriteLine($"{descriptor.StatusCode} {descriptor.Message}");
            }
        }
```
