---
uid: Cuemon.Threading.AsyncOptions
example:
- *content
---

The following example demonstrates how to configure AsyncOptions to provide a cancellation token for asynchronous operations.

```csharp
// Program.cs
using System;
using System.Threading;
using System.Threading.Tasks;
using Cuemon.Threading;

var defaults = new AsyncOptions();
Console.WriteLine(defaults.CancellationToken == CancellationToken.None);

using var cts = new CancellationTokenSource();
var options = new AsyncOptions
{
    CancellationTokenProvider = () => cts.Token
};

cts.Cancel();

try
{
    await Task.Delay(10, options.CancellationToken);
}
catch (OperationCanceledException)
{
    Console.WriteLine("Cancelled as expected.");
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="2490d13995181c455228acbf"><a href="~/run-actions/2490d13995181c455228acbf/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/2490d13995181c455228acbf/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
