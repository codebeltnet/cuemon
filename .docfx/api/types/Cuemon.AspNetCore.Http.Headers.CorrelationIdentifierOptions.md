---
uid: Cuemon.AspNetCore.Http.Headers.CorrelationIdentifierOptions
example:
- *content
---

The following example shows how to configure `CorrelationIdentifierOptions` with a custom correlation token. After validation, it prints the token's correlation ID.

```csharp
using System;
using Cuemon.Messaging;

        namespace Cuemon.AspNetCore.Http.Headers;

        public static class CorrelationIdentifierOptionsExample
        {
            public static void Demonstrate()
            {
                var options = new CorrelationIdentifierOptions
        {
            Token = new CorrelationToken("corr-42")
        };

        options.ValidateOptions();
        Console.WriteLine(options.Token.CorrelationId);
            }
        }
```
