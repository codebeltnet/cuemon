---
uid: Cuemon.IO.AsyncDisposableOptions
example:
- *content
---

The following example demonstrates how to configure `AsyncDisposableOptions` to control whether a disposable resource is left open after an async operation.

```csharp
using System;
using System.IO;
using System.Threading.Tasks;
using Cuemon.IO;

namespace MyApp.Examples;

public class AsyncDisposableOptionsExample
{
    public static async Task Main()
    {
        var options = new AsyncDisposableOptions
        {
            LeaveOpen = true
        };

        var stream = new MemoryStream();
        // Use options.LeaveOpen to decide disposal behavior
        if (!options.LeaveOpen)
        {
            stream.Dispose();
        }

        Console.WriteLine("LeaveOpen: {0}", options.LeaveOpen);
        Console.WriteLine("Stream still open: {0}", stream.CanWrite);

        // Output:
        // LeaveOpen: True
        // Stream still open: True
    }
}

```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="f6fde2bb005cea9e7e460361"><a href="~/run-actions/f6fde2bb005cea9e7e460361/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/f6fde2bb005cea9e7e460361/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
