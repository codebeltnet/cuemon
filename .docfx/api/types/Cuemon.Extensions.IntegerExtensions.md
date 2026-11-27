---
uid: Cuemon.Extensions.IntegerExtensions
example:
- *content
---

The following example demonstrates how to use the <xref:Cuemon.Extensions.IntegerExtensions> extension methods to check whether a given integer is prime, even, or odd, and to find the smallest or largest of two values.

```csharp
// Program.cs
using System;
using Cuemon.Extensions;

var number = 17;
bool isPrime = number.IsPrime();
bool isEven = number.IsEven();
bool isOdd = number.IsOdd();

Console.WriteLine(isPrime);
Console.WriteLine(isEven);
Console.WriteLine(isOdd);
Console.WriteLine(5.Max(10));
Console.WriteLine(15.Min(10));
Console.WriteLine(500L.Min(1000L));
Console.WriteLine(((short)3).Max((short)7));
Console.WriteLine(new[] { 1, 3, 5, 7 }.IsCountableSequence());
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="a616a6b827e8278b9dc6850e"><a href="~/run-actions/a616a6b827e8278b9dc6850e/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/a616a6b827e8278b9dc6850e/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
