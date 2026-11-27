---
uid: Cuemon.Reflection.MemberParser
example:
- *content
---

The following example shows how to hydrate a type from named reflection arguments. The sample supplies named constructor and property arguments, selects a two-parameter constructor, and prints the hydrated values plus the processed argument names.

```csharp
// Program.cs
using System;
using System.Collections.Generic;
using System.Linq;
using Cuemon.Reflection;
using MyApp.Examples;

MemberParserExample.Demonstrate();

namespace MyApp.Examples
{
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
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="f87695fb8519d321c37f8de4"><a href="~/run-actions/f87695fb8519d321c37f8de4/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/f87695fb8519d321c37f8de4/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
