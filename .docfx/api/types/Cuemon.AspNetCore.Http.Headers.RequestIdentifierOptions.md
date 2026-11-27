---
uid: Cuemon.AspNetCore.Http.Headers.RequestIdentifierOptions
example:
- *content
---

The following example demonstrates how to configure <xref cref="Cuemon.AspNetCore.Http.Headers.RequestIdentifierOptions"/> to customize the Request-ID header name and token.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Http.Headers;
using Cuemon.Messaging;

var options = new RequestIdentifierOptions
{
    HeaderName = "X-Custom-Request-ID",
    Token = new RequestToken("req-42")
};

options.ValidateOptions();
Console.WriteLine($"{options.HeaderName}: {options.Token.RequestId}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="4e306eac5aebaf153a283c89"><a href="~/run-actions/4e306eac5aebaf153a283c89/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/4e306eac5aebaf153a283c89/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
