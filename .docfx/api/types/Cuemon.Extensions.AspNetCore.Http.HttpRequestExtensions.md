---
uid: Cuemon.Extensions.AspNetCore.Http.HttpRequestExtensions
example:
- *content
---

`HttpRequestExtensions` provides extension methods for `HttpRequest` that inspect accepted MIME types, HTTP method type, and client-side caching status. This example sets up a request with `Accept` header values (`text/html`, `application/json`, `*/*` with quality scores), `If-None-Match` with an ETag, and `If-Modified-Since` with a date, then calls `AcceptMimeTypesOrderedByQuality` to sort by q-value, `IsGetOrHeadMethod` to check the HTTP method, and `IsClientSideResourceCached` with both a `ChecksumBuilder` ETag and a `Last-Modified` date overload. Console output shows the sorted MIME types (`application/json, text/html, */*`), the method check results, and the cache state booleans.

```csharp
// Program.cs
using System;
using System.Text;
using System.Collections.Generic;
using Cuemon.Data.Integrity;
using Cuemon.Extensions.AspNetCore.Http;
using Cuemon.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

var context = new DefaultHttpContext();
var request = context.Request;

request.Headers.Accept = new StringValues(new[] { "text/html;q=0.8", "application/json;q=0.9", "*/*;q=0.1" });
IEnumerable<string> preferredTypes = request.AcceptMimeTypesOrderedByQuality();
Console.WriteLine(string.Join(", ", preferredTypes));

request.Method = "GET";
Console.WriteLine(request.IsGetOrHeadMethod());

request.Method = "POST";
Console.WriteLine(request.IsGetOrHeadMethod());

request.Method = "GET";
request.Headers["If-None-Match"] = "\"abc123\"";
var checksumBuilder = new ChecksumBuilder(() => HashFactory.CreateFnv128());
checksumBuilder.CombineWith(Encoding.UTF8.GetBytes("content-data"));
Console.WriteLine(request.IsClientSideResourceCached(checksumBuilder));

request.Headers["If-Modified-Since"] = "Tue, 15 Jun 2024 10:00:00 GMT";
var lastModified = new DateTime(2024, 6, 15, 9, 0, 0, DateTimeKind.Utc);
Console.WriteLine(request.IsClientSideResourceCached(lastModified));
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="d5eba7c050506e62130e25e3"><a href="~/run-actions/d5eba7c050506e62130e25e3/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/d5eba7c050506e62130e25e3/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
