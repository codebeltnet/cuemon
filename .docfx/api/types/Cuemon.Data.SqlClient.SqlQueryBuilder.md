---
uid: Cuemon.Data.SqlClient.SqlQueryBuilder
example:
- *content
---

`SqlQueryBuilder` generates SQL Server-specific SELECT, INSERT, UPDATE, DELETE, and EXISTS queries from key and column mapping dictionaries. This example configures multiple `SqlQueryBuilder` instances for an `Employees` table with key column `EmployeeId` and optional columns `FirstName`, `LastName`, and `Email`. Settings include `EnableTableAndColumnEncapsulation` for bracket-delimited identifiers, `EnableDirtyReads` for `WITH(NOLOCK)`, and `EnableReadLimit` with `ReadLimit = 50` for `TOP 50`. Console output shows the generated SQL for each query type, such as `SELECT TOP 50 [EmployeeId],[FirstName],[LastName],[Email] FROM [Employees] WITH(NOLOCK) WHERE [EmployeeId]=@EmployeeId`.

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
