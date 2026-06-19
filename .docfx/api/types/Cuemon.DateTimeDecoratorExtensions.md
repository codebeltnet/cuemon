---
uid: Cuemon.DateTimeDecoratorExtensions
example:
- *content
---

The following example shows how to extend `DateTime` with `DateTimeDecoratorExtensions` methods to perform Unix epoch conversions and adjust `DateTimeKind` without changing the underlying ticks.

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
