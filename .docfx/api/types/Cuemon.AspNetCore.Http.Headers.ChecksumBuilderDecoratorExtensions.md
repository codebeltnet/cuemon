---
uid: Cuemon.AspNetCore.Http.Headers.ChecksumBuilderDecoratorExtensions
example:
- *content
---

The following example demonstrates how to create an `EntityTagHeaderValue` from a `ChecksumBuilder` using the decorator pattern.

```csharp
// Program.cs
using System;
using Cuemon;
using Cuemon.Data.Integrity;
using Cuemon.Security;
using Cuemon.AspNetCore.Http.Headers;

        var builder = new ChecksumBuilder(() => HashFactory.CreateFnv128());
var entityTag = Decorator.Enclose(builder).ToEntityTagHeaderValue(isWeak: true);

Console.WriteLine(entityTag.ToString());
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="67b4a4ec7dd8864f385beb2a"><a href="~/run-actions/67b4a4ec7dd8864f385beb2a/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/67b4a4ec7dd8864f385beb2a/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
