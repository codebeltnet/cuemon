---
uid: Cuemon.Net.Http.HttpMethods
example:
- *content
---

The following example demonstrates how to combine and test <see cref="HttpMethods" /> flags.

```csharp
using System;
using Cuemon.Net.Http;

namespace MyApp.Examples;

public static class HttpMethodsExample
{
    public static void Demonstrate()
    {
        var allowedMethods = HttpMethods.Get | HttpMethods.Post | HttpMethods.Head;

        Console.WriteLine(allowedMethods.HasFlag(HttpMethods.Get));
        Console.WriteLine(allowedMethods.HasFlag(HttpMethods.Delete));
        Console.WriteLine(allowedMethods & ~HttpMethods.Head);
    }
}
```
