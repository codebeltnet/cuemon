---
uid: Cuemon.Alphanumeric
example:
- *content
---

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
