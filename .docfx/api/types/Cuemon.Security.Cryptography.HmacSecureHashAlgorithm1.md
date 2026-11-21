---
uid: Cuemon.Security.Cryptography.HmacSecureHashAlgorithm1
example:
- *content
---

The following example demonstrates how to compute an HMAC-SHA1 hash with <see cref="HmacSecureHashAlgorithm1" />.

```csharp
// Program.cs
using System;
using System.Text;
using Cuemon.Security.Cryptography;
using MyApp.Examples;

HmacSecureHashAlgorithm1Example.Demonstrate();

namespace MyApp.Examples
{
    public static class HmacSecureHashAlgorithm1Example
    {
        public static void Demonstrate()
        {
            var algorithm = new HmacSecureHashAlgorithm1(CreateSecret(), null);
            var result = algorithm.ComputeHash(Encoding.UTF8.GetBytes("Authenticate this message"));

            Console.WriteLine(result.GetBytes().Length);
            Console.WriteLine(result.ToBase64String());
        }

        private static byte[] CreateSecret() => Encoding.UTF8.GetBytes("docs-secret-sha1");
    }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="6d716aca3cabc9f82f9b315b"><a href="~/run-actions/6d716aca3cabc9f82f9b315b/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/6d716aca3cabc9f82f9b315b/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
