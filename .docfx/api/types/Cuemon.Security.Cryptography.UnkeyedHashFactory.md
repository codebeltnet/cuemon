---
uid: Cuemon.Security.Cryptography.UnkeyedHashFactory
example:
- *content
---

```csharp
using System;
using System.Text;
using Cuemon.Security.Cryptography;

namespace Cuemon.Security.Cryptography;

public class UnkeyedHashFactoryExample
{
    public void Demonstrate()
    {
        var sha256 = UnkeyedHashFactory.CreateCryptoSha256();
        var input = "Data to hash";
        var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
        Console.WriteLine($"SHA-256: {BitConverter.ToString(hash.GetBytes()).Replace("-", "")}");

        var sha512 = UnkeyedHashFactory.CreateCryptoSha512();
        hash = sha512.ComputeHash(Encoding.UTF8.GetBytes(input));
        Console.WriteLine($"SHA-512: {BitConverter.ToString(hash.GetBytes()).Replace("-", "")}");
    }
}
```
