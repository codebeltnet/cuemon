---
uid: Cuemon.Extensions.RoundOffAccuracy
example:
- *content
---

The following example demonstrates how to use the <see cref="Cuemon.Extensions.RoundOffAccuracy"/> enum with the <see cref="M:Cuemon.Extensions.DoubleExtensions.RoundOff(System.Double,Cuemon.Extensions.RoundOffAccuracy)"/> extension method to round double values to the nearest specified accuracy.

```csharp
using System;
using Cuemon.Extensions;

namespace MyApp.Examples;

public static class RoundOffAccuracyExample
{
    public static void Demonstrate()
    {
        double value = 123456789.987654321d;

        Console.WriteLine(value.RoundOff(RoundOffAccuracy.NearestTenth));
        Console.WriteLine(value.RoundOff(RoundOffAccuracy.NearestHundredth));
        Console.WriteLine(value.RoundOff(RoundOffAccuracy.NearestThousandth));
        Console.WriteLine(value.RoundOff(RoundOffAccuracy.NearestMillion));
    }
}

```
