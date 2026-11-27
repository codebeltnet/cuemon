---
uid: Cuemon.Net.Http.HttpMethodConverter
example:
- *content
---

The following example demonstrates how to convert a `System.Net.Http.HttpMethod` to the corresponding `HttpMethods` enum value using `HttpMethodConverter`. It converts the GET method and prints the result.

```csharp
// Program.cs
using System;
using System.Net.Http;
using Cuemon.Net.Http;

HttpMethod get = HttpMethod.Get;
HttpMethods result = HttpMethodConverter.ToHttpMethod(get);
Console.WriteLine(result); // Get
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="d69dd38b6adad346c75761f5"><a href="~/run-actions/d69dd38b6adad346c75761f5/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/d69dd38b6adad346c75761f5/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
