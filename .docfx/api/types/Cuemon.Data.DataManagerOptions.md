---
uid: Cuemon.Data.DataManagerOptions
example:
- *content
---

The following example demonstrates how to configure `DataManagerOptions` with a connection string, reader behavior, and connection lifecycle settings.

```csharp
using System;
using System.Data;
using Cuemon.Data;

namespace MyApp.Data
{
    public sealed class DataManagerOptionsExample
    {
        public void Demonstrate()
        {
            var options = new DataManagerOptions
            {
                ConnectionString = "Data Source=app.db",
                PreferredReaderBehavior = CommandBehavior.SequentialAccess | CommandBehavior.CloseConnection,
                LeaveConnectionOpen = false,
                LeaveCommandOpen = false
            };

            options.ValidateOptions();

            Console.WriteLine($"Connection: {options.ConnectionString}");
            Console.WriteLine($"Reader behavior: {options.PreferredReaderBehavior}");
            Console.WriteLine($"Leave connection open: {options.LeaveConnectionOpen}");
            Console.WriteLine($"Leave command open: {options.LeaveCommandOpen}");
        }
    }
}
```
