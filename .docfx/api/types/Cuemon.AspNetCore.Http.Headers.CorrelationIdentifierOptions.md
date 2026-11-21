---
uid: Cuemon.AspNetCore.Http.Headers.CorrelationIdentifierOptions
example:
- *content
---

The following example shows how to configure `CorrelationIdentifierOptions` with a custom correlation token. After validation, it prints the token's correlation ID.

```csharp
// Program.cs
using System;
using Cuemon.Messaging;
using Cuemon.AspNetCore.Http.Headers;

        var options = new CorrelationIdentifierOptions
{
    Token = new CorrelationToken("corr-42")
};

options.ValidateOptions();
Console.WriteLine(options.Token.CorrelationId);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="f966be9c65502cf8bed9a5f5"><a href="~/run-actions/f966be9c65502cf8bed9a5f5/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/f966be9c65502cf8bed9a5f5/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
