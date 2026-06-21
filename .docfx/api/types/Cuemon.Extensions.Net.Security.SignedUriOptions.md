---
uid: Cuemon.Extensions.Net.Security.SignedUriOptions
example:
- *content
---

The following example demonstrates how to configure `SignedUriOptions` for generating time-limited signed URIs with HMAC-SHA256.

```csharp
using Cuemon.Extensions.Net.Security;
using Cuemon.Security.Cryptography;

namespace MyApp.Examples;

public class SignedUriOptionsExample
{
    public void Demonstrate()
    {
        var options = new SignedUriOptions
        {
            Algorithm = KeyedCryptoAlgorithm.HmacSha256,
            SignatureFieldName = "sig",
            StartFieldName = "st",
            ExpiryFieldName = "se",
            UrlEncode = true
        };

}
}

```
