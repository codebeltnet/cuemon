---
uid: Cuemon.Security.CyclicRedundancyCheck64
example:
- *content
---

The following example demonstrates how to compute a 64-bit Cyclic Redundancy Check (CRC) checksum using the <xref:Cuemon.Security.CyclicRedundancyCheck64> class.

```csharp
using System;
using System.Text;
using Cuemon.Security;

namespace MyApp.Examples;

public class CyclicRedundancyCheck64Example
{
    public void Demonstrate()
    {
        // Create CRC-64 instance with default ECMA-182 polynomial
        var crc64 = new CyclicRedundancyCheck64();

        // Compute checksum from ASCII input
        byte[] data = Encoding.ASCII.GetBytes("123456789");
        HashResult result = crc64.ComputeHash(data);

        // Display the checksum in hexadecimal
        string hex = result.ToHexadecimalString();
        Console.WriteLine(hex); // 6c40df5f0b497347

        // Create CRC-64 with custom polynomial
        var customCrc = new CyclicRedundancyCheck64(
            polynomial: 0x42F0E1EBA9EA3693,
            initialValue: 0xFFFFFFFFFFFFFFFF,
            finalXor: 0xFFFFFFFFFFFFFFFF);

        HashResult customResult = customCrc.ComputeHash(data);
        Console.WriteLine(customResult.ToHexadecimalString());

}
}

```
