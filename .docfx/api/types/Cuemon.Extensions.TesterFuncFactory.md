---
uid: Cuemon.Extensions.TesterFuncFactory
example:
- *content
---

The following example demonstrates how to create a `TesterFuncFactory` from a TryParse-style delegate. It shows both a successful parse and a failure case, printing the parsed value or fallback accordingly.

```csharp
using System;
using Cuemon;
using Cuemon.Extensions;

namespace Cuemon.Extensions;

public class TesterFuncFactoryExample
{
    public void Demonstrate()
    {
        TesterFunc<string, int, bool> tryParse = (string input, out int result) => int.TryParse(input, out result);

        var factory = TesterFuncFactory.Create(tryParse, "42");
        bool success = factory.ExecuteMethod(out int value);
        Console.WriteLine($"Parsed: {value}, Success: {success}");

        var failFactory = TesterFuncFactory.Create(tryParse, "not-a-number");
        bool failed = failFactory.ExecuteMethod(out int fallback);
        Console.WriteLine($"Fallback: {fallback}, Success: {failed}");
    }
}
```
