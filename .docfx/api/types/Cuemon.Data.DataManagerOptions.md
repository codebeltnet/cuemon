---
uid: Cuemon.Data.DataManagerOptions
example:
- *content
---

`DataManagerOptions` configures connection strings, reader behavior, and connection lifecycle settings for use with `DataManager`. This example creates an options instance with `ConnectionString = "Data Source=app.db"`, `PreferredReaderBehavior` set to `SequentialAccess | CloseConnection`, and both `LeaveConnectionOpen` and `LeaveCommandOpen` set to `false`. After configuration, `ValidateOptions()` is called to confirm the settings are valid. Console output prints the connection string, reader behavior flag, and lifecycle settings.

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
