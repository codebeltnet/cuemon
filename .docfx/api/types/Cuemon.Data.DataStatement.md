---
uid: Cuemon.Data.DataStatement
example:
- *content
---

The following example demonstrates how to create `DataStatement` instances for text queries, stored procedures, and parameterized commands.

```csharp
using System;
using System.Data;
using System.Linq;
using Cuemon.Data;

namespace MyApp.DataAccess
{
    public sealed class DataStatementExample
    {
        public void Demonstrate()
        {
            DataStatement textStatement = "SELECT * FROM Product";
            Console.WriteLine(textStatement.Text);
            Console.WriteLine(textStatement.Type);

            var storedProcedure = new DataStatement("dbo.GetOrdersByDate", options =>
            {
                options.Type = CommandType.StoredProcedure;
                options.Timeout = TimeSpan.FromSeconds(120);
            });
            Console.WriteLine($"{storedProcedure.Text} ({storedProcedure.Type})");

            var parameterized = new DataStatement("UPDATE Inventory SET Quantity = Quantity - @qty WHERE ProductId = @id", options =>
            {
                options.Parameters = new IDataParameter[]
                {
                    new DemoParameter("@qty", 5),
                    new DemoParameter("@id", 1001)
                };
            });

            Console.WriteLine($"Parameter count: {parameterized.Parameters.Length}");
            Console.WriteLine(string.Join(", ", parameterized.Parameters.Select(parameter => parameter.ParameterName)));
        }

        private sealed class DemoParameter : IDataParameter
        {
            public DemoParameter(string name, object value)
            {
                ParameterName = name;
                Value = value;
            }

            public DbType DbType { get; set; }

            public ParameterDirection Direction { get; set; } = ParameterDirection.Input;

            public bool IsNullable => true;

            public string ParameterName { get; set; }

            public string SourceColumn { get; set; } = string.Empty;

            public DataRowVersion SourceVersion { get; set; } = DataRowVersion.Current;

            public object Value { get; set; }
        }
    }
}
```
