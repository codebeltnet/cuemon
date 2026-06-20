---
uid: Cuemon.AspNetCore.Authentication.Basic.BasicFields
example:
- *content
---

The following example demonstrates how to use the field constants of `BasicFields` to construct a Basic authorization header.

```csharp
using System;
using Cuemon.AspNetCore.Authentication.Basic;

namespace MyApp.Examples;

public static class BasicFieldsExample
{
    public static void Demonstrate()
    {
        var builder = new BasicAuthorizationHeaderBuilder();
        builder.AddUserName("alice");
        builder.AddPassword("s3cret");
        var header = builder.Build();

        Console.WriteLine(BasicAuthorizationHeader.Scheme);
        Console.WriteLine(BasicFields.Realm);
        Console.WriteLine(BasicFields.Credentials);
    }
}

```
