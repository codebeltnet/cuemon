---
uid: Cuemon.Extensions.DoubleExtensions
example:
- *content
---

The following example demonstrates how to use DoubleExtensions for numeric operations such as Unix epoch conversion, time span creation, factorial computation, and rounding to specified accuracy.

```csharp
using System;
using Cuemon;
using Cuemon.Extensions;

namespace MyApp.Numerics;

public static class DoubleExtensionsExample
{
    public static void Demonstrate()
    {
        double unixTimestamp = 1617738277d;
        DateTime fromUnix = unixTimestamp.FromUnixEpochTime().ToLocalTime();

        TimeSpan fromSeconds = 3661d.ToTimeSpan(TimeUnit.Seconds);
        double factorial = 5d.Factorial();

        double value = 123456789.987654321d;
        double nearestThousand = value.RoundOff(RoundOffAccuracy.NearestThousandth);
        double nearestMillion = value.RoundOff(RoundOffAccuracy.NearestMillion);

        Console.WriteLine(fromUnix.ToString("O"));
        Console.WriteLine(fromSeconds);
        Console.WriteLine(factorial);
        Console.WriteLine(nearestThousand);
        Console.WriteLine(nearestMillion);
    }
}

```
