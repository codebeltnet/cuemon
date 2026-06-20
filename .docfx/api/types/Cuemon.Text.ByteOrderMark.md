---
uid: Cuemon.Text.ByteOrderMark
example:
- *content
---

The following example demonstrates how to detect and remove byte order marks (BOM) from a UTF-8 byte array and detect encoding from a stream. The output shows the detected encoding name, the byte count after BOM removal, and the encoding detected from the stream.

```csharp
using System;
using System.IO;
using System.Text;
using Cuemon.Text;

namespace Cuemon.Text;

public class ByteOrderMarkExample
{
    public void Demonstrate()
    {
        var utf8Bytes = new byte[] { 0xEF, 0xBB, 0xBF, 0x48, 0x65, 0x6C, 0x6C, 0x6F };
        var encoding = ByteOrderMark.Decode(utf8Bytes);
        Console.WriteLine($"Detected encoding: {encoding.EncodingName}");

        var bytes = ByteOrderMark.Remove(utf8Bytes, Encoding.UTF8);
        Console.WriteLine($"BOM removed, remaining length: {bytes.Length}");

        using var stream = new MemoryStream(utf8Bytes);
        if (ByteOrderMark.TryDetectEncoding(stream, out var detected))
        {
            Console.WriteLine($"Stream encoding: {detected.EncodingName}");
        }
    }
}
```
