---
uid: Cuemon.AspNetCore.Authentication.Basic.BasicAuthenticationMiddleware
example:
- *content
---

The following example demonstrates how to construct `BasicAuthenticationMiddleware` with inline option setup.

```csharp
// Program.cs
using System;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Authentication.Basic;

var middleware = new BasicAuthenticationMiddleware(_ => Task.CompletedTask, options =>
{
    options.Realm = "docs-example";
    options.RequireSecureConnection = false;
    options.Authenticator = (username, password) => null;
});

Console.WriteLine(middleware.Options.Realm);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="2a20c0efa15f4e2a7068c857"><a href="~/run-actions/2a20c0efa15f4e2a7068c857/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/2a20c0efa15f4e2a7068c857/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
