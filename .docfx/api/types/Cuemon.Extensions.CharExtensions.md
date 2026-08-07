---
uid: Cuemon.Extensions.CharExtensions
example:
- *content
---

The following example demonstrates converting sequences of <see cref="char"/> values to strings and string sequences using the [ToEnumerable](https://docs.cuemon.net/api/extensions/dotnet/Cuemon.Extensions.CharExtensions.html#Cuemon_Extensions_CharExtensions_ToEnumerable_System_Collections_Generic_IEnumerable_System_Char__) and [FromChars](https://docs.cuemon.net/api/extensions/dotnet/Cuemon.Extensions.CharExtensions.html#Cuemon_Extensions_CharExtensions_FromChars_System_Collections_Generic_IEnumerable_System_Char__) extension methods.

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
