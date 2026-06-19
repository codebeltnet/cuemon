---
uid: Cuemon.Data.SqlClient.SqlQueryBuilder
example:
- *content
---

The following example demonstrates how to use `SqlQueryBuilder` to generate SELECT, INSERT, UPDATE, DELETE, and EXISTS queries for SQL Server with table and column encapsulation, dirty reads, and read limits.

```csharp
using System;
using System.Collections.Generic;
using Cuemon.Data;
using Cuemon.Data.SqlClient;

namespace MyApp.Data
{
    public class SqlQueryBuilderExample
    {
        public void Demonstrate()
        {
            // Build a SELECT query with key columns and optional columns
            var selectBuilder = new SqlQueryBuilder(
                "Employees",
                new Dictionary<string, string> { { "EmployeeId", "@EmployeeId" } },
                new Dictionary<string, string>
                {
                    { "FirstName", "@FirstName" },
                    { "LastName", "@LastName" },
                    { "Email", "@Email" }
                })
            {
                EnableTableAndColumnEncapsulation = true,
                EnableDirtyReads = true,
                EnableReadLimit = true,
                ReadLimit = 50
            };

            string selectQuery = selectBuilder.GetQuery(QueryType.Select);
            Console.WriteLine(selectQuery);
            // SELECT TOP 50 [EmployeeId],[FirstName],[LastName],[Email] FROM [Employees] WITH(NOLOCK) WHERE [EmployeeId]=@EmployeeId

            // Build an INSERT query
            var insertBuilder = new SqlQueryBuilder(
                "Employees",
                new Dictionary<string, string>(),
                new Dictionary<string, string>
                {
                    { "FirstName", "@FirstName" },
                    { "LastName", "@LastName" },
                    { "Email", "@Email" }
                })
            {
                EnableTableAndColumnEncapsulation = true
            };

            string insertQuery = insertBuilder.GetQuery(QueryType.Insert);
            Console.WriteLine(insertQuery);
            // INSERT INTO [Employees] ([FirstName],[LastName],[Email]) VALUES (@FirstName,@LastName,@Email)

            // Build an UPDATE query
            var updateBuilder = new SqlQueryBuilder(
                "Employees",
                new Dictionary<string, string> { { "EmployeeId", "@EmployeeId" } },
                new Dictionary<string, string>
                {
                    { "FirstName", "@FirstName" },
                    { "LastName", "@LastName" }
                })
            {
                EnableTableAndColumnEncapsulation = true
            };

            string updateQuery = updateBuilder.GetQuery(QueryType.Update);
            Console.WriteLine(updateQuery);
            // UPDATE [Employees] SET [FirstName]=@FirstName,[LastName]=@LastName WHERE [EmployeeId]=@EmployeeId

            // Build a DELETE query
            var deleteBuilder = new SqlQueryBuilder(
                "Employees",
                new Dictionary<string, string> { { "EmployeeId", "@EmployeeId" } },
                new Dictionary<string, string>())
            {
                EnableTableAndColumnEncapsulation = true
            };

            string deleteQuery = deleteBuilder.GetQuery(QueryType.Delete);
            Console.WriteLine(deleteQuery);
            // DELETE FROM [Employees] WHERE [EmployeeId]=@EmployeeId

            // Build an EXISTS query
            string existsQuery = deleteBuilder.GetQuery(QueryType.Exists);
            Console.WriteLine(existsQuery);
            // SELECT 1 FROM [Employees] WHERE [EmployeeId]=@EmployeeId

}}
}

```
