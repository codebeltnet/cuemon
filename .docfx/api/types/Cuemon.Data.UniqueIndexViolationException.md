---
uid: Cuemon.Data.UniqueIndexViolationException
example:
- *content
---

`UniqueIndexViolationException` represents a unique index or constraint violation error, with support for inner exceptions and parameterless construction. This example throws a new instance with a descriptive message about a duplicate key in `dbo.Users` and catches it to print the message. It also creates a wrapped exception with an inner `InvalidOperationException` as the cause, and demonstrates that the default parameterless constructor has no inner exception.

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
            Console.WriteLine($"Default exception has no inner exception: {empty.InnerException is null}");
        }
    }
}
```
