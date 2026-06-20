---
uid: Cuemon.Extensions.Net.Security.UriExtensions
example:
- *content
---

The following example demonstrates how to sign and validate a <see cref="Uri" /> with <see cref="UriExtensions" />.

```csharp
using System;
using System.Text;
using Cuemon.Extensions.Net.Security;

namespace MyApp.Examples
{
    public static class SignedUriExtensionsExample
    {
        public static void Demonstrate()
        {
            var secret = Encoding.UTF8.GetBytes("1234");
            var location = new Uri("https://example.com/search?q=cuemon");
            var signed = location.ToSignedUri(secret, DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow.AddMinutes(1));

            signed.ValidateSignedUri(secret);

            Console.WriteLine(signed != location);
            Console.WriteLine(signed.AbsoluteUri);
        }
    }
}
```
