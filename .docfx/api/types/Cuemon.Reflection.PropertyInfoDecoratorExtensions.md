---
uid: Cuemon.Reflection.PropertyInfoDecoratorExtensions
example:
- *content
---

`PropertyInfoDecoratorExtensions` provides extension methods on `Decorator.Enclose` for detecting auto-implemented properties and override behavior via `IsAutoProperty` and `IsOverridden`. This example retrieves `PropertyInfo` for `Product.Code` (auto-property), `Product.Label` (expression-bodied), and `FeaturedProduct.Summary` (overridden in a derived class), then calls `IsAutoProperty` on the first two and `IsOverridden` on the third. Console output shows `True` for the auto-property, `False` for the expression-bodied property, and `True` for the overridden property, confirming correct identification of property characteristics.

```csharp
using System;
using System.Reflection;
using Cuemon;
using Cuemon.Reflection;

namespace MyApp.Examples;

public static class PropertyInfoDecoratorExtensionsExample
{
    public static void Demonstrate()
    {
        PropertyInfo code = typeof(Product).GetProperty(nameof(Product.Code))!;
        PropertyInfo label = typeof(Product).GetProperty(nameof(Product.Label))!;
        PropertyInfo summary = typeof(FeaturedProduct).GetProperty(nameof(Product.Summary))!;

        Console.WriteLine(Decorator.Enclose(code).IsAutoProperty());
        Console.WriteLine(Decorator.Enclose(label).IsAutoProperty());
        Console.WriteLine(Decorator.Enclose(summary).IsOverridden());
    }
}

public class Product
{
    public string Code { get; set; } = string.Empty;

    public string Label => $"Product:{Code}";

    public virtual string Summary => "standard";
}

public sealed class FeaturedProduct : Product
{
    public override string Summary => "featured";
}
```
