---
uid: Cuemon.Security.Cryptography.AesKeyOptions
example:
- *content
---

The following example demonstrates how to configure AesKeyOptions to specify the key size and custom random string provider when generating AES keys.

```csharp
using System;
using Cuemon.Security.Cryptography;

namespace MyApp.Security.Cryptography
{
    public class AesKeyOptionsExamples
    {
        public static void ConfigureKeySize()
        {
            // Default is AesSize.Aes256 (32 bytes).
            var defaultOptions = new AesKeyOptions();
            Console.WriteLine("Default key size: {0}", defaultOptions.Size); // Aes256

            // Generate a 128-bit key with AesKeyOptions.
            byte[] key128 = AesCryptor.GenerateKey(o =>
            {
                o.Size = AesSize.Aes128;
            });
            Console.WriteLine("128-bit key length: {0} bytes", key128.Length); // 16

            // Generate a 192-bit key.
            byte[] key192 = AesCryptor.GenerateKey(o =>
            {
                o.Size = AesSize.Aes192;
            });
            Console.WriteLine("192-bit key length: {0} bytes", key192.Length); // 24

            // Generate a 256-bit key explicitly.
            byte[] key256 = AesCryptor.GenerateKey(o =>
            {
                o.Size = AesSize.Aes256;
            });
            Console.WriteLine("256-bit key length: {0} bytes", key256.Length); // 32
        }

        public static void CustomRandomStringProvider()
        {
            // Replace the default random string provider with a custom one.
            // The provider is used internally when generating passphrase-based keys.
            byte[] key = AesCryptor.GenerateKey(o =>
            {
                o.Size = AesSize.Aes128;
                o.RandomStringProvider = size => new string('X', size == AesSize.Aes128 ? 16 : size == AesSize.Aes192 ? 24 : 32);
            });

            Console.WriteLine("Custom provider key length: {0} bytes", key.Length); // 16
        }

        public static void InspectDefaultProperties()
        {
            var options = new AesKeyOptions();
            Console.WriteLine("Default size:                {0}", options.Size); // Aes256
            Console.WriteLine("RandomStringProvider != null: {0}", options.RandomStringProvider != null); // true
        }
    }
}

```
