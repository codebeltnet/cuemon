---
uid: Cuemon.Text.AsyncEncodingOptions
example:
- *content
---

The following example demonstrates how to configure `AsyncEncodingOptions` with a cancellation token for use in asynchronous encoding operations.

```csharp
// Program.cs
using System;
using System.Threading.Tasks;
using System.Text;
using System.Threading;
using Cuemon.Text;

using var cts = new CancellationTokenSource();
var options = new AsyncEncodingOptions
{
    Encoding = Encoding.UTF8,
    Preamble = PreambleSequence.Remove,
    CancellationToken = cts.Token
};

// The options can be passed to async encoding methods
// that accept AsyncEncodingOptions. If cancellation is
// requested, the operation will be cancelled gracefully.
Console.WriteLine(options.Encoding.WebName);
Console.WriteLine(options.Preamble);
Console.WriteLine(options.CancellationToken.CanBeCanceled);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="7dac037ec19dadac590338ee"><a href="~/run-actions/7dac037ec19dadac590338ee/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/7dac037ec19dadac590338ee/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
