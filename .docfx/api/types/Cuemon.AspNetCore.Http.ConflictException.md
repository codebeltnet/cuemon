---
uid: Cuemon.AspNetCore.Http.ConflictException
example:
- *content
---

The following example demonstrates how to use <xref cref="Cuemon.AspNetCore.Http.ConflictException"/> to signal HTTP 409 Conflict responses.

```csharp
using System;
using Cuemon.AspNetCore.Http;

namespace MyApp.Examples;

public class ConflictExceptionExample
{
    public void Demonstrate()
    {
        // Create a ConflictException with default message
        var ex = new ConflictException();
        Console.WriteLine(ex.StatusCode);  // 409
        Console.WriteLine(ex.ReasonPhrase); // Conflict
        Console.WriteLine(ex.Message);     // The request could not be completed due to a conflict...

        // Create with a custom message
        var custom = new ConflictException("A record with the same email address already exists.");
        Console.WriteLine(custom.Message);

        // Create with inner exception
        var inner = new InvalidOperationException("Duplicate key violation.");
        var withInner = new ConflictException("Resource update conflict.", inner);
        Console.WriteLine(withInner.InnerException?.GetType().Name); // InvalidOperationException

        // Use TryParse from the base class to resolve by status code
        if (HttpStatusCodeException.TryParse(409, out var parsed))
        {
            Console.WriteLine(parsed.GetType().Name); // ConflictException
            Console.WriteLine(parsed.StatusCode);     // 409

        // Simulate a conflict check without throwing
        var dbTimestamp = DateTime.UtcNow;
        var clientTimestamp = dbTimestamp.AddHours(-1);
        if (clientTimestamp < dbTimestamp)
        {
            var conflict = new ConflictException(
                "The resource was modified by another user. Please refresh and retry.");
            Console.WriteLine(conflict.Message); // The resource was modified by another user...

}}}
}

```
