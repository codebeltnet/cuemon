---
uid: Cuemon.Security.Cryptography.UnkeyedHashFactory
example:
- *content
---

The following example demonstrates how to compute SHA-256 and SHA-512 hashes for data integrity using `UnkeyedHashFactory`. It hashes a sample input and prints the hexadecimal digest for each algorithm.

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
