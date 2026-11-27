---
uid: Cuemon.Extensions.CharExtensions
example:
- *content
---

The following example demonstrates converting sequences of <see cref="char"/> values to strings and string sequences using the [ToEnumerable](https://docs.cuemon.net/api/extensions/dotnet/Cuemon.Extensions.CharExtensions.html#Cuemon_Extensions_CharExtensions_ToEnumerable_System_Collections_Generic_IEnumerable_System_Char__) and [FromChars](https://docs.cuemon.net/api/extensions/dotnet/Cuemon.Extensions.CharExtensions.html#Cuemon_Extensions_CharExtensions_FromChars_System_Collections_Generic_IEnumerable_System_Char__) extension methods.

```csharp
// Program.cs
using System;
using System.Collections.Generic;
using System.Linq;
using Cuemon.Extensions;

char[] chars = "Hello World".ToCharArray();
string text = chars.FromChars();
IEnumerable<string> strings = chars.ToEnumerable();
string alphabet = Enumerable.Range('A', 26).Select(c => (char)c).FromChars();

Console.WriteLine(text);
Console.WriteLine(strings.Count());
Console.WriteLine(alphabet);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="5ea41b079f807f48372f84a7"><a href="~/run-actions/5ea41b079f807f48372f84a7/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/5ea41b079f807f48372f84a7/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
