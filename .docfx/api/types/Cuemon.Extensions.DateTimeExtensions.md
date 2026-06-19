---
uid: Cuemon.Extensions.DateTimeExtensions
example:
- *content
---

The following example demonstrates common `DateTime` extension methods for rounding, range checking, time-of-day classification, and timezone kind conversion.

```csharp
using System;
using Cuemon;
using Cuemon.Extensions;

namespace MyApp.Examples;

public static class DateTimeExtensionsExample
{
    public static void Demonstrate()
    {
        var utc = new DateTime(2025, 6, 16, 14, 33, 22, DateTimeKind.Utc);
        var local = new DateTime(2025, 6, 16, 14, 33, 22, DateTimeKind.Local);

        var floorHour = utc.Floor(TimeSpan.FromHours(1));
        var ceilingHour = utc.Ceiling(TimeSpan.FromHours(1));
        var floorFifteenMinutes = utc.Floor(15, TimeUnit.Minutes);
        var roundedUp = utc.Round(TimeSpan.FromMinutes(30), VerticalDirection.Up);
        var isWithinRange = utc.IsWithinRange(new DateTimeRange(utc.AddHours(-1), utc.AddHours(1)));

        var isNight = new DateTime(2025, 6, 16, 22, 15, 0, DateTimeKind.Utc).IsTimeOfDayNight();
        var isMorning = new DateTime(2025, 6, 16, 6, 15, 0, DateTimeKind.Utc).IsTimeOfDayMorning();
        var isForenoon = new DateTime(2025, 6, 16, 10, 15, 0, DateTimeKind.Utc).IsTimeOfDayForenoon();
        var isAfternoon = utc.IsTimeOfDayAfternoon();
        var isEvening = new DateTime(2025, 6, 16, 19, 15, 0, DateTimeKind.Utc).IsTimeOfDayEvening();

        var utcKind = local.ToUtcKind();
        var localKind = utc.ToLocalKind();
        var unspecifiedKind = utc.ToDefaultKind();

        var unixTime = utc.ToUnixEpochTime();
        var restored = unixTime.FromUnixEpochTime();

        Console.WriteLine(floorHour.ToString("O"));
        Console.WriteLine(ceilingHour.ToString("O"));
        Console.WriteLine(floorFifteenMinutes.ToString("O"));
        Console.WriteLine(roundedUp.ToString("O"));
        Console.WriteLine(isWithinRange);
        Console.WriteLine($"{isNight}, {isMorning}, {isForenoon}, {isAfternoon}, {isEvening}");
        Console.WriteLine($"{utcKind.Kind}, {localKind.Kind}, {unspecifiedKind.Kind}");
        Console.WriteLine(restored.ToString("O"));
    }
}

```
