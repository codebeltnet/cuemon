---
uid: Cuemon.TesterFuncFactory`3
example:
- *content
---

The following example demonstrates how to use `TesterFuncFactory<TArgs, TResult, TSuccess>` to encapsulate a tester function with its arguments for deferred execution with output extraction.

```csharp
using System;
using Cuemon;

namespace Contoso.Validation;

public sealed class TesterFuncFactoryExample
{
    public static void Run()
    {
        var factory = new TesterFuncFactory<MutableTuple<int, int, int>, string, bool>(
            (MutableTuple<int, int, int> tuple, out string result) =>
            {
                result = $"{tuple.Arg1},{tuple.Arg2},{tuple.Arg3}";
                return true;
            },
            new MutableTuple<int, int, int>(1, 2, 3));

        bool success = factory.ExecuteMethod(out string output);
        var clone = (TesterFuncFactory<MutableTuple<int, int, int>, string, bool>)factory.Clone();
        clone.ExecuteMethod(out string clonedOutput);

        Console.WriteLine($"Success: {success}");
        Console.WriteLine(output);
        Console.WriteLine(clonedOutput);
    }
}
```
