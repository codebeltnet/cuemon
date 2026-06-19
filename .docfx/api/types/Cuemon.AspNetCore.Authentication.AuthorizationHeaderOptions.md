---
uid: Cuemon.AspNetCore.Authentication.AuthorizationHeaderOptions
example:
- *content
---

The following example demonstrates how to customize the delimiters used when parsing authorization header credentials.

```csharp
using System;
using Cuemon.AspNetCore.Authentication;

namespace MyApp.Examples;

public static class AuthorizationHeaderOptionsExample
{
    public static void Demonstrate()
    {
        var options = new AuthorizationHeaderOptions
        {
            CredentialsDelimiter = ", ",
            CredentialsKeyValueDelimiter = "="
        };

        options.ValidateOptions();

        Console.WriteLine(options.CredentialsDelimiter);
        Console.WriteLine(options.CredentialsKeyValueDelimiter);
    }
}

```
