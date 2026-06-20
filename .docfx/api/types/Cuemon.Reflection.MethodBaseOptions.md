---
uid: Cuemon.Reflection.MethodBaseOptions
example:
- *content
---

`MethodBaseOptions` stores method lookup rules including binding flags, string comparison mode, and expected parameter types for use during reflection-based method resolution. This example configures options with `BindingFlags.Instance | BindingFlags.Public`, `StringComparison.OrdinalIgnoreCase`, and `typeof(decimal)` parameter types, then passes them to a custom `ResolveMethod` helper that searches the `PricingEngine` class for a method matching the name and parameter signature. Console output shows the resolved method name (`ApplyDiscount`) or `"not found"`, the binding flags, and the expected parameter type names (`Decimal, Decimal`).

```csharp
using System;
using System.Linq;
using System.Reflection;
using Cuemon.Reflection;

namespace MyApp.Examples;

public static class MethodBaseOptionsExample
{
    public static void Demonstrate()
    {
        var options = new MethodBaseOptions
        {
            Flags = BindingFlags.Instance | BindingFlags.Public,
            Comparison = StringComparison.OrdinalIgnoreCase,
            Types = new[] { typeof(decimal), typeof(decimal) }
        };

        MethodInfo method = ResolveMethod(typeof(PricingEngine), "applydiscount", options);

        Console.WriteLine(method == null ? "not found" : method.Name);
        Console.WriteLine(options.Flags);
        Console.WriteLine(string.Join(", ", options.Types.Select(type => type.Name)));
    }

    private static MethodInfo ResolveMethod(Type source, string name, MethodBaseOptions options)
    {
        return source.GetMethods(options.Flags).FirstOrDefault(method =>
            method.Name.Equals(name, options.Comparison) &&
            method.GetParameters().Select(parameter => parameter.ParameterType)
                .SequenceEqual(options.Types ?? Array.Empty<Type>()));
    }
}

public sealed class PricingEngine
{
    public decimal ApplyDiscount(decimal subtotal, decimal discount)
    {
        return subtotal - discount;
    }
}
```
