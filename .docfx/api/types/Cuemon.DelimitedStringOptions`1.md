---
uid: Cuemon.DelimitedStringOptions`1
example:
- *content
---

The following example demonstrates how to use <see cref="DelimitedStringOptions{T}"/> to configure the conversion of a sequence of objects into a delimited string.

```csharp
using System;
using System.Globalization;
using Cuemon;

namespace Contoso.Telemetry;

public sealed class DelimitedStringOptionsOfTExample
{
    public static void Run()
    {
        var options = new DelimitedStringOptions<int>
        {
            Delimiter = " | ",
            StringConverter = number => number.ToString("X2", CultureInfo.InvariantCulture)
        };

        int[] numbers = { 1, 2, 3, 4 };
        string hex = DelimitedString.Create(numbers, setup =>
        {
            setup.Delimiter = options.Delimiter;
            setup.StringConverter = options.StringConverter;
        });

        Console.WriteLine(hex);
    }
}
```
