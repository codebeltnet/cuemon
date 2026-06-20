---
uid: Cuemon.Collections.Generic.PaginationOptions
example:
- *content
---

The following example demonstrates how to configure pagination options with a custom page size and number. It shows both lazy (`PaginationEnumerable`) and eager (`PaginationList`) pagination, printing page metadata and specific items.

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using Cuemon.Collections.Generic;

namespace MyApp.Collections;

public class PaginationOptionsExample
{
    public void Demonstrate()
    {
        // Create a source sequence with 100 items
        var allItems = Enumerable.Range(1, 100).ToList();

        // Configure pagination options directly
        var options = new PaginationOptions { PageSize = 10, PageNumber = 3 };
        Console.WriteLine($"Page size: {options.PageSize}, Page number: {options.PageNumber}");

        // Paginate with 10 items per page, starting at page 3
        var page = new PaginationEnumerable<int>(allItems,
            () => allItems.Count,
            setup =>
            {
                setup.PageSize = 10;      // show 10 items per page
                setup.PageNumber = 3;     // go to page 3
            });

        Console.WriteLine($"Page 3 items: {string.Join(", ", page)}"); // 21..30
        Console.WriteLine($"Total pages: {page.PageCount}");            // 10
        Console.WriteLine($"Total items: {page.TotalElementCount}");    // 100
        Console.WriteLine($"Has next page: {page.HasNextPage}");        // True
        Console.WriteLine($"First page: {page.FirstPage}");            // False
        Console.WriteLine($"Last page: {page.LastPage}");              // False

        // Default PaginationOptions: page 1, 25 items per page
        var firstPage = new PaginationEnumerable<int>(allItems,
            () => allItems.Count);

        Console.WriteLine($"First page items: {string.Join(", ", firstPage)}"); // 1..25
        Console.WriteLine($"Has previous page: {firstPage.HasPreviousPage}");   // False

        // Using PaginationList for eager materialization with indexer access
        var thirdPage = new PaginationList<int>(allItems,
            () => allItems.Count,
            setup =>
            {
                setup.PageSize = 10;
                setup.PageNumber = 3;
            });

        Console.WriteLine($"Third item on page 3: {thirdPage[2]}"); // 23 (zero-based index)
        Console.WriteLine($"Page 3 count: {thirdPage.Count}");      // 10

}
}

```
