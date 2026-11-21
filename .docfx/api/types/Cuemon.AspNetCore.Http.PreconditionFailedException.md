---
uid: Cuemon.AspNetCore.Http.PreconditionFailedException
example:
- *content
---

The following example demonstrates how to use `PreconditionFailedException` when a conditional request header check fails.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Http;

const string currentEtag = "\"v2\"";
const string suppliedEtag = "\"v1\"";
if (!string.Equals(suppliedEtag, currentEtag, StringComparison.Ordinal))
{
    var exception = new PreconditionFailedException(
        $"The supplied If-Match value {suppliedEtag} does not match the current ETag {currentEtag}.",
        new InvalidOperationException("The resource changed after the client read it."));

    Console.WriteLine(exception.StatusCode);
    Console.WriteLine(exception.Message);
    Console.WriteLine(exception.InnerException?.Message);
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="8374effecf66f5eec5c1e325"><a href="~/run-actions/8374effecf66f5eec5c1e325/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/8374effecf66f5eec5c1e325/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
