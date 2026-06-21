---
uid: Cuemon.TesterFunc`2
example:
- *content
---

The following example demonstrates the shared workflow for the <see cref="TesterFunc{TResult, TSuccess}" /> family: call the delegate, capture the out value, and branch on the success result.

```csharp
using System;
using Cuemon;

namespace MyApp.Examples;

public static class TesterFuncExample
{
    public static void Demonstrate()
    {
        TesterFunc<int, bool> tryReadPort = (out int port) =>
        {
            var configuredPort = "8080";
            return int.TryParse(configuredPort, out port);
        };

        var success = tryReadPort(out var port);

        Console.WriteLine(success);
        Console.WriteLine(port);
    }
}
```
