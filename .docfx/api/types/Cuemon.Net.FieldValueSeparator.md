---
uid: Cuemon.Net.FieldValueSeparator
example:
- *content
---

The following example demonstrates how to use <see cref="Cuemon.Net.FieldValueSeparator"/> to specify the separator for query string key-value pairs.

```csharp
using System;
using Cuemon.Net;

namespace MyApp.Examples;

public class FieldValueSeparatorExample
{
    public void Demonstrate()
    {
        var separator = FieldValueSeparator.Ampersand;

        switch (separator)
        {
            case FieldValueSeparator.Ampersand:
                Console.WriteLine("Using & separator for query string parameters (default).");
                break;
            case FieldValueSeparator.Semicolon:
                Console.WriteLine("Using ; separator for query string parameters.");
                break;

}}
}

```
