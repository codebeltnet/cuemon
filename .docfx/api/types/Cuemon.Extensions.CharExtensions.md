---
uid: Cuemon.Extensions.CharExtensions
example:
- *content
---

The following example demonstrates converting sequences of <see cref="char"/> values to strings and string sequences using the <xref:Cuemon.Extensions.CharExtensions.ToEnumerable(System.Collections.Generic.IEnumerable{char})> and <xref:Cuemon.Extensions.CharExtensions.FromChars(System.Collections.Generic.IEnumerable{char})> extension methods.

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using Cuemon.Extensions;

namespace MyApp.Examples;

public static class CharExtensionsExample
{
    public static void Demonstrate()
    {
        char[] chars = "Hello World".ToCharArray();
        string text = chars.FromChars();
        IEnumerable<string> strings = chars.ToEnumerable();
        string alphabet = Enumerable.Range('A', 26).Select(c => (char)c).FromChars();

        Console.WriteLine(text);
        Console.WriteLine(strings.Count());
        Console.WriteLine(alphabet);
    }
}

```
