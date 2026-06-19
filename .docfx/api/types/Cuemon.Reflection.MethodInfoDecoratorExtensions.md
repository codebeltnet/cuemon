---
uid: Cuemon.Reflection.MethodInfoDecoratorExtensions
example:
- *content
---

The following example shows how to detect whether a reflected method overrides a base implementation.

```csharp
using System;
using System.Reflection;
using Cuemon;
using Cuemon.Reflection;

namespace MyApp.Examples;

public static class MethodInfoDecoratorExtensionsExample
{
    public static void Demonstrate()
    {
        MethodInfo baseMethod = typeof(PricingCalculator).GetMethod(nameof(PricingCalculator.Calculate))!;
        MethodInfo derivedMethod = typeof(RegionalPricingCalculator).GetMethod(nameof(PricingCalculator.Calculate))!;
        MethodInfo localMethod = typeof(RegionalPricingCalculator).GetMethod(nameof(RegionalPricingCalculator.FormatRegion))!;

        Console.WriteLine(Decorator.Enclose(baseMethod).IsOverridden());
        Console.WriteLine(Decorator.Enclose(derivedMethod).IsOverridden());
        Console.WriteLine(Decorator.Enclose(localMethod).IsOverridden());
    }
}

public class PricingCalculator
{
    public virtual decimal Calculate(decimal subtotal)
    {
        return subtotal;
    }
}

public sealed class RegionalPricingCalculator : PricingCalculator
{
    public override decimal Calculate(decimal subtotal)
    {
        return subtotal * 1.25m;
    }

    public string FormatRegion()
    {
        return "EU";
    }
}
```
