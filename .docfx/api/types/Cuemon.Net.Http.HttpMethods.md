---
uid: Cuemon.Net.Http.HttpMethods
example:
- *content
---

The following example demonstrates how to combine and test <see cref="HttpMethods" /> flags.

```csharp
// Program.cs
using System;
using Cuemon.Net.Http;

var allowedMethods = HttpMethods.Get | HttpMethods.Post | HttpMethods.Head;

Console.WriteLine(allowedMethods.HasFlag(HttpMethods.Get));
Console.WriteLine(allowedMethods.HasFlag(HttpMethods.Delete));
Console.WriteLine(allowedMethods & ~HttpMethods.Head);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="eb2672f67d28cd9e77131c03"><a href="~/run-actions/eb2672f67d28cd9e77131c03/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/eb2672f67d28cd9e77131c03/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
