---
uid: Cuemon.Extensions.Net.Security.StringExtensions
example:
- *content
---

The following example demonstrates how to sign a URI string and validate the signature later.

```csharp
using System;
using System.Security;
using System.Text;
using Cuemon.Extensions.Net.Security;

namespace MyApp.Examples
{
    public static class SignedStringExtensionsExample
    {
        public static void Demonstrate()
        {
            var secret = Encoding.UTF8.GetBytes("1234");
            var uriString = "https://example.com/search?q=cuemon";
            var signedUri = uriString.ToSignedUri(secret, DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow.AddMinutes(1));

            signedUri.OriginalString.ValidateSignedUri(secret);

            Console.WriteLine(signedUri);

            try
            {
                var tampered = new UriBuilder(signedUri);
                tampered.Query = tampered.Query.TrimStart('?') + "&tampered=1";
                tampered.Uri.OriginalString.ValidateSignedUri(secret);
            }
            catch (SecurityException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
```
