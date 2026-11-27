---
uid: Cuemon.Extensions.AspNetCore.Data.Integrity.CacheValidatorExtensions
example:
- *content
---

The following example demonstrates how to turn assembly-backed cache validators into ETag headers for weak and strong validation scenarios.

```csharp
// Program.cs
using System;
using Cuemon.Data.Integrity;
using Cuemon.Extensions.AspNetCore.Data.Integrity;
using Microsoft.Net.Http.Headers;

CacheValidator weakValidator = CacheValidatorFactory.CreateValidator(typeof(CacheValidatorExtensions).Assembly);
EntityTagHeaderValue weakEntityTag = weakValidator.ToEntityTagHeaderValue();

CacheValidator strongValidator = CacheValidatorFactory.CreateValidator(
    typeof(CacheValidatorExtensions).Assembly,
    setup: options => options.BytesToRead = int.MaxValue);

EntityTagHeaderValue strongEntityTag = strongValidator.ToEntityTagHeaderValue();

Console.WriteLine(weakEntityTag);
Console.WriteLine(strongEntityTag);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="468539edc62a30bb2cca15ef"><a href="~/run-actions/468539edc62a30bb2cca15ef/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/468539edc62a30bb2cca15ef/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
