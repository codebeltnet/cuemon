---
uid: Cuemon.Data.SqlClient.SqlDataManager
example:
- *content
---

The following example demonstrates how to use `SqlDataManager` to execute commands against Microsoft SQL Server.

```csharp
using System;
using Cuemon.Collections.Generic;
using Cuemon.Data;
using Cuemon.Data.SqlClient;
using Microsoft.Data.SqlClient;

namespace MyApp.Examples;

public class Example
{
    public void Run()
    {

        // Configure the SQL data manager with a connection string
        var manager = new SqlDataManager(o =>
        {
            o.ConnectionString = "Server=.;Database=AdventureWorks;Trusted_Connection=True;TrustServerCertificate=True;";
        });

        // Execute a scalar query (implicit string to DataStatement conversion)
        var productCount = manager.ExecuteScalar("SELECT COUNT(*) FROM Production.Product");
        Console.WriteLine($"Product count: {productCount}");

        // Execute a reader command with parameters
using var reader = manager.ExecuteReader(new DataStatement(
            "SELECT ProductID, Name, ListPrice FROM Production.Product WHERE ListPrice > @minPrice",
            o => o.Parameters = Arguments.ToArrayOf(new SqlParameter("@minPrice", 1000m))));

        while (reader.Read())
        {
            Console.WriteLine($"  #{reader.GetInt32(0)}: {reader.GetString(1)} - ${reader.GetDecimal(2):F2}");

        // Execute a non-query (INSERT)
        var affected = manager.Execute(new DataStatement(
            "UPDATE Production.Product SET ListPrice = ListPrice * 1.05 WHERE ProductID = @id",
            o => o.Parameters = Arguments.ToArrayOf(new SqlParameter("@id", 999))));

        Console.WriteLine($"Rows affected: {affected}");

}}
}

```
