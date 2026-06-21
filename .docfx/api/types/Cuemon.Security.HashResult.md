---
uid: Cuemon.Security.HashResult
example:
- *content
---

The following example demonstrates how to create a `HashResult` from a computed hash value and convert it to different string representations.

```csharp
using System;
using Cuemon.Security;

namespace MyApp.Examples;

public class HashResultExample
{
    public static void Main()
    {
        // Simulate a computed hash value (e.g., from any hash algorithm).
        byte[] hashBytes = { 0xdf, 0xfd, 0x60, 0x21, 0xbb, 0x2b, 0xd5, 0xb0, 0xaf, 0x67, 0x62, 0x90, 0x80, 0x9e, 0xc3, 0xa5, 0x31, 0x91, 0xdd, 0x81, 0xc7, 0xf7, 0x0a, 0x4b, 0x28, 0x68, 0x8a, 0x36, 0x21, 0x82, 0x98, 0x6f };

        var hashResult = new HashResult(hashBytes);

        // Check if the hash has a value
        Console.WriteLine("HasValue: {0}", hashResult.HasValue);

        // Convert to various string formats
        Console.WriteLine("Hex: {0}", hashResult.ToHexadecimalString());
        Console.WriteLine("Base64: {0}", hashResult.ToBase64String());
        Console.WriteLine("Url-safe Base64: {0}", hashResult.ToUrlEncodedBase64String());

        // Default ToString() returns hexadecimal
        Console.WriteLine("ToString: {0}", hashResult.ToString());

        // Output:
        // HasValue: True
        // Hex: dffd6021bb2bd5b0af676290809ec3a53191dd81c7f70a4b28688a362182986f
        // Base64: 3/1gIbsr1bCvZ2KQgJ7DpTGR3YHH9wpLKGiKNiGCmG8=
        // Url-safe Base64: 3_1gIbsr1bCvZ2KQgJ7DpTGR3YHH9wpLKGiKNiGCmG8=
        // ToString: dffd6021bb2bd5b0af676290809ec3a53191dd81c7f70a4b28688a362182986f

}
}

```
