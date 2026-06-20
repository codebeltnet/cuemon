---
uid: Cuemon.StringReplacePair
example:
- *content
---

The following example demonstrates how to use the <see cref="StringReplacePair"/> struct to perform bulk string replacement and removal operations.

```csharp
using System;
using System.Collections.Generic;
using Cuemon; // for StringReplacePair

namespace MyApp.Examples;

public class StringReplacePairExample
{
    public void Demonstrate()
    {
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
        // Output: Hi Universe from Codebelt! Welcome to the Universe of dotnet.

        // Remove specific words (case-sensitive ordinal comparison)
        result = StringReplacePair.RemoveAll(input, StringComparison.Ordinal, "World", "Cuemon", "Welcome");
        Console.WriteLine(result);
        // Output: Hello  from !  to the  of .NET.

        // Remove specific characters
        result = StringReplacePair.RemoveAll(input, '.', '!');
        Console.WriteLine(result);
        // Output: Hello World from Cuemon Welcome to the World of NET

}
}

```
