---
uid: Cuemon.TesterFunc`2
example:
- *content
---

The following example demonstrates the shared workflow for the <see cref="TesterFunc{TResult, TSuccess}" /> family: call the delegate, capture the out value, and branch on the success result.

```csharp
// Program.cs
using System;
using Cuemon;

TesterFunc<int, bool> tryReadPort = (out int port) =>
{
    var configuredPort = "8080";
    return int.TryParse(configuredPort, out port);
};

var success = tryReadPort(out var port);

Console.WriteLine(success);
Console.WriteLine(port);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="9ffd8e34ee97a0c92b04a947"><a href="~/run-actions/9ffd8e34ee97a0c92b04a947/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/9ffd8e34ee97a0c92b04a947/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
