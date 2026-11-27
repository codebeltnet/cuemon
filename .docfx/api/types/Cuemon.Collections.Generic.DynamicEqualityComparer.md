---
uid: Cuemon.Collections.Generic.DynamicEqualityComparer
example:
- *content
---

The following example shows how to create a case-insensitive `IEqualityComparer<string>` using `DynamicEqualityComparer` with lambda expressions. It filters distinct values from a mixed-case word list and prints the result.

```csharp
// Program.cs
using System;
using System.Collections.Generic;
using System.Linq;
using Cuemon.Collections.Generic;

string[] words = ["Hello", "world", "hello", "World", "HELLO"];

// Create a case-insensitive equality comparer for strings
IEqualityComparer<string> caseInsensitive = DynamicEqualityComparer.Create<string>(
    hashCalculator: s => StringComparer.OrdinalIgnoreCase.GetHashCode(s),
    equalityComparer: (x, y) => string.Equals(x, y, StringComparison.OrdinalIgnoreCase));

string[] distinct = words.Distinct(caseInsensitive).ToArray();
Console.WriteLine(string.Join(", ", distinct));
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="de32d97f5e42e87b0ed99e98"><a href="~/run-actions/de32d97f5e42e87b0ed99e98/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/de32d97f5e42e87b0ed99e98/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
