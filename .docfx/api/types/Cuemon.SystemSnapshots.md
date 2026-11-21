---
uid: Cuemon.SystemSnapshots
example:
- *content
---

The following example demonstrates how to use the `SystemSnapshots` flags enum to specify which system information categories to capture in a diagnostics snapshot.

```csharp
// Program.cs
using System;
using Cuemon;

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

}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="0a3f204ebac75cfb9b9d91e1"><a href="~/run-actions/0a3f204ebac75cfb9b9d91e1/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/0a3f204ebac75cfb9b9d91e1/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
