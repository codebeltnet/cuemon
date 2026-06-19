---
uid: Cuemon.AspNetCore.Authentication.Basic.BasicAuthorizationHeaderBuilder
example:
- *content
---

The following example demonstrates how to build a Basic Authorization header value using `BasicAuthorizationHeaderBuilder`.

```csharp
using Cuemon.AspNetCore.Authentication.Basic;

namespace MyApp.Examples;

public class BasicAuthorizationHeaderBuilderExample
{
    public void Demonstrate()
    {
        var builder = new BasicAuthorizationHeaderBuilder();
        builder.AddUserName("alice");
        builder.AddPassword("password");
        var headerValue = builder.Build(); // "Basic YWxpY2U6cGFzc3dvcmQ="

}
}

```
