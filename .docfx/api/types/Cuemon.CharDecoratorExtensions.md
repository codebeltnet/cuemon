---
uid: Cuemon.CharDecoratorExtensions
example:
- *content
---

`CharDecoratorExtensions` provides extension methods on `Decorator.Enclose` for converting between `IEnumerable<char>` sequences and string collections. This example wraps the characters of `"Hello"` and calls `ToEnumerable` to split them into single-character strings, then `ToStringEquivalent` to rejoin them back into the original string. It also demonstrates the same round-trip with a `char[]` array of `'A'`, `'B'`, `'C'`. Console output confirms the split produces `"H, e, l, l, o"` and the rejoined result matches `"Hello"` and `"ABC"`.

```csharp
// Program.cs
using System;
using System.Collections.Generic;
using System.Linq;
using Cuemon;

// Convert a sequence of characters to a sequence of single-character strings
IEnumerable<char> characters = "Hello".AsEnumerable();

IEnumerable<string> strings = Decorator.Enclose(characters).ToEnumerable();
Console.WriteLine(string.Join(", ", strings)); // "H, e, l, l, o"

// Convert a sequence of characters back to a single string
string result = Decorator.Enclose(characters).ToStringEquivalent();
Console.WriteLine(result); // "Hello"

// Works with any IEnumerable<char> including char arrays
char[] charArray = { 'A', 'B', 'C' };
string joined = Decorator.Enclose(charArray.AsEnumerable()).ToStringEquivalent();
Console.WriteLine(joined); // "ABC"
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="74c56cb40ddb896e973df09b"><a href="~/run-actions/74c56cb40ddb896e973df09b/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/74c56cb40ddb896e973df09b/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
