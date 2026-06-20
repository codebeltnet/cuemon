---
uid: Cuemon.Security.Cryptography.HmacMessageDigest5
example:
- *content
---

`HmacMessageDigest5` computes and verifies HMAC-MD5 message authentication codes with support for custom byte order and direct string input. This example creates a 64-byte secret key, computes the HMAC of `"Important: Transfer $100 to account 12345"`, then verifies it by recomputing with the same secret. A tampered message with a different account number produces a different HMAC, confirming tamper detection. It also demonstrates options-based construction with `Endianness.LittleEndian` and direct `ComputeHash(string)` overload. Console output shows the hex and base64 HMAC values, verification status (`True`), tamper detection (`False`), and options-based result.

```csharp
using System;
using System.Text;
using Cuemon.Security;
using Cuemon.Security.Cryptography;

using Cuemon;
namespace MyApp.Security
{
    public static class HmacMessageDigest5Examples
    {
        public static void Demonstrate()
        {
            // Define a secret key (minimum 64 bytes recommended for HMAC-MD5).
            byte[] secret = Encoding.UTF8.GetBytes(
                "ThisIsASecretKeyThatShouldBeAtLeastSixtyFourBytesLongForBestResults1234567");

            // Create an HMAC-MD5 instance with the secret.
            var hmacMd5 = new HmacMessageDigest5(secret, null);

            // Compute the HMAC of a message.
            byte[] message = Encoding.UTF8.GetBytes("Important: Transfer $100 to account 12345");
            HashResult hash = hmacMd5.ComputeHash(message);

            Console.WriteLine("HMAC-MD5 (hex): {0}", hash.ToHexadecimalString());
            Console.WriteLine("HMAC-MD5 (base64): {0}", hash.ToBase64String());

            // Verify the HMAC by recomputing with the same secret.
            var verifier = new HmacMessageDigest5(secret, null);
            HashResult expected = verifier.ComputeHash(message);
            bool isValid = hash.Equals(expected);
            Console.WriteLine("HMAC valid: {0}", isValid); // true

            // Tampered message produces a different HMAC.
            byte[] tampered = Encoding.UTF8.GetBytes("Important: Transfer $100 to account 99999");
            HashResult tamperedHash = hmacMd5.ComputeHash(tampered);
            Console.WriteLine("Tampered HMAC matches: {0}", hash.Equals(tamperedHash)); // false

            // Compute HMAC from string directly (UTF-8 encoding by default).
            HashResult fromString = hmacMd5.ComputeHash("Hello, HMAC!");
            Console.WriteLine("String HMAC: {0}", fromString);
        }

        public static void DemonstrateWithOptions()
        {
            byte[] secret = Encoding.UTF8.GetBytes("my-secret-key");
            var hmacMd5 = new HmacMessageDigest5(secret, o =>
            {
                o.ByteOrder = Endianness.LittleEndian;
            });
            HashResult result = hmacMd5.ComputeHash(Encoding.UTF8.GetBytes("test data"));
            Console.WriteLine("HMAC-MD5 (little-endian): {0}", result.ToHexadecimalString());
        }
    }
}
```
