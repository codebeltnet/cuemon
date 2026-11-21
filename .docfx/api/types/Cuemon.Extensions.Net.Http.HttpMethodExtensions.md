---
uid: Cuemon.Extensions.Net.Http.HttpMethodExtensions
example:
- *content
---

The following example demonstrates how to convert between System.Net.Http.HttpMethod and the Cuemon HttpMethods enum using HttpMethodExtensions.

```csharp
// Program.cs
using System;
using System.Net.Http;
using Cuemon.Extensions.Net.Http;
using Cuemon.Net.Http;

            // Convert System.Net.Http.HttpMethod to the Cuemon HttpMethods enum
            HttpMethod getMethod = HttpMethod.Get;
            HttpMethods method = getMethod.ToHttpMethod();
            Console.WriteLine($"{getMethod} -> {method}"); // GET -> Get

            HttpMethod postMethod = HttpMethod.Post;
            HttpMethods post = postMethod.ToHttpMethod();
            Console.WriteLine($"{postMethod} -> {post}");  // POST -> Post

            // Custom HTTP methods are also supported
            var patchMethod = new HttpMethod("PATCH");
            HttpMethods patch = patchMethod.ToHttpMethod();
            Console.WriteLine($"{patchMethod} -> {patch}"); // PATCH -> Patch

            // Check flags with bitwise operations
            if (method.HasFlag(HttpMethods.Get))
            {
                Console.WriteLine("This is a GET request.");

}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="140d6619210398202cf76197"><a href="~/run-actions/140d6619210398202cf76197/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/140d6619210398202cf76197/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
