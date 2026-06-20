---
uid: Cuemon.DateTimeDecoratorExtensions
example:
- *content
---

`DateTimeDecoratorExtensions` provides extension methods for converting between `DateTime` and Unix epoch time and for switching `DateTimeKind` without altering the underlying ticks. This example retrieves the Unix epoch via `Decorator.Syntactic<DateTime>().GetUnixEpoch()`, converts `DateTime.UtcNow` to seconds since epoch with `ToUnixEpochTime`, and transforms `DateTimeKind` between `Utc`, `Local`, and `Unspecified` using `ToUtcKind`, `ToLocalKind`, and `ToDefaultKind`. Key setup includes creating UTC and local `DateTime` values, then verifying each kind-only transformation preserves the tick count. Console output shows the epoch value, Unix timestamp, and confirms `Kind` changes while `Ticks == localTime.Ticks` remains `True`.

```csharp
using System;
using Cuemon;

namespace MyApp.DateTimeExamples
{
    public class DateTimeDecoratorExtensionsExample
    {
        public void Demonstrate()
        {
            // Get the Unix epoch (January 1st, 1970 UTC)
            var unixEpoch = Decorator.Syntactic<DateTime>().GetUnixEpoch();
            Console.WriteLine(unixEpoch); // 1/1/1970 12:00:00 AM

            // Convert a DateTime to Unix epoch time (seconds since 1970-01-01 UTC)
            var utcNow = DateTime.UtcNow;
            var unixTime = Decorator.Enclose(utcNow).ToUnixEpochTime();
            Console.WriteLine(unixTime); // e.g., 1700000000

            // Convert from local DateTime to UTC-kind DateTime (same ticks, different kind)
            var localTime = new DateTime(2024, 6, 15, 12, 0, 0, DateTimeKind.Local);
            var utcKind = Decorator.Enclose(localTime).ToUtcKind();
            Console.WriteLine(utcKind.Kind); // Utc
            Console.WriteLine(utcKind.Ticks == localTime.Ticks); // True

            // Convert from UTC DateTime to local-kind DateTime
            var utcTime = new DateTime(2024, 6, 15, 10, 0, 0, DateTimeKind.Utc);
            var localKind = Decorator.Enclose(utcTime).ToLocalKind();
            Console.WriteLine(localKind.Kind); // Local
            Console.WriteLine(localKind.Ticks == utcTime.Ticks); // True

            // Strip kind information (set to Unspecified)
            var defaultKind = Decorator.Enclose(utcTime).ToDefaultKind();
            Console.WriteLine(defaultKind.Kind); // Unspecified
            Console.WriteLine(defaultKind.Ticks == utcTime.Ticks); // True

}}
}

```
