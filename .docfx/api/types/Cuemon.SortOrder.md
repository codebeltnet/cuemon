---
uid: Cuemon.SortOrder
example:
- *content
---

The following example demonstrates how to use the <see cref="SortOrder"/> enum to indicate the direction of a sort operation in a custom comparer.

```csharp
// Program.cs
using System;
using System.Collections.Generic;
using Cuemon;
using Contoso.Search;

SortOrderExample.Run();

namespace Contoso.Search
{
    public sealed class SortOrderExample
    {
        public static void Run()
        {
            var names = new List<string> { "Charlie", "Alice", "Bob" };

            Apply(names, SortOrder.Ascending);
            Console.WriteLine(string.Join(", ", names));

            Apply(names, SortOrder.Descending);
            Console.WriteLine(string.Join(", ", names));
        }

        private static void Apply(List<string> names, SortOrder sortOrder)
        {
            names.Sort((left, right) =>
            {
                int result = string.Compare(left, right, StringComparison.Ordinal);
                return sortOrder == SortOrder.Descending ? -result : result;
            });
        }
    }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="8ed39f1d9706467796de299d"><a href="~/run-actions/8ed39f1d9706467796de299d/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/8ed39f1d9706467796de299d/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
