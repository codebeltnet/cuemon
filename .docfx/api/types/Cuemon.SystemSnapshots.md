---
uid: Cuemon.SystemSnapshots
example:
- *content
---

The following example demonstrates how to use the `SystemSnapshots` flags enum to specify which system information categories to capture in a diagnostics snapshot.

```csharp
using System;
using Cuemon;

namespace MyApp.Examples
{
    public class SystemSnapshotsExample
    {
        public void Demonstrate()
        {
            // SystemSnapshots is a flags enum controlling which system
            // information categories to capture.

            // Capture everything available.
            var all = SystemSnapshots.CaptureAll;
            Console.WriteLine($"CaptureAll = {all}");   // CaptureThreadInfo | CaptureProcessInfo | CaptureEnvironmentInfo

            // Capture only specific categories.
            var minimal = SystemSnapshots.CaptureThreadInfo | SystemSnapshots.CaptureProcessInfo;

            // Test if a flag is set.
            if (all.HasFlag(SystemSnapshots.CaptureEnvironmentInfo))
            {
                Console.WriteLine("Environment info will be captured.");

            // Start with none and add incrementally.
            var flags = SystemSnapshots.None;
            flags |= SystemSnapshots.CaptureThreadInfo;
            flags |= SystemSnapshots.CaptureProcessInfo;

            Console.WriteLine($"Flags: {flags}");                           // CaptureThreadInfo | CaptureProcessInfo
            Console.WriteLine($"Has thread info: {flags.HasFlag(SystemSnapshots.CaptureThreadInfo)}");   // True

}}}
}

```
