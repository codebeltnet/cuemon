---
uid: Cuemon.AspNetCore.Http.Headers.RetryConditionScope
example:
- *content
---

The following example demonstrates how to use `RetryConditionScope` to choose the format of a Retry-After header. It compares delta-seconds and absolute-date scopes and prints the resulting header value.

```csharp
// Program.cs
using System;
using System.Net.Http.Headers;
using Cuemon.AspNetCore.Http.Headers;

var scope = RetryConditionScope.DeltaSeconds;
string retryAfter = scope == RetryConditionScope.DeltaSeconds
    ? new RetryConditionHeaderValue(TimeSpan.FromSeconds(30)).ToString()
    : new RetryConditionHeaderValue(DateTimeOffset.UtcNow.AddMinutes(1)).ToString();

Console.WriteLine(retryAfter);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="bb7040215b715afea36c9c22"><a href="~/run-actions/bb7040215b715afea36c9c22/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/bb7040215b715afea36c9c22/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
