---
uid: Cuemon.Net.Http.HttpMethodConverter
example:
- *content
---

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
