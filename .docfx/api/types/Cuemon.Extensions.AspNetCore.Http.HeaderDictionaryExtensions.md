---
uid: Cuemon.Extensions.AspNetCore.Http.HeaderDictionaryExtensions
example:
- *content
---

The following example demonstrates how to add or update HTTP headers in an [IHeaderDictionary](https://learn.microsoft.com/dotnet/api/microsoft.aspnetcore.http.iheaderdictionary) using the <xref:Cuemon.Extensions.AspNetCore.Http.HeaderDictionaryExtensions> class.

```csharp
// Program.cs
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Cuemon.Extensions.AspNetCore.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

var headers = new HeaderDictionary();

// Add or update a single header
headers.AddOrUpdateHeader("X-Custom", new StringValues("my-value"));

// Add or update multiple headers from an HttpResponseHeaders collection
var responseMessage = new HttpResponseMessage();
responseMessage.Headers.Add("X-Trace", "abc123");
responseMessage.Headers.Add("X-Session", "session-456");

headers.AddOrUpdateHeaders(responseMessage.Headers);

// Verify headers
Console.WriteLine(headers["X-Custom"]);      // my-value
Console.WriteLine(headers["X-Trace"]);       // abc123
Console.WriteLine(headers["X-Session"]);     // session-456
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="ddf6e88746e4103ae34f2222"><a href="~/run-actions/ddf6e88746e4103ae34f2222/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/ddf6e88746e4103ae34f2222/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
