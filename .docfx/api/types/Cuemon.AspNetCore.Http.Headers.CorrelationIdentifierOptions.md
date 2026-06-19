---
uid: Cuemon.AspNetCore.Http.Headers.CorrelationIdentifierOptions
example:
- *content
---

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
