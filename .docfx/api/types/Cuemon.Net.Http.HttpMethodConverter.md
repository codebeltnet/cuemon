---
uid: Cuemon.Net.Http.HttpMethodConverter
example:
- *content
---

The following example demonstrates how to convert a `System.Net.Http.HttpMethod` to the corresponding `HttpMethods` enum value using `HttpMethodConverter`. It converts the GET method and prints the result.

```csharp
using System;
using System.Net.Http;
using Cuemon.Net.Http;

namespace MyApp.Examples;

public static class HttpMethodConverterExample
{
    public static void Demonstrate()
    {
        HttpMethod get = HttpMethod.Get;
        HttpMethods result = HttpMethodConverter.ToHttpMethod(get);
        Console.WriteLine(result); // Get
    }
}
```
