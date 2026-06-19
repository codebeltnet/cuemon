---
uid: Cuemon.Data.DatabaseDependency
example:
- *content
---

The following example demonstrates how to use <see cref="DatabaseDependency"/> to monitor a relational data source for changes and notify dependent objects.

```csharp
using System;
using System.Data;
using System.Threading.Tasks;
using Cuemon.Data;
using Cuemon.Runtime;

namespace MyApp.Examples;

public class DatabaseDependencyExample
{
    public async Task DemonstrateAsync()
    {
        var lazyWatcher = new Lazy<DatabaseWatcher>(() =>
        {
            var connection = new StubConnection();
            return new DatabaseWatcher(
                connection,
                conn =>
                {
                    var command = conn.CreateCommand();
                    command.CommandText = "SELECT COUNT(*) FROM Products";
                    return command.ExecuteReader();
                });
        });

        var dependency = new DatabaseDependency(lazyWatcher, breakTieOnChanged: true);

        dependency.DependencyChanged += (sender, args) =>
        {
            Console.WriteLine("Database data has changed!");
        };

        await dependency.StartAsync();
    }

    private sealed class StubConnection : IDbConnection
    {
        public string ConnectionString { get; set; }
        public int ConnectionTimeout => 30;
        public string Database => "MyDb";
        public ConnectionState State => ConnectionState.Closed;
        public IDbTransaction BeginTransaction() => null;
        public IDbTransaction BeginTransaction(IsolationLevel il) => null;
        public void ChangeDatabase(string databaseName) { }
        public void Close() { }
        public IDbCommand CreateCommand() => new StubCommand();
        public void Open() { }
        public void Dispose() { }
    }

    private sealed class StubCommand : IDbCommand
    {
        public string CommandText { get; set; }
        public int CommandTimeout { get; set; }
        public CommandType CommandType { get; set; }
        public IDbConnection Connection { get; set; }
        public IDataParameterCollection Parameters => null;
        public IDbTransaction Transaction { get; set; }
        public UpdateRowSource UpdatedRowSource { get; set; }
        public bool DesignTimeVisible { get; set; }
        public void Cancel() { }
        public IDbDataParameter CreateParameter() => null;
        public int ExecuteNonQuery() => 0;
        public IDataReader ExecuteReader() => null;
        public IDataReader ExecuteReader(CommandBehavior behavior) => null;
        public object ExecuteScalar() => 0;
        public void Prepare() { }
        public void Dispose() { }
    }
}
```
