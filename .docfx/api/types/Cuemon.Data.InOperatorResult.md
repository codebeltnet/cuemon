---
uid: Cuemon.Data.InOperatorResult
example:
- *content
---

The following example demonstrates how to create an `InOperatorResult` by using a custom `InOperator<T>` subclass and then access its arguments, parameters, and string representation.

```csharp
using System;
using System.Data;
using System.Linq;
using Cuemon.Data;

namespace MyApp.Examples;

public class InOperatorResultExample
{
    public static void Main()
    {
        var safeOperator = new IntInOperator();
        InOperatorResult result = safeOperator.ToSafeResult(10, 20, 30);

        Console.WriteLine("Arguments CSV: {0}", result);
        Console.WriteLine("Parameter count: {0}", result.Parameters.Count());
        Console.WriteLine("First parameter value: {0}", result.Parameters.First().Value);

        // Output:
        // Arguments CSV: @p0, @p1, @p2
        // Parameter count: 3
        // First parameter value: 10
    }

    private sealed class IntInOperator : InOperator<int>
    {
        public IntInOperator() : base(() => "@p") { }

        protected override IDbDataParameter ParametersSelector(int expression, int index)
        {
            return new SimpleParameter(string.Concat(ParameterPrefix, index), expression);
        }

        private sealed class SimpleParameter : IDbDataParameter
        {
            public SimpleParameter(string name, object value)
            {
                ParameterName = name;
                Value = value;
            }

            public DbType DbType { get; set; } = DbType.Int32;
            public ParameterDirection Direction { get; set; } = ParameterDirection.Input;
            public bool IsNullable => false;
            public string ParameterName { get; set; }
            public int Size { get; set; }
            public string SourceColumn { get; set; } = string.Empty;
            public bool SourceColumnNullMapping { get; set; }
            public DataRowVersion SourceVersion { get; set; } = DataRowVersion.Current;
            public object Value { get; set; }
            public byte Precision { get; set; }
            public byte Scale { get; set; }
        }
    }
}

```
