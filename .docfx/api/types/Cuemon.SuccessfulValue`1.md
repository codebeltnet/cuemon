---
uid: Cuemon.SuccessfulValue`1
example:
- *content
---

The following example demonstrates how to use `SuccessfulValue<TResult>` to represent a typed operation that completed successfully, pairing a result value with a success signal.

```csharp
using System;
using Cuemon;

namespace Contoso.Configuration;

public sealed class SuccessfulValueOfTResultExample
{
    public static void Run()
    {
        ConditionalValue<int> outcome = ParsePort("443");

        Console.WriteLine($"Succeeded: {outcome.Succeeded}");
        Console.WriteLine($"Port: {outcome.Result}");
    }

    private static ConditionalValue<int> ParsePort(string text)
    {
        if (int.TryParse(text, out int port))
        {
            return new SuccessfulValue<int>(port);
        }

        return new UnsuccessfulValue<int>(new FormatException("The port number is invalid."));
    }
}
```
