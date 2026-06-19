---
uid: Cuemon.AspNetCore.Http.ForbiddenException
example:
- *content
---

The following example demonstrates how to use `ForbiddenException` to reject unauthorized access in an authorization filter.

```csharp
using System;
using Cuemon.AspNetCore.Http;

namespace MyApp.Examples;

public class ForbiddenExceptionExample
{
    public void Demonstrate()
    {
        try
        {
            var userRole = "guest";
            if (userRole != "admin")
            {
                throw new ForbiddenException("Only administrators can perform this action.");
            }
        }
        catch (ForbiddenException ex)
        {
            Console.WriteLine(ex.StatusCode); // 403
            Console.WriteLine(ex.Message);    // Only administrators can perform this action.
        }
    }
}
```
