---
uid: Cuemon.AspNetCore.Http.Headers.UserAgentSentinelOptions
example:
- *content
---

The following example demonstrates how to configure <xref cref="Cuemon.AspNetCore.Http.Headers.UserAgentSentinelOptions"/> to restrict which User-Agent headers are allowed.

```csharp
// Program.cs
using System;
using System.Collections.Generic;
using Cuemon.AspNetCore.Http.Headers;

        var options = new UserAgentSentinelOptions
{
    RequireUserAgentHeader = true,
    ValidateUserAgentHeader = true,
    AllowedUserAgents = new List<string> { "Cuemon-Agent" }
};

options.ValidateOptions();
using var response = options.ResponseHandler("Unknown-Agent");
var message = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
Console.WriteLine(message);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="533bfcfd8a930e86659d8797"><a href="~/run-actions/533bfcfd8a930e86659d8797/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/533bfcfd8a930e86659d8797/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
