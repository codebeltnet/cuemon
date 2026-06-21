---
uid: Cuemon.UnsuccessfulValue
example:
- *content
---

The following example demonstrates how to use `UnsuccessfulValue` to represent a void operation that failed with an exception, providing a consistent way to signal failure without throwing.

```csharp
using System;
using Cuemon;

namespace Contoso.Connections;

public sealed class UnsuccessfulValueExample
{
    public static void Run()
    {
        ConditionalValue outcome = OpenConnection(null);

        Console.WriteLine($"Succeeded: {outcome.Succeeded}");
        Console.WriteLine($"Failure: {outcome.Failure?.GetType().Name}");
    }

    private static ConditionalValue OpenConnection(string connectionString)
    {
        try
        {
            Validator.ThrowIfNullOrWhitespace(connectionString);
            return new SuccessfulValue();
        }
        catch (Exception ex)
        {
            return new UnsuccessfulValue(ex);
        }
    }
}
```
