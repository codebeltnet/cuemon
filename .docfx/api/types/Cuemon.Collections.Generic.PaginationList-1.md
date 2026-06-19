---
uid: Cuemon.Collections.Generic.PaginationList`1
example:
- *content
---

```csharp
using System;
using System.Collections.Generic;
using Cuemon.Collections.Generic;

namespace MyApp.Examples
{
    public class PaginationListExample
    {
        public static void Demonstrate()
        {
            var customers = new List<string>
            {
                "Alice", "Bob", "Carol", "Dave", "Eve",
                "Frank", "Grace", "Hank", "Iris", "Jack",
                "Kate", "Leo", "Mia", "Noah", "Olivia"
            };

            // Eagerly materialize page 3 with 5 items per page
            var page = new PaginationList<string>(customers, () => customers.Count, setup =>
            {
                setup.PageSize = 5;
                setup.PageNumber = 3;
            });

            Console.WriteLine($"Page items (count: {page.Count})");
            for (int i = 0; i < page.Count; i++)
            {
                Console.WriteLine($"  [{i}] {page[i]}");
            Console.WriteLine($"Total items: {page.TotalElementCount}");
            Console.WriteLine($"Total pages: {page.PageCount}");
            Console.WriteLine($"First page: {page.FirstPage}");
            Console.WriteLine($"Last page: {page.LastPage}");

}}}
}

```
