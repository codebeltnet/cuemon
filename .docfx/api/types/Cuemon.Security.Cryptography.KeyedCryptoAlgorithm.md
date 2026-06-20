---
uid: Cuemon.Security.Cryptography.KeyedCryptoAlgorithm
example:
- *content
---

`KeyedCryptoAlgorithm` is an enumeration that selects an HMAC algorithm (SHA-256, SHA-384, SHA-512, SHA-1, or MD5) for keyed hashing operations. This example computes HMAC hashes for each algorithm using a shared key and sample data (`"The quick brown fox jumps over the lazy dog"`) with `HMACSHA256`, `HMACSHA384`, `HMACSHA512`, and `HMACSHA1`. It also uses a `KeyedCryptoAlgorithm` value in a switch expression to dynamically select the algorithm name, and demonstrates enum value comparison to confirm `HmacSha256 == 0`. Console output displays each hex hash and the selected algorithm name.

```csharp
using System;
using System.Security.Cryptography;
using System.Text;
using Cuemon.Security.Cryptography;

namespace MyApp.Security
{
    public class KeyedCryptoAlgorithmExample
    {
        public void Demonstrate()
        {
            // KeyedCryptoAlgorithm specifies the HMAC algorithm to use.
            // HmacMd5     = -2  (HMAC-MD5,   128 bits)
            // HmacSha1    = -1  (HMAC-SHA1,  160 bits)
            // HmacSha256  =  0  (HMAC-SHA256,256 bits)
            // HmacSha384  =  1  (HMAC-SHA384,384 bits)
            // HmacSha512  =  2  (HMAC-SHA512,512 bits)

            byte[] key = Encoding.UTF8.GetBytes("my-secret-key");
            byte[] data = Encoding.UTF8.GetBytes("The quick brown fox jumps over the lazy dog");

            // HMAC-SHA256 (default/recommended)
            using var hmacSha256 = new HMACSHA256(key);
            byte[] hash256 = hmacSha256.ComputeHash(data);
            Console.WriteLine($"HmacSha256: {BitConverter.ToString(hash256).Replace("-", "").ToLowerInvariant()}");

            // HMAC-SHA384
            using var hmacSha384 = new HMACSHA384(key);
            byte[] hash384 = hmacSha384.ComputeHash(data);
            Console.WriteLine($"HmacSha384: {BitConverter.ToString(hash384).Replace("-", "").ToLowerInvariant()}");

            // HMAC-SHA512
            using var hmacSha512 = new HMACSHA512(key);
            byte[] hash512 = hmacSha512.ComputeHash(data);
            Console.WriteLine($"HmacSha512: {BitConverter.ToString(hash512).Replace("-", "").ToLowerInvariant()}");

            // HMAC-SHA1
            using var hmacSha1 = new HMACSHA1(key);
            byte[] hashSha1 = hmacSha1.ComputeHash(data);
            Console.WriteLine($"HmacSha1: {BitConverter.ToString(hashSha1).Replace("-", "").ToLowerInvariant()}");

            // Using KeyedCryptoAlgorithm to select algorithm dynamically
            KeyedCryptoAlgorithm algorithm = KeyedCryptoAlgorithm.HmacSha384;
            string algorithmName = algorithm switch
            {
                KeyedCryptoAlgorithm.HmacMd5 => "MD5",
                KeyedCryptoAlgorithm.HmacSha1 => "SHA1",
                KeyedCryptoAlgorithm.HmacSha256 => "SHA256",
                KeyedCryptoAlgorithm.HmacSha384 => "SHA384",
                KeyedCryptoAlgorithm.HmacSha512 => "SHA512",
                _ => "SHA256"
            };
            Console.WriteLine($"Selected HMAC algorithm: HMAC-{algorithmName}");

            // Enum value comparison
            Console.WriteLine($"HmacSha256 == 0: {KeyedCryptoAlgorithm.HmacSha256 == (KeyedCryptoAlgorithm)0}"); // True

}}
}

```
