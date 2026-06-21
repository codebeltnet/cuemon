---
uid: Cuemon.Security.Cryptography.KeyedHashFactory
example:
- *content
---

The following example shows how to compute an HMAC-SHA256 hash for message authentication using `KeyedHashFactory`. It creates a keyed hash algorithm with a secret key, hashes an input string, and prints the hexadecimal digest.

```csharp
using System;
using System.Text;
using Cuemon.Security.Cryptography;

namespace Cuemon.Security.Cryptography;

public class KeyedHashFactoryExample
{
    public void Demonstrate()
    {
        var secret = Encoding.UTF8.GetBytes("my-secret-key");
        var hmac = KeyedHashFactory.CreateHmacCryptoSha256(secret);
        var input = "Message to authenticate";
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(input));
        Console.WriteLine($"HMAC-SHA256: {BitConverter.ToString(hash.To<byte[]>(bytes => bytes)).Replace("-", "")}");
    }
}
```
