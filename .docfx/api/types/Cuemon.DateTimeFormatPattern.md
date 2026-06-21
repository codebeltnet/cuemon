---
uid: Cuemon.DateTimeFormatPattern
example:
- *content
---

The following example demonstrates how to use `DateTimeFormatPattern` to select a format pattern for date and time display.

```csharp
using System;
using Cuemon;

namespace MyApp.Examples;

public class DateTimeFormatPatternExample
{
    public void Demonstrate()
    {
        var shortDate = DateTimeFormatPattern.ShortDate;
        var longDateTime = DateTimeFormatPattern.LongDateTime;

        var now = DateTime.Now;
        Console.WriteLine($"Selected pattern: {shortDate}");
        Console.WriteLine($"Selected pattern: {longDateTime}");

        // Use the pattern with formatting utilities
        // that accept DateTimeFormatPattern to control output.

}
}

```
