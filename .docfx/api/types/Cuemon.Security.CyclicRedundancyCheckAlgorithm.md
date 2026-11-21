---
uid: Cuemon.Security.CyclicRedundancyCheckAlgorithm
example:
- *content
---

The following example demonstrates how to use the <see cref="CyclicRedundancyCheckAlgorithm"/> enum to select a specific CRC algorithm variant.

```csharp
// Program.cs
using System;
using Cuemon.Security; // for CyclicRedundancyCheckAlgorithm

// Select commonly used CRC algorithms
CyclicRedundancyCheckAlgorithm crc32 = CyclicRedundancyCheckAlgorithm.Crc32;
CyclicRedundancyCheckAlgorithm crc32C = CyclicRedundancyCheckAlgorithm.Crc32C;
CyclicRedundancyCheckAlgorithm crc64 = CyclicRedundancyCheckAlgorithm.Crc64;

Console.WriteLine(crc32);  // Crc32
Console.WriteLine(crc32C); // Crc32C
Console.WriteLine(crc64);  // Crc64

// Switch on algorithm
string GetDescription(CyclicRedundancyCheckAlgorithm algo) => algo switch
{
    CyclicRedundancyCheckAlgorithm.Crc32 => "CRC-32 (ISO-HDLC, PKZIP)",
    CyclicRedundancyCheckAlgorithm.Crc32C => "CRC-32C (ISCSI, Castagnoli)",
    CyclicRedundancyCheckAlgorithm.Crc64 => "CRC-64 (ECMA-182)",
    _ => "Unknown"
};

Console.WriteLine(GetDescription(crc32C)); // CRC-32C (ISCSI, Castagnoli)
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="c20a953ef91a8cbfe7ebf47e"><a href="~/run-actions/c20a953ef91a8cbfe7ebf47e/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/c20a953ef91a8cbfe7ebf47e/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
