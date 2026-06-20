---
uid: Cuemon.Extensions.DoubleExtensions
example:
- *content
---

`DoubleExtensions` provides extension methods for `Double` covering Unix epoch conversion, time span creation, factorial computation, and precision rounding. This example starts with a Unix timestamp of `1617738277` and converts it to a local `DateTime` via `FromUnixEpochTime`, creates a `TimeSpan` from `3661` seconds using `ToTimeSpan`, computes `5!` using `Factorial`, and rounds `123456789.987654321` to the nearest thousand and million using `RoundOff`. Console output shows the resulting date (`O` format), the duration (`01:01:01`), the factorial value (`120`), and the rounded figures (`123457000` and `123000000`).

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
