---
uid: Cuemon.DoubleDecoratorExtensions
example:
- *content
---

`DoubleDecoratorExtensions` provides extension methods on `Decorator.Enclose` for converting `double` values into `TimeSpan` instances using the `ToTimeSpan` method with a specified `TimeUnit`. This example creates four `double` values representing 1.5 days, 90 minutes, 5000 milliseconds, and 2.5 hours, each wrapped with `Decorator.Enclose` and passed to `ToTimeSpan` with the matching time unit. Key setup includes choosing the correct `TimeUnit` enum value for each conversion. Console output shows `1.12:00:00` for 1.5 days, `01:30:00` for 90 minutes, `00:00:05` for 5000 milliseconds, and `02:30:00` for 2.5 hours.

```csharp
using System;
using Cuemon;

namespace MyApp.Numeric
{
    public class DoubleDecoratorExtensionsExample
    {
        public void Demonstrate()
        {
            // Convert 1.5 days to a TimeSpan
            double days = 1.5;
            TimeSpan duration = Decorator.Enclose(days).ToTimeSpan(TimeUnit.Days);
            Console.WriteLine(duration); // Output: 1.12:00:00

            // Convert 90 minutes to a TimeSpan
            double minutes = 90;
            TimeSpan meeting = Decorator.Enclose(minutes).ToTimeSpan(TimeUnit.Minutes);
            Console.WriteLine(meeting); // Output: 01:30:00

            // Convert 5000 milliseconds to a TimeSpan
            double ms = 5000;
            TimeSpan interval = Decorator.Enclose(ms).ToTimeSpan(TimeUnit.Milliseconds);
            Console.WriteLine(interval); // Output: 00:00:05

            // Convert 2.5 hours to a TimeSpan
            double hours = 2.5;
            TimeSpan task = Decorator.Enclose(hours).ToTimeSpan(TimeUnit.Hours);
            Console.WriteLine(task); // Output: 02:30:00

}}
}

```
