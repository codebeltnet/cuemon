---
uid: Cuemon.AspNetCore.Http.Throttling.MemoryThrottlingCache
example:
- *content
---

The following example shows how to create a `MemoryThrottlingCache`, add a throttle request for a client, and retrieve the request count from the cache.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Http.Throttling;

        var cache = new MemoryThrottlingCache();
var request = new ThrottleRequest(new ThrottleQuota(10, TimeSpan.FromMinutes(1)));

cache.TryAdd("client-1", request);
Console.WriteLine(cache["client-1"].Total);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="0cbd8cc55f8121bbbb33d33f"><a href="~/run-actions/0cbd8cc55f8121bbbb33d33f/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/0cbd8cc55f8121bbbb33d33f/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
