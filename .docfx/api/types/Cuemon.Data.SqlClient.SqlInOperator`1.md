---
uid: Cuemon.Data.SqlClient.SqlInOperator`1
example:
- *content
---

The following example demonstrates how to use <see cref="SqlInOperator{T}"/> to safely generate parameterized SQL IN clauses that are protected against SQL injection.

```csharp
// Program.cs
using System;
using System.Data;
using Cuemon.Data;
using Cuemon.Data.SqlClient;
using Microsoft.Data.SqlClient;

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
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="bb9004b3debb41d00082b474"><a href="~/run-actions/bb9004b3debb41d00082b474/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/bb9004b3debb41d00082b474/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
