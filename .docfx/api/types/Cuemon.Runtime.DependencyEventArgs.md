---
uid: Cuemon.Runtime.DependencyEventArgs
example:
- *content
---

The following example demonstrates how to create and use DependencyEventArgs to report the UTC timestamp of a dependency change event.

```csharp
using System;
using Cuemon.Runtime;

namespace MyApp.Runtime
{
    public class DependencyEventArgsExamples
    {
        public static void CreateEventArgs()
        {
            // Create event args with the UTC time of the last dependency change.
            var args = new DependencyEventArgs(DateTime.UtcNow);
            Console.WriteLine("Last modified (UTC): {0:O}", args.UtcLastModified);
        }

        public static void CreateEventArgsWithPastTimestamp()
        {
            // Create event args with a specific past timestamp.
            var lastWrite = new DateTime(2026, 6, 1, 12, 0, 0, DateTimeKind.Utc);
            var args = new DependencyEventArgs(lastWrite);
            Console.WriteLine("Last dependency change: {0:O}", args.UtcLastModified);
        }

        public static void UseEmptySentinel()
        {
            // Use the static Empty sentinel to represent no change.
            DependencyEventArgs args = DependencyEventArgs.Empty;
            Console.WriteLine("Is empty: {0}", args.UtcLastModified == DateTime.MinValue); // true
        }

        public static void RaiseDependencyEvent()
        {
            var monitor = new DependencyMonitor();
            monitor.DependencyChanged += (sender, args) =>
            {
                Console.WriteLine("Dependency last changed at: {0:O}", args.UtcLastModified);
            };

            monitor.CheckForChanges();
        }
    }

    public class DependencyMonitor
    {
        public event EventHandler<DependencyEventArgs> DependencyChanged;

        public void CheckForChanges()
        {
            DependencyChanged?.Invoke(this, new DependencyEventArgs(DateTime.UtcNow));
        }
    }
}
```