---
uid: Cuemon.Net.Http.HttpManagerOptions
example:
- *content
---

The following example demonstrates how to configure <see cref="HttpManagerOptions" /> before passing them to <see cref="HttpManager" />.

```csharp
// Program.cs
using System;
using System.Net.Http;
using Cuemon.Net.Http;

var options = new HttpManagerOptions
{
    Timeout = TimeSpan.FromSeconds(10),
    HandlerFactory = () => new HttpClientHandler(),
    DefaultRequestHeaders =
    {
        ["X-Correlation-Id"] = "docs-123"
    }
};

options.ValidateOptions();

Console.WriteLine(options.Timeout.TotalSeconds);
Console.WriteLine(options.DefaultRequestHeaders["X-Correlation-Id"]);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="b7d831942ac1e33a1435d49f"><a href="~/run-actions/b7d831942ac1e33a1435d49f/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/b7d831942ac1e33a1435d49f/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
