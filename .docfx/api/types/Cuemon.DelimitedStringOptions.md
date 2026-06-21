---
uid: Cuemon.DelimitedStringOptions
example:
- *content
---

The following example demonstrates how to use <see cref="DelimitedStringOptions"/> to configure custom delimiters and qualifiers when parsing delimited strings using <see cref="DelimitedString"/>.

```csharp
using System;
using Cuemon; // for DelimitedStringOptions, DelimitedString

namespace MyApp.Examples;

public class DelimitedStringOptionsExample
{
    public void Demonstrate()
    {
        // Default options: comma delimiter, double-quote qualifier
        var defaultOptions = new DelimitedStringOptions();
        Console.WriteLine($"Delimiter: '{defaultOptions.Delimiter}'"); // ','
        Console.WriteLine($"Qualifier: '{defaultOptions.Qualifier}'"); // '"'

        // Custom tab-delimited options
        var tabOptions = new DelimitedStringOptions
        {
            Delimiter = "\t",
            Qualifier = "'"
        };

        // Parse a tab-delimited line using the setup action
        string tabLine = "Alice\t30\tNew York";
        string[] fields = DelimitedString.Split(tabLine, o =>
        {
            o.Delimiter = tabOptions.Delimiter;
            o.Qualifier = tabOptions.Qualifier;
        });
        Console.WriteLine($"Fields: {string.Join(", ", fields)}"); // Alice, 30, New York

        // Parse a pipe-delimited line
        string pipeLine = "'Bob'|'25'|'London'";
        fields = DelimitedString.Split(pipeLine, o =>
        {
            o.Delimiter = "|";
            o.Qualifier = "'";
        });
        Console.WriteLine($"Fields: {string.Join(", ", fields)}"); // Bob, 25, London

}
}

```
