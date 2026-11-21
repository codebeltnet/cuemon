---
uid: Cuemon.Calculator
example:
- *content
---

The following example demonstrates how to use the `Calculator` class to perform basic arithmetic and bitwise operations. Each method call prints the computed result to the console.

```csharp
// Program.cs
using System;
using Cuemon;

int sum = Calculator.Add(10, 20);
Console.WriteLine(sum); // 30

int difference = Calculator.Subtract(20, 5);
Console.WriteLine(difference); // 15

int product = Calculator.Multiply(4, 5);
Console.WriteLine(product); // 20

int quotient = Calculator.Divide(20, 4);
Console.WriteLine(quotient); // 5

int remainder = Calculator.Remainder(10, 3);
Console.WriteLine(remainder); // 1

int bitwiseAnd = Calculator.And(0b1100, 0b1010);
Console.WriteLine(bitwiseAnd); // 8 (0b1000)
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="514de72b008a7b03fd3c502f"><a href="~/run-actions/514de72b008a7b03fd3c502f/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/514de72b008a7b03fd3c502f/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
