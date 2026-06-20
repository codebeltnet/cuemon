---
uid: Cuemon.Security.Cryptography.MessageDigest5
example:
- *content
---

The following example demonstrates how to compute an MD5 hash with <see cref="MessageDigest5" />.

```csharp
using System;
using System.Text;
using Cuemon.Security.Cryptography;

namespace MyApp.Examples;

public static class MessageDigest5Example
{
    public static void Demonstrate()
    {
        var algorithm = new MessageDigest5();
        var result = algorithm.ComputeHash(Encoding.UTF8.GetBytes("The quick brown fox jumps over the lazy dog"));

        Console.WriteLine(MessageDigest5.BitSize);
        Console.WriteLine(result.GetBytes().Length);
        Console.WriteLine(result.ToHexadecimalString());
    }
}
```
