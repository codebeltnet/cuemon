---
uid: Cuemon.AspNetCore.Http.NotFoundException
example:
- *content
---

The following example demonstrates how to use `NotFoundException` when a requested resource does not exist.

```csharp
using System;
using Cuemon.AspNetCore.Http;

namespace MyApp.Examples;

public class NotFoundExceptionExample
{
    public void Demonstrate()
    {
        try
        {
            var userId = 999;
            var user = FindUser(userId);
            if (user == null)
            {
                throw new NotFoundException($"User with ID {userId} was not found.");
            }
        }
        catch (NotFoundException ex)
        {
            Console.WriteLine(ex.StatusCode); // 404
            Console.WriteLine(ex.Message);    // User with ID 999 was not found.
        }
    }

    private object FindUser(int id) => null;
}
```
