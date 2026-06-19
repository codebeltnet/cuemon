---
uid: Cuemon.Reflection.PropertyInfoDecoratorExtensions
example:
- *content
---

The following example shows how to inspect reflected properties for auto-property and override behavior.

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
