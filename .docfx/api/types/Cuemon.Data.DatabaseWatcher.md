---
uid: Cuemon.Data.DatabaseWatcher
example:
- *content
---

The following example demonstrates how to use `DatabaseWatcher` to monitor a database for data changes by comparing checksums over time.

```csharp
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Cuemon.Data;
using Cuemon.Runtime;

namespace MyApp.Data
{
    public sealed class DatabaseWatcherExample
    {
        public async Task DemonstrateAsync()
        {
            var table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("Name", typeof(string));
            table.Rows.Add(1, "Alpha");

            using var connection = new InMemoryConnection();
            var watcher = new SampleDatabaseWatcher(connection, _ => table.CreateDataReader(), options =>
            {
                options.DueTime = Timeout.InfiniteTimeSpan;
                options.Period = Timeout.InfiniteTimeSpan;
            });

            var changedSignals = 0;
            watcher.Changed += (_, args) =>
            {
                changedSignals++;
                Console.WriteLine($"Detected a change at {args.UtcLastModified:O}.");
            };

            await watcher.SignalAsync();
            Console.WriteLine($"Initial checksum: {watcher.Checksum}");

            table.Rows[0]["Name"] = "Beta";

            await watcher.SignalAsync();
            Console.WriteLine($"Signals raised: {changedSignals}");
            Console.WriteLine($"Updated checksum: {watcher.Checksum}");
        }

        private sealed class SampleDatabaseWatcher : DatabaseWatcher
        {
            public SampleDatabaseWatcher(IDbConnection connection, Func<IDbConnection, IDataReader> readerFactory, Action<WatcherOptions> setup = null)
                : base(connection, readerFactory, setup)
            {
            }

            public Task SignalAsync()
            {
                return HandleSignalingAsync();
            }
        }

        private sealed class InMemoryConnection : IDbConnection
        {
            public string ConnectionString { get; set; } = string.Empty;

            public int ConnectionTimeout => 0;

            public string Database => "Sample";

            public ConnectionState State { get; private set; } = ConnectionState.Closed;

            public IDbTransaction BeginTransaction()
            {
                throw new NotSupportedException();
            }

            public IDbTransaction BeginTransaction(IsolationLevel il)
            {
                throw new NotSupportedException();
            }

            public void ChangeDatabase(string databaseName)
            {
                throw new NotSupportedException();
            }

            public void Close()
            {
                State = ConnectionState.Closed;
            }

            public IDbCommand CreateCommand()
            {
                throw new NotSupportedException();
            }

            public void Open()
            {
                State = ConnectionState.Open;
            }

            public void Dispose()
            {
                Close();
            }
        }
    }
}
```
