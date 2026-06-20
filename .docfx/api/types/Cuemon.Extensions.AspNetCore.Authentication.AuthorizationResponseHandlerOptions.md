---
uid: Cuemon.Extensions.AspNetCore.Authentication.AuthorizationResponseHandlerOptions
example:
- *content
---

The following example demonstrates how to configure `AuthorizationResponseHandlerOptions` to customize the behavior of the authorization response handler.

```csharp
using Cuemon.Diagnostics;
using Cuemon.Extensions.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization.Policy;

namespace MyApp.Examples;

public class AuthorizationResponseHandlerOptionsExample
{
    public void Demonstrate()
    {
        var options = new AuthorizationResponseHandlerOptions
        {
            SensitivityDetails = FaultSensitivityDetails.All,
            FallbackResponseHandler = new AuthorizationMiddlewareResultHandler()
        };
    }
}
```
