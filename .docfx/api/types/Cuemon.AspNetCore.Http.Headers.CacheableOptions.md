---
uid: Cuemon.AspNetCore.Http.Headers.CacheableOptions
example:
- *content
---

The following example shows how to configure `CacheableOptions` with cache-control and expiration headers. After validation, it prints whether each header type is enabled.

```csharp
// Program.cs
using System;
using Microsoft.Net.Http.Headers;
using Cuemon.AspNetCore.Http.Headers;

        var options = new CacheableOptions
{
    CacheControl = new CacheControlHeaderValue
    {
        Public = true,
        MaxAge = TimeSpan.FromHours(12)
    },
    Expires = new ExpiresHeaderValue(TimeSpan.FromHours(12))
};

options.ValidateOptions();
Console.WriteLine($"{options.UseCacheControl}:{options.UseExpires}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="88adb8381e9bdaffe39e4123"><a href="~/run-actions/88adb8381e9bdaffe39e4123/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/88adb8381e9bdaffe39e4123/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
