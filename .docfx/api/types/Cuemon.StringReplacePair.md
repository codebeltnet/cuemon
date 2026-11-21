---
uid: Cuemon.StringReplacePair
example:
- *content
---

The following example demonstrates how to use the <see cref="StringReplacePair"/> struct to perform bulk string replacement and removal operations.

```csharp
// Program.cs
using System;
using System.Collections.Generic;
using Cuemon; // for StringReplacePair

string input = "Hello World from Cuemon! Welcome to the World of .NET.";

// Replace all occurrences of "World" with "Universe" (case-insensitive by default)
string result = StringReplacePair.ReplaceAll(input, "World", "Universe");
Console.WriteLine(result);
// Output: Hello Universe from Cuemon! Welcome to the Universe of .NET.

// Replace multiple pairs at once
var pairs = new StringReplacePair[]
{
    new StringReplacePair("Hello", "Hi"),
    new StringReplacePair("Cuemon", "Codebelt"),
    new StringReplacePair(".NET", "dotnet")
};
result = StringReplacePair.ReplaceAll(input, pairs);
Console.WriteLine(result);
// Output: Hi World from Codebelt! Welcome to the World of dotnet.

// Remove specific words (case-sensitive ordinal comparison)
result = StringReplacePair.RemoveAll(input, StringComparison.Ordinal, "World", "Cuemon", "Welcome");
Console.WriteLine(result);
// Output: Hello  from !  to the  of .NET.

// Remove specific characters
result = StringReplacePair.RemoveAll(input, '.', '!');
Console.WriteLine(result);
// Output: Hello World from Cuemon Welcome to the World of NET
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="2ab423a8efab4e86e799dab4"><a href="~/run-actions/2ab423a8efab4e86e799dab4/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/2ab423a8efab4e86e799dab4/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
