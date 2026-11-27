---
uid: Cuemon.Security.Cryptography.AesSize
example:
- *content
---

The following example demonstrates how to select AES key sizes using the AesSize enumeration when generating cryptographic keys.

```csharp
// Program.cs
using System;
using Cuemon.Security.Cryptography;

// AesSize specifies the key size for the AES symmetric algorithm:
//   Aes128 = 128-bit key (16 bytes)
//   Aes192 = 192-bit key (24 bytes)
//   Aes256 = 256-bit key (32 bytes, default)

// Generate a 256-bit key (default when no options are specified)
byte[] defaultKey = AesCryptor.GenerateKey();
Console.WriteLine($"Default key length: {defaultKey.Length} bytes"); // 32

// Generate a 128-bit key using AesSize.Aes128
byte[] key128 = AesCryptor.GenerateKey(o => o.Size = AesSize.Aes128);
Console.WriteLine($"128-bit key length: {key128.Length} bytes"); // 16

// Generate a 192-bit key using AesSize.Aes192
byte[] key192 = AesCryptor.GenerateKey(o => o.Size = AesSize.Aes192);
Console.WriteLine($"192-bit key length: {key192.Length} bytes"); // 24

// Generate a 256-bit key explicitly using AesSize.Aes256
byte[] key256 = AesCryptor.GenerateKey(o => o.Size = AesSize.Aes256);
Console.WriteLine($"256-bit key length: {key256.Length} bytes"); // 32

// Compare enum values directly
AesSize size = AesSize.Aes256;
Console.WriteLine($"Selected size: {size}"); // Aes256
Console.WriteLine($"Is Aes256 selected: {size == AesSize.Aes256}"); // True
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="3febff6ede30c55f989abf1d"><a href="~/run-actions/3febff6ede30c55f989abf1d/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/3febff6ede30c55f989abf1d/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
