---
uid: Cuemon.ObjectFormattingOptions
example:
- *content
---

The following example demonstrates how to use <see cref="Cuemon.ObjectFormattingOptions"/> to customize the conversion of an object to a different type via <see cref="ObjectDecoratorExtensions.ChangeType{T}"/>.

```csharp
using System;
using System.Globalization;
using Cuemon;

namespace MyApp.Examples;

public class ObjectFormattingOptionsExample
{
    public void Demonstrate()
    {
        // Direct instantiation of ObjectFormattingOptions
        var options = new ObjectFormattingOptions
        {
            FormatProvider = new CultureInfo("da-DK")
        };

        var value = "1234.56";

        // Convert the string to a double using Danish formatting
        var result = Decorator.Enclose((object)value)
            .ChangeType<double>(o =>
            {
                o.FormatProvider = new CultureInfo("da-DK");
            });

        Console.WriteLine(result); // Output: 1234.56

}
}

```
