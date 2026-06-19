---
uid: Cuemon.Calculator
example:
- *content
---

```csharp
using System;
using Cuemon;

namespace MyApp.Arithmetic;

public class CalculatorExample
{
    public void Demonstrate()
    {
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
    }
}
```
