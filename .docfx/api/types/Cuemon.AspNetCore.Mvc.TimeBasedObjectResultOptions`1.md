---
uid: Cuemon.AspNetCore.Mvc.TimeBasedObjectResultOptions`1
example:
- *content
---

The following example demonstrates how to configure <xref cref="Cuemon.AspNetCore.Mvc.TimeBasedObjectResultOptions{T}"/> to provide timestamp providers for generating Last-Modified headers.

```csharp
using System;
using Cuemon.AspNetCore.Mvc;

namespace MyApp.Examples;

public class TimeBasedObjectResultOptionsExample
{
    public void Demonstrate()
    {
        var options = new TimeBasedObjectResultOptions<DateTime>
        {
            TimestampProvider = value => value,
            ChangedTimestampProvider = value => value.AddHours(1)
        };

        // Validate that the required properties are configured
        options.ValidateOptions();

        var created = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        Console.WriteLine($"Created: {options.TimestampProvider(created)}");
        Console.WriteLine($"Modified: {options.ChangedTimestampProvider(created)}");

}
}

```
