---
uid: Cuemon.Net.Http.HttpRequestOptions
example:
- *content
---

The following example demonstrates how to configure and send an HTTP request using `HttpRequestOptions` with the `HttpManager`.

```csharp
// Program.cs
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Cuemon.Net.Http;
using HttpRequestOptions = Cuemon.Net.Http.HttpRequestOptions;

// Direct instantiation of HttpRequestOptions
var requestOptions = new HttpRequestOptions();
requestOptions.Request.Method = HttpMethod.Get;

requestOptions.Request.Headers.Add("Accept", "application/json");
Console.WriteLine($"{requestOptions.Request.Method} {string.Join(", ", requestOptions.Request.Headers.Accept)}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="ce261fa5e4c42172dba24385"><a href="~/run-actions/ce261fa5e4c42172dba24385/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/ce261fa5e4c42172dba24385/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
