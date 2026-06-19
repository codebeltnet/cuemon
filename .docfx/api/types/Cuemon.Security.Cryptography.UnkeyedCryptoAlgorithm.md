---
uid: Cuemon.Security.Cryptography.UnkeyedCryptoAlgorithm
example:
- *content
---

The following example demonstrates how to use the UnkeyedCryptoAlgorithm enumeration to select a hash algorithm (SHA-256, SHA-384, SHA-512, SHA-1, or MD5) for unkeyed hashing.

```csharp
using System;
using System.Security.Cryptography;
using System.Text;
using Cuemon.Security.Cryptography;

namespace MyApp.Security
{
    public class UnkeyedCryptoAlgorithmExample
    {
        public void Demonstrate()
        {
            // UnkeyedCryptoAlgorithm specifies the hash algorithm to use.
            // Md5           = -2  (MD5,       128 bits)
            // Sha1          = -1  (SHA-1,     160 bits)
            // Sha256        =  0  (SHA-256,   256 bits)
            // Sha384        =  1  (SHA-384,   384 bits)
            // Sha512        =  2  (SHA-512,   512 bits)
            // Sha512Slash256 = 3  (SHA-512/256, 256 bits)

            byte[] data = Encoding.UTF8.GetBytes("The quick brown fox jumps over the lazy dog");

            // SHA-256 (default/recommended)
            using var sha256 = SHA256.Create();
            byte[] hash256 = sha256.ComputeHash(data);
            Console.WriteLine($"Sha256: {BitConverter.ToString(hash256).Replace("-", "").ToLowerInvariant()}");

            // SHA-384
            using var sha384 = SHA384.Create();
            byte[] hash384 = sha384.ComputeHash(data);
            Console.WriteLine($"Sha384: {BitConverter.ToString(hash384).Replace("-", "").ToLowerInvariant()}");

            // SHA-512
            using var sha512 = SHA512.Create();
            byte[] hash512 = sha512.ComputeHash(data);
            Console.WriteLine($"Sha512: {BitConverter.ToString(hash512).Replace("-", "").ToLowerInvariant()}");

            // SHA-1
            using var sha1 = SHA1.Create();
            byte[] hashSha1 = sha1.ComputeHash(data);
            Console.WriteLine($"Sha1: {BitConverter.ToString(hashSha1).Replace("-", "").ToLowerInvariant()}");

            // Using UnkeyedCryptoAlgorithm to select algorithm dynamically
            UnkeyedCryptoAlgorithm algorithm = UnkeyedCryptoAlgorithm.Sha512;
            Console.WriteLine($"Selected algorithm: {algorithm}"); // Sha512

            // Enum value comparison
            Console.WriteLine($"Sha256 == 0: {UnkeyedCryptoAlgorithm.Sha256 == (UnkeyedCryptoAlgorithm)0}"); // True
            Console.WriteLine($"Bit size of Sha512: 512 bits");

}}
}

```
