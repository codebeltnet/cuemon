---
uid: Cuemon.Collections.Generic.DynamicComparer
example:
- *content
---

The following example shows how to create custom `IComparer<T>` instances using `DynamicComparer` with lambda expressions. It demonstrates sorting by string length and in descending order, printing each sorted array.

```csharp
// Program.cs
using System;
using System.Collections.Generic;
using System.Linq;
using Cuemon.Collections.Generic;

string[] fruits = ["apple", "pear", "banana", "kiwi"];

// Create a dynamic comparer that sorts by string length
IComparer<string> lengthComparer = DynamicComparer.Create<string>((x, y) =>
    x.Length.CompareTo(y.Length));

Array.Sort(fruits, lengthComparer);
Console.WriteLine(string.Join(", ", fruits));

// Create a comparer that sorts descending
IComparer<string> descendingComparer = DynamicComparer.Create<string>((x, y) =>
    string.Compare(y, x, StringComparison.Ordinal));

Array.Sort(fruits, descendingComparer);
Console.WriteLine(string.Join(", ", fruits));
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="b1fe980dc799326bfed8dccf"><a href="~/run-actions/b1fe980dc799326bfed8dccf/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/b1fe980dc799326bfed8dccf/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
