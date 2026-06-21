---
uid: Cuemon.AspNetCore.Authentication.Hmac.HmacAuthorizationHeader
example:
- *content
---

The following example demonstrates how to parse an HMAC Authorization header value.

```csharp
using Cuemon.AspNetCore.Authentication.Hmac;

namespace MyApp.Examples;

public class HmacAuthorizationHeaderExample
{
    public void Demonstrate()
    {
        var header = HmacAuthorizationHeader.Create(
            HmacFields.Scheme,
            "HMAC Credential=alice/some-scope, SignedHeaders=date;host, Signature=abc123");
        var clientId = header.ClientId; // "alice"
    }
}
```
