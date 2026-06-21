---
uid: Cuemon.DelimitedString
example:
- *content
---

The following example shows how to create a delimited string from an array of integers and split it back into parts using `DelimitedString`. The delimiter and string converter are configured through setup options.

```csharp
using System;
using System.Globalization;
using Cuemon;

namespace MyApp.Delimited;

public class DelimitedStringExample
{
    public void Demonstrate()
    {
        var numbers = new[] { 1, 2, 3, 4, 5 };
        string csv = DelimitedString.Create(numbers, o =>
        {
            o.Delimiter = ",";
            o.StringConverter = value => value.ToString();
        });
        Console.WriteLine(csv); // "1,2,3,4,5"

        string[] parts = DelimitedString.Split(csv, o =>
        {
            o.Delimiter = ",";
        });
        Console.WriteLine(string.Join(" | ", parts)); // "1 | 2 | 3 | 4 | 5"
    }
}
```
