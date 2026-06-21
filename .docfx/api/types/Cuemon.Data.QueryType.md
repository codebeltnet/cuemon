---
uid: Cuemon.Data.QueryType
example:
- *content
---

The following example demonstrates how to use the `QueryType` enumeration to identify the type of a data operation.

```csharp
using System;
using Cuemon.Data;

namespace MyApp.Examples;

public class QueryTypeExample
{
    public static void Main()
    {
        var operation = QueryType.Select;
        Console.WriteLine("Operation: {0} (value: {1})", operation, (int)operation);

        operation = QueryType.Insert;
        Console.WriteLine("Operation: {0} (value: {1})", operation, (int)operation);

        // Use in a switch expression
        string label = operation switch
        {
            QueryType.Select => "Read",
            QueryType.Insert => "Create",
            QueryType.Update => "Update",
            QueryType.Delete => "Delete",
            QueryType.Exists => "Check existence",
            _ => "Unknown"
        };
        Console.WriteLine("Label: {0}", label);

        // Output:
        // Operation: Select (value: 0)
        // Operation: Insert (value: 2)
        // Label: Create

}
}

```
