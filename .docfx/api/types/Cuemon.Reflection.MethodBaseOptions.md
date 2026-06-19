---
uid: Cuemon.Reflection.MethodBaseOptions
example:
- *content
---

The following example shows how to store method lookup rules in `MethodBaseOptions` and apply them during reflection.

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
