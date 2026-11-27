---
uid: Cuemon.TesterFuncFactory`3
example:
- *content
---

The following example demonstrates how to use `TesterFuncFactory<TArgs, TResult, TSuccess>` to encapsulate a tester function with its arguments for deferred execution with output extraction.

```csharp
// Program.cs
using System;
using Cuemon;
using Contoso.Validation;

TesterFuncFactoryExample.Run();

namespace Contoso.Validation
{
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
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="86260d5e2fdd88f222f3ccef"><a href="~/run-actions/86260d5e2fdd88f222f3ccef/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/86260d5e2fdd88f222f3ccef/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
