---
uid: Cuemon.Collections.Generic.Arguments
example:
- *content
---

The following example demonstrates how to use the `Arguments` class to create arrays and enumerables from argument lists. It shows array concatenation, single-element yielding, and object-based overloads.

```csharp
// Program.cs
using System;
using System.Collections.Generic;
using System.Linq;
using Cuemon.Collections.Generic;

int[] first = Arguments.ToArrayOf(1, 2, 3);
int[] second = Arguments.ToArrayOf(4, 5, 6);

// Concat two arrays
int[] combined = Arguments.Concat(first, second);
Console.WriteLine(string.Join(", ", combined));

// Yield a single element
IEnumerable<int> yielded = Arguments.Yield(42);
Console.WriteLine(yielded.First());

// Convert to IEnumerable
IEnumerable<string> enumerable = Arguments.ToEnumerableOf("a", "b", "c");
Console.WriteLine(string.Concat(enumerable));

// Object overloads
object[] objs = Arguments.ToArray(1, "two", 3.0);
IEnumerable<object> objEnumerable = Arguments.ToEnumerable(true, false);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="8e728635675872aff45149b3"><a href="~/run-actions/8e728635675872aff45149b3/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/8e728635675872aff45149b3/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
