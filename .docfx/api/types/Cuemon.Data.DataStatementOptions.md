---
uid: Cuemon.Data.DataStatementOptions
example:
- *content
---

The following example demonstrates how to configure `DataStatementOptions` for text commands and stored procedures with custom timeout and parameters.

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
