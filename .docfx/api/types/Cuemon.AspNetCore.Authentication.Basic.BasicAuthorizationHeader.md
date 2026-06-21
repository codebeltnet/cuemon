---
uid: Cuemon.AspNetCore.Authentication.Basic.BasicAuthorizationHeader
example:
- *content
---

The following example demonstrates how to serialize and parse a Basic authorization header.

```csharp
using System;
using Cuemon.AspNetCore.Authentication.Basic;

namespace MyApp.Examples;

public static class BasicAuthorizationHeaderExample
{
    public static void Demonstrate()
    {
        var header = new BasicAuthorizationHeader("Agent", "Test");
        var parsed = BasicAuthorizationHeader.Create(header.ToString());

        Console.WriteLine(parsed.UserName);
        Console.WriteLine(parsed.Password);
    }
}

```
