---
uid: Cuemon.Net.Http.HttpWatcher
example:
- *content
---

The following example demonstrates how to configure an <see cref="HttpWatcher" /> for a remote URI and inspect its active monitoring settings.

```csharp
// Program.cs
using System;
using Cuemon.Net.Http;

var watcher = new HttpWatcher(new Uri("https://example.com/feed"), options =>
{
    options.ReadResponseBody = true;
    options.Period = TimeSpan.FromSeconds(10);
});

Console.WriteLine(watcher.Location);
Console.WriteLine(watcher.ReadResponseBody);
Console.WriteLine(watcher.HashFactory != null);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="ded1637126f62de68fe51c12"><a href="~/run-actions/ded1637126f62de68fe51c12/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/ded1637126f62de68fe51c12/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
