---
uid: Cuemon.AspNetCore.Http.NotFoundException
example:
- *content
---

The following example demonstrates how to use `NotFoundException` when a requested resource does not exist.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Http;
using MyApp.Examples;

new NotFoundExceptionExample().Demonstrate();

namespace MyApp.Examples
{
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
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="f28dbb695d6a3caccc6242b6"><a href="~/run-actions/f28dbb695d6a3caccc6242b6/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/f28dbb695d6a3caccc6242b6/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
