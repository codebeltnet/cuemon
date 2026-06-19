---
uid: Cuemon.FormattingOptions
example:
- *content
---

The following example demonstrates how to configure a <see cref="Cuemon.FormattingOptions"/> with a custom format provider to control numeric formatting.

```csharp
using System;
using System.Globalization;
using Cuemon;

namespace MyApp.Examples;

public class FormattingOptionsExample
{
    public void Demonstrate()
    {
        var options = new FormattingOptions
        {
            FormatProvider = new CultureInfo("da-DK")
        };

        var value = 1234.56;
        var formatted = value.ToString("N2", options.FormatProvider);

        Console.WriteLine(formatted); // Output depends on the culture (e.g., "1.234,56" for da-DK)

}
}

```
