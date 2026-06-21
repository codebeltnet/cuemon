---
uid: Cuemon.Extensions.ByteExtensions
example:
- *content
---

The following example demonstrates how to use the <xref:Cuemon.Extensions.ByteExtensions> extension methods to convert byte arrays to various string representations and detect Unicode encoding.

```csharp
using System;
using System.Text;
using Cuemon.Extensions;

namespace MyApp.Examples;

public static class ByteExtensionsExample
{
    public static void Demonstrate()
    {
        byte[] data = { 0xEF, 0xBB, 0xBF, 0x48, 0x65, 0x6C, 0x6C, 0x6F };
        string text = data.ToEncodedString();
        string hex = data.ToHexadecimalString();
        string binary = data.ToBinaryString();
        string base64 = data.ToBase64String();
        string urlBase64 = data.ToUrlEncodedBase64String();

        data.TryDetectUnicodeEncoding(out Encoding detectedEncoding);

        Console.WriteLine(text);
        Console.WriteLine(hex);
        Console.WriteLine(binary);
        Console.WriteLine(base64);
        Console.WriteLine(urlBase64);
        Console.WriteLine(detectedEncoding?.WebName ?? "unknown");
    }
}

```
