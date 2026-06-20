---
uid: Cuemon.Alphanumeric
example:
- *content
---

The following example demonstrates how to use the `Alphanumeric` class to access predefined character sets. It prints the available character ranges for digits, uppercase letters, hexadecimal characters, and punctuation marks.

```csharp
using System;
using Cuemon;

namespace MyApp.CharacterSets;

public class AlphanumericExample
{
    public void Demonstrate()
    {
        Console.WriteLine(Alphanumeric.Numbers);           // "0123456789"
        Console.WriteLine(Alphanumeric.UppercaseLetters);  // "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
        Console.WriteLine(Alphanumeric.Hexadecimal);       // "0123456789ABCDEF"
        Console.WriteLine(Alphanumeric.PunctuationMarks);  // "!@#$%^&*()_-+=[{]};:<>|.,/?`~\"'"
    }
}
```
