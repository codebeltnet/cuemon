---
uid: Cuemon.Diagnostics.ExceptionDescriptorAttribute
example:
- *content
---

The following example demonstrates how to annotate a method with <see cref="ExceptionDescriptorAttribute"/> and inspect the configured metadata at runtime.

```csharp
using System;
using System.Linq;
using Cuemon.Diagnostics;

namespace MyApp.Examples;

public static class ExceptionDescriptorAttributeExample
{
    [ExceptionDescriptor(typeof(ArgumentNullException),
        Code = "ERR_NULL_ARGUMENT",
        Message = "A required parameter was not provided.",
        HelpLink = "https://example.com/errors/null-argument")]
    public static void ProcessOrder(string orderId)
    {
        if (orderId == null) { throw new ArgumentNullException(nameof(orderId)); }
    }

    public static void Demonstrate()
    {
        var attribute = (ExceptionDescriptorAttribute)Attribute
            .GetCustomAttributes(typeof(ExceptionDescriptorAttributeExample).GetMethod(nameof(ProcessOrder))!, typeof(ExceptionDescriptorAttribute))
            .Single();

        Console.WriteLine(attribute.FailureType.Name);
        Console.WriteLine(attribute.Code);
        Console.WriteLine(attribute.Message);
        Console.WriteLine(attribute.HelpLink);
    }
}

```
