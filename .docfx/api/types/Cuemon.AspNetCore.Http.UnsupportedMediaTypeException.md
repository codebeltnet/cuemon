---
uid: Cuemon.AspNetCore.Http.UnsupportedMediaTypeException
example:
- *content
---

The following example demonstrates how to return an `UnsupportedMediaTypeException` when the API receives an unsupported file upload format.

```csharp
using System;
using Cuemon.AspNetCore.Http;

namespace MyApp.Examples;

public class UnsupportedMediaTypeExceptionExample
{
    public void Demonstrate()
    {
        try
        {
            throw new UnsupportedMediaTypeException("text/html");
        }
        catch (UnsupportedMediaTypeException ex)
        {
            Console.WriteLine(ex.StatusCode); // 415
        }
    }
}
```
