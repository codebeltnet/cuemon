---
uid: Cuemon.DisposableOptions
example:
- *content
---

The following example demonstrates how to use <see cref="Cuemon.DisposableOptions"/> to control whether a disposable resource is released when the owning wrapper is disposed.

```csharp
// Program.cs
using System;
using Cuemon;
using Contoso.IO;

DisposableOptionsExample.Run();

namespace Contoso.IO
{
    public sealed class DisposableOptionsExample
    {
        public static void Run()
        {
            var resource = new TrackedDisposable();
            var options = new DisposableOptions
            {
                LeaveOpen = true
            };

            DisposeWhenAllowed(resource, options);
            Console.WriteLine($"Disposed after leave-open: {resource.IsDisposed}");

            options.LeaveOpen = false;
            DisposeWhenAllowed(resource, options);
            Console.WriteLine($"Disposed after close: {resource.IsDisposed}");
        }

        private static void DisposeWhenAllowed(TrackedDisposable resource, DisposableOptions options)
        {
            if (!options.LeaveOpen)
            {
                resource.Dispose();
            }
        }

        private sealed class TrackedDisposable : IDisposable
        {
            public bool IsDisposed { get; private set; }

            public void Dispose()
            {
                IsDisposed = true;
            }
        }
    }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="971dec3e05c07ac80a9873c1"><a href="~/run-actions/971dec3e05c07ac80a9873c1/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/971dec3e05c07ac80a9873c1/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
