---
uid: Cuemon.Data.UniqueIndexViolationException
example:
- *content
---

The following example demonstrates how to use `UniqueIndexViolationException` to represent a unique index or unique constraint violation in a data source.

```csharp
using System;
using Cuemon.Data;

namespace MyApp.Data
{
    public sealed class UniqueIndexViolationExceptionExample
    {
        public void Demonstrate()
        {
            try
            {
                throw new UniqueIndexViolationException("Cannot insert duplicate key row in object 'dbo.Users'.");
            }
            catch (UniqueIndexViolationException ex)
            {
                Console.WriteLine(ex.Message);
            }

            var wrapped = new UniqueIndexViolationException(
                "Failed to register user.",
                new InvalidOperationException("IX_Users_Email was violated."));

            Console.WriteLine(wrapped.Message);
            Console.WriteLine(wrapped.InnerException?.Message);

            var empty = new UniqueIndexViolationException();
            Console.WriteLine(empty.GetType().Name);
        }
    }
}
```
