---
uid: Cuemon.AspNetCore.Http.Headers.RequestIdentifierOptions
example:
- *content
---

The following example demonstrates how to configure <xref cref="Cuemon.AspNetCore.Http.Headers.RequestIdentifierOptions"/> to customize the Request-ID header name and token generator.

```csharp
using System;
using Cuemon.Messaging;

        namespace Cuemon.AspNetCore.Http.Headers;

        public static class RequestIdentifierOptionsExample
        {
            public static void Demonstrate()
            {
                var options = new RequestIdentifierOptions
        {
            Token = new RequestToken("req-42")
        };

        options.ValidateOptions();
        Console.WriteLine(options.Token.RequestId);
            }
        }
```
