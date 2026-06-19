---
uid: Cuemon.Extensions.Net.Http.HttpMethodExtensions
example:
- *content
---

The following example demonstrates how to convert between System.Net.Http.HttpMethod and the Cuemon HttpMethods enum using HttpMethodExtensions.

```csharp
using System;
using System.Net.Http;
using Cuemon.Extensions.Net.Http;
using Cuemon.Net.Http;

namespace MyApp.Net
{
    public class HttpMethodExtensionsExample
    {
        public void Demonstrate()
        {
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

}}}
}

```
