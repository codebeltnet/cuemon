---
uid: Cuemon.Data.DataStatementOptions
example:
- *content
---

`DataStatementOptions` provides configuration for `DataStatement` instances including command type, timeout, and parameters. This example creates a text command option with a 30-second timeout and prints its `CommandType` and timeout values, then creates a stored procedure option with a 5-minute timeout and an empty `IDataParameter` array. After configuration, `ValidateOptions()` is called to confirm the stored procedure settings are valid. Console output displays the command type, timeout in seconds, and validation status.

```csharp
using System;
using System.Data;
using Cuemon.Data;

namespace MyApp.Data
{
    public class DataStatementOptionsExample
    {
        public void Demonstrate()
        {
            // Create options for a text command with default timeout (90 seconds)
            var options = new DataStatementOptions
            {
                Type = CommandType.Text,
                Timeout = TimeSpan.FromSeconds(30)
            };

            Console.WriteLine($"Command type: {options.Type}");
            Console.WriteLine($"Timeout: {options.Timeout.TotalSeconds} seconds");
            Console.WriteLine($"Default timeout: {DataStatementOptions.DefaultTimeout.TotalSeconds} seconds");

            // Configure for a stored procedure
            var spOptions = new DataStatementOptions
            {
                Type = CommandType.StoredProcedure,
                Timeout = TimeSpan.FromMinutes(5),
                Parameters = Array.Empty<IDataParameter>()
            };

            // Validate that parameters is not null
            spOptions.ValidateOptions();
            Console.WriteLine("Stored procedure options are valid.");

}}
}

```
