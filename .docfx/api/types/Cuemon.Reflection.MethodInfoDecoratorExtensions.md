---
uid: Cuemon.Reflection.MethodInfoDecoratorExtensions
example:
- *content
---

`MethodInfoDecoratorExtensions` provides extension methods on `Decorator.Enclose` for detecting whether a reflected method overrides a base class implementation via `IsOverridden`. This example retrieves `MethodInfo` for `PricingCalculator.Calculate` (base method), `RegionalPricingCalculator.Calculate` (overridden), and `RegionalPricingCalculator.FormatRegion` (local method), then calls `IsOverridden()` on each. Console output shows `False` for the base method, `True` for the overridden method, and `False` for the local method that does not override anything.

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
