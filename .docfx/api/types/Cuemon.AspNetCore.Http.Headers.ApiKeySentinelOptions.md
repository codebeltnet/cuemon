---
uid: Cuemon.AspNetCore.Http.Headers.ApiKeySentinelOptions
example:
- *content
---

The following example demonstrates configuring API key sentinel options.

```csharp
// Program.cs
using System;
using System.Collections.Generic;
using Cuemon.AspNetCore.Http.Headers;

        var options = new ApiKeySentinelOptions
{
    AllowedKeys = new List<string> { "secret-key" }
};

options.ValidateOptions();
using var response = options.ResponseHandler("wrong-key");
var message = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
Console.WriteLine(message);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="ef14a96086c0b672f21e011f"><a href="~/run-actions/ef14a96086c0b672f21e011f/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/ef14a96086c0b672f21e011f/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
