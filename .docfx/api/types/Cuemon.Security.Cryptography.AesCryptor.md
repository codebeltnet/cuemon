---
uid: Cuemon.Security.Cryptography.AesCryptor
example:
- *content
---

`AesCryptor` provides AES encryption and decryption with configurable key size, cipher mode, and padding. This example generates a 256-bit key and 128-bit IV using `AesCryptor.GenerateKey()` and `GenerateInitializationVector()`, encrypts `"This is a sensitive message that needs encryption."`, decrypts the ciphertext, and verifies the round-trip matches the original. It also demonstrates explicit CBC mode with PKCS7 padding via an options delegate, custom key sizes (`AesSize.Aes128` and `AesSize.Aes192`), and the default constructor that generates random credentials automatically. Console output confirms key and IV lengths, base64 ciphertext, round-trip success, and explicit options round-trip verification.

```csharp
using System;
using System.Security.Cryptography;
using System.Text;
using Cuemon.Security.Cryptography;

namespace MyApp.Security
{
    public class AesCryptorExamples
    {
        public static void Demonstrate()
        {
            // Generate a 256-bit key and 128-bit initialization vector.
            byte[] key = AesCryptor.GenerateKey();
            byte[] iv = AesCryptor.GenerateInitializationVector();

            Console.WriteLine("Key length: {0} bytes ({1} bits)", key.Length, key.Length * 8);
            Console.WriteLine("IV length: {0} bytes ({1} bits)", iv.Length, iv.Length * 8);

            // Create an AesCryptor instance with the generated key and IV.
            var cryptor = new AesCryptor(key, iv);

            // Encrypt a secret message.
            byte[] plaintext = Encoding.UTF8.GetBytes("This is a sensitive message that needs encryption.");
            byte[] ciphertext = cryptor.Encrypt(plaintext);

            Console.WriteLine("Plaintext bytes: {0}", plaintext.Length);
            Console.WriteLine("Ciphertext (base64): {0}", Convert.ToBase64String(ciphertext));

            // Decrypt the ciphertext back to the original message.
            byte[] decrypted = cryptor.Decrypt(ciphertext);
            string roundtrip = Encoding.UTF8.GetString(decrypted);

            Console.WriteLine("Decrypted message: {0}", roundtrip); // matches original

            // Use explicit AES options (CBC mode, PKCS7 padding).
            byte[] ciphertextExplicit = cryptor.Encrypt(plaintext, o =>
            {
                o.Mode = CipherMode.CBC;
                o.Padding = PaddingMode.PKCS7;
            });

            byte[] decryptedExplicit = cryptor.Decrypt(ciphertextExplicit, o =>
            {
                o.Mode = CipherMode.CBC;
                o.Padding = PaddingMode.PKCS7;
            });

            Console.WriteLine("Explicit options roundtrip: {0}",
                Encoding.UTF8.GetString(decryptedExplicit) == roundtrip); // true
        }

        public static void GenerateKeysWithCustomSize()
        {
            // Generate a 128-bit key explicitly.
            byte[] key128 = AesCryptor.GenerateKey(o => o.Size = AesSize.Aes128);
            Console.WriteLine("128-bit key length: {0} bytes", key128.Length); // 16

            // Generate a 192-bit key.
            byte[] key192 = AesCryptor.GenerateKey(o => o.Size = AesSize.Aes192);
            Console.WriteLine("192-bit key length: {0} bytes", key192.Length); // 24

            // Default is 256-bit.
            byte[] key256 = AesCryptor.GenerateKey();
            Console.WriteLine("256-bit key length: {0} bytes", key256.Length); // 32
        }

        public static void UseDefaultConstructor()
        {
            // Default constructor generates a random key and IV.
            var cryptor = new AesCryptor();

            byte[] data = Encoding.UTF8.GetBytes("Hello, world!");
            byte[] encrypted = cryptor.Encrypt(data);
            byte[] decrypted = cryptor.Decrypt(encrypted);

            Console.WriteLine("Default constructor roundtrip: {0}",
                Encoding.UTF8.GetString(decrypted)); // Hello, world!
        }
    }
}

```
