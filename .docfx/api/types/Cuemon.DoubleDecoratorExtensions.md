---
uid: Cuemon.DoubleDecoratorExtensions
example:
- *content
---

The following example shows how to extend `double` with `DoubleDecoratorExtensions` methods to convert numeric values into `TimeSpan` instances using a specified `TimeUnit`.

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
