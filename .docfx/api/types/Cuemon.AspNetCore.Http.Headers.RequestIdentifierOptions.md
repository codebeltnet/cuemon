---
uid: Cuemon.AspNetCore.Http.Headers.RequestIdentifierOptions
example:
- *content
---

The following example demonstrates how to configure <xref cref="Cuemon.AspNetCore.Http.Headers.RequestIdentifierOptions"/> to customize the Request-ID header name and token generator.

```csharp
// Program.cs
using System;
using Cuemon.Messaging;
using Cuemon.AspNetCore.Http.Headers;

        var options = new RequestIdentifierOptions
{
    Token = new RequestToken("req-42")
};

options.ValidateOptions();
Console.WriteLine(options.Token.RequestId);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="e0724bf05a90cdb3db53e167"><a href="~/run-actions/e0724bf05a90cdb3db53e167/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/e0724bf05a90cdb3db53e167/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
