---
uid: Cuemon.Eradicate
example:
- *content
---

The following example demonstrates how to use `Eradicate` to clean up byte arrays by removing trailing zero bytes or specific trailing byte sequences.

```csharp
// Program.cs
using System;
using System.Text;
using Cuemon;

// Remove trailing zero bytes from a byte array
byte[] dataWithZeros = { 1, 2, 3, 0, 0, 0 };
byte[] cleaned = Eradicate.TrailingZeros(dataWithZeros);
Console.WriteLine(BitConverter.ToString(cleaned)); // 01-02-03

// Remove specific trailing byte sequence (e.g., CR/LF)
byte[] dataWithCrLf = { 72, 101, 108, 108, 111, 13, 10, 13, 10 };
byte[] stripped = Eradicate.TrailingBytes(dataWithCrLf, new byte[] { 13, 10 });
Console.WriteLine(Encoding.UTF8.GetString(stripped)); // Hello
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="a111f64116f5474aeb6db136"><a href="~/run-actions/a111f64116f5474aeb6db136/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/a111f64116f5474aeb6db136/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
