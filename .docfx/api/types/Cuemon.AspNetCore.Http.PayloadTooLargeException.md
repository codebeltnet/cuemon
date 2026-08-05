---
uid: Cuemon.AspNetCore.Http.PayloadTooLargeException
example:
- *content
---

The following example demonstrates how to use <xref cref="Cuemon.AspNetCore.Http.PayloadTooLargeException"/> to signal HTTP 413 Payload Too Large responses. It constructs the exception with default and custom messages, wraps an inner exception, and uses `TryParse` to resolve by status code. A payload-size guard check then creates the exception conditionally. Each variant writes status code, reason phrase, and message to the console, showing how to communicate request-entity-size violations.

```csharp
using System.IO;
using System;
using Cuemon.AspNetCore.Http;

namespace MyApp.Examples;

public class PayloadTooLargeExceptionExample
{
    public void Demonstrate()
    {
        // Create a PayloadTooLargeException with default message
        var ex = new PayloadTooLargeException();
        Console.WriteLine(ex.StatusCode);   // 413
        Console.WriteLine(ex.ReasonPhrase); // Payload Too Large

        // Create with a custom message
        var custom = new PayloadTooLargeException("Upload size exceeds the maximum of 10 MB.");
        Console.WriteLine(custom.Message);

        // Create with inner exception
        var inner = new InvalidOperationException("Stream exceeded configured limit.");
        var withInner = new PayloadTooLargeException("Request entity too large.", inner);
        Console.WriteLine(withInner.InnerException?.Message); // Stream exceeded configured limit.

        // Use TryParse from the base class to resolve by status code
        if (HttpStatusCodeException.TryParse(413, "File exceeds 10 MB limit.", out var parsed))
        {
            Console.WriteLine(parsed.Message);        // File exceeds 10 MB limit.
            Console.WriteLine(parsed.StatusCode);     // 413
        }

        // Simulate a payload size check
        long uploadSize = 15 * 1024 * 1024; // 15 MB
        long maxSize = 10 * 1024 * 1024;    // 10 MB
        if (uploadSize > maxSize)
        {
            var rejected = new PayloadTooLargeException(
                $"Upload of {uploadSize / 1024 / 1024} MB exceeds the {maxSize / 1024 / 1024} MB limit.");
            Console.WriteLine(rejected.Message);

        }
    }
}

```
