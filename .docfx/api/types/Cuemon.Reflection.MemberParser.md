---
uid: Cuemon.Reflection.MemberParser
example:
- *content
---

The following example shows how to hydrate a type from named reflection arguments.

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using Cuemon.Reflection;

namespace MyApp.Examples;

public static class MemberParserExample
{
    public static void Demonstrate()
    {
        var arguments = new List<MemberArgument>
        {
            new("sku", "SKU-42"),
            new("price", 19.95m),
            new("stock", 12)
        };

        var parser = new MemberParser(typeof(CatalogItem), arguments);
        var item = (CatalogItem)parser.CreateInstance(ctor => ctor.GetParameters().Length == 2);

        Console.WriteLine(item.Sku);
        Console.WriteLine(item.Price);
        Console.WriteLine(item.Stock);
        Console.WriteLine(string.Join(", ", parser.ProcessedMemberArguments.Select(argument => argument.Name)));
    }
}

public sealed class CatalogItem
{
    public CatalogItem(string sku, decimal price)
    {
        Sku = sku;
        Price = price;
    }

    public string Sku { get; }

    public decimal Price { get; }

    public int Stock { get; set; }
}
```
