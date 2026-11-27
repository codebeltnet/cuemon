---
uid: Cuemon.AspNetCore.Authentication.AuthorizationHeaderOptions
example:
- *content
---

The following example demonstrates how to customize the delimiters used when parsing authorization header credentials.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Authentication;

var options = new AuthorizationHeaderOptions
{
    CredentialsDelimiter = ", ",
    CredentialsKeyValueDelimiter = "="
};

options.ValidateOptions();

Console.WriteLine(options.CredentialsDelimiter);
Console.WriteLine(options.CredentialsKeyValueDelimiter);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="5a89a4022884f3554e64af50"><a href="~/run-actions/5a89a4022884f3554e64af50/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/5a89a4022884f3554e64af50/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
