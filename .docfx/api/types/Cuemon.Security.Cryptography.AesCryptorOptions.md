---
uid: Cuemon.Security.Cryptography.AesCryptorOptions
example:
- *content
---

The following example demonstrates how to configure AesCryptorOptions to specify cipher mode and padding when performing AES encryption and decryption.

```csharp
using System;
using System.Security.Cryptography;
using System.Text;
using Cuemon.Security.Cryptography;

namespace MyApp.Security;

public class AesCryptorOptionsExample
{
    public void Demonstrate()
    {
        // Generate a key and initialization vector
        byte[] key = AesCryptor.GenerateKey();
        byte[] iv = AesCryptor.GenerateInitializationVector();

        // Create an AES cryptor with the key and IV
        var cryptor = new AesCryptor(key, iv);

        // Encrypt data using default options (CBC mode, PKCS7 padding)
        byte[] plaintext = Encoding.UTF8.GetBytes("Secret message.");
        byte[] encrypted = cryptor.Encrypt(plaintext);
        Console.WriteLine($"Encrypted ({encrypted.Length} bytes): {Convert.ToBase64String(encrypted)}");

        // Decrypt with default options
        byte[] decrypted = cryptor.Decrypt(encrypted);
        Console.WriteLine($"Decrypted: {Encoding.UTF8.GetString(decrypted)}"); // Secret message.

        // Create options explicitly and copy settings via delegate
        var defaultOptions = new AesCryptorOptions();
        defaultOptions.Mode = CipherMode.CBC;
        defaultOptions.Padding = PaddingMode.PKCS7;

        // Encrypt with explicit cipher mode and padding via delegate from AesCryptorOptions
        byte[] plaintext2 = Encoding.UTF8.GetBytes("Configured encryption.");
        byte[] encrypted2 = cryptor.Encrypt(plaintext2, o =>
        {
            o.Mode = defaultOptions.Mode;
            o.Padding = defaultOptions.Padding;
        });

        // Decrypt using the same options
        byte[] decrypted2 = cryptor.Decrypt(encrypted2, o =>
        {
            o.Mode = defaultOptions.Mode;
            o.Padding = defaultOptions.Padding;
        });
        Console.WriteLine($"Decrypted: {Encoding.UTF8.GetString(decrypted2)}"); // Configured encryption.

        // Using ECB mode with Zeros padding (not recommended for production)
        defaultOptions.Mode = CipherMode.ECB;
        defaultOptions.Padding = PaddingMode.Zeros;
        byte[] plaintext3 = Encoding.UTF8.GetBytes("Alternative mode.");
        byte[] encrypted3 = cryptor.Encrypt(plaintext3, o =>
        {
            o.Mode = defaultOptions.Mode;
            o.Padding = defaultOptions.Padding;
        });
        byte[] decrypted3 = cryptor.Decrypt(encrypted3, o =>
        {
            o.Mode = defaultOptions.Mode;
            o.Padding = defaultOptions.Padding;
        });
        Console.WriteLine($"Decrypted: {Encoding.UTF8.GetString(decrypted3)}"); // Alternative mode.

}
}

```
