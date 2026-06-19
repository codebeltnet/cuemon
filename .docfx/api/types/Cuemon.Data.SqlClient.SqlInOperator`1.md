---
uid: Cuemon.Data.SqlClient.SqlInOperator`1
example:
- *content
---

The following example demonstrates how to use <see cref="SqlInOperator{T}"/> to safely generate parameterized SQL IN clauses that are protected against SQL injection.

```csharp
using System;
using System.Data;
using Cuemon.Data;
using Cuemon.Data.SqlClient;
using Microsoft.Data.SqlClient;

namespace MyApp.Examples
{
    public sealed class SqlInOperatorExample
    {
        public void Demonstrate()
        {
            var inOperator = new SqlInOperator<string>(() => "@color");
            InOperatorResult result = inOperator.ToSafeResult("Red", "Green", "Blue");

            var commandText = $"SELECT * FROM Products WHERE Color IN ({result})";
            using var command = new SqlCommand(commandText);

            foreach (IDataParameter dbParameter in result.ToParametersArray())
            {
                command.Parameters.Add((SqlParameter)dbParameter);
                Console.WriteLine($"{dbParameter.ParameterName} = {dbParameter.Value}");
            }

            Console.WriteLine(command.CommandText);
            Console.WriteLine(string.Join(", ", result.Arguments));
        }
    }
}
```
