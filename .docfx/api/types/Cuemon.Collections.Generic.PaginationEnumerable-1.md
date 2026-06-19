---
uid: Cuemon.Collections.Generic.PaginationEnumerable`1
example:
- *content
---

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using Cuemon.Collections.Generic;

namespace MyApp.Examples
{
    public class PaginationEnumerableExample
    {
        public static void Demonstrate()
        {
            var fruits = new[] { "Apple", "Banana", "Cherry", "Date", "Elderberry", "Fig", "Grape", "Honeydew" };

            // Show page 2 with 3 items per page
            var page = new PaginationEnumerable<string>(fruits, () => fruits.Length, setup =>
            {
                setup.PageSize = 3;
                setup.PageNumber = 2;
            });

            Console.WriteLine($"Page {2} of {page.PageCount} (total items: {page.TotalElementCount})");
            Console.WriteLine($"Has previous page: {page.HasPreviousPage}");
            Console.WriteLine($"Has next page: {page.HasNextPage}");
            Console.WriteLine("Items on this page:");
            foreach (var item in page)
            {
                Console.WriteLine($"  {item}");

}}}
}

```
