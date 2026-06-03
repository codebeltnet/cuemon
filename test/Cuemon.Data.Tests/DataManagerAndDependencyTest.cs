using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Cuemon.Runtime;
using Microsoft.Data.Sqlite;
using Xunit;

namespace Cuemon.Data
{
    public class DataManagerAndDependencyTest : Test
    {
        public DataManagerAndDependencyTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task AsyncAndWatcherDependency_ShouldReactToChanges()
        {
            var manager = CreateManager();
            var affected = await manager.ExecuteAsync(new DataStatement("UPDATE Product SET DiscontinuedDate = @expired", o =>
            {
                o.Parameters = new IDataParameter[] { new SqliteParameter("@expired", DateTime.UtcNow) };
            }));
            var scalar = await manager.ExecuteScalarAsync(new DataStatement("SELECT Name FROM Product WHERE ProductID = 1"));

            Assert.Equal(504, affected);
            Assert.Equal("Adjustable Race", scalar);

            var connectionString = $"Data Source=watcher-{Guid.NewGuid():N};Mode=Memory;Cache=Shared";
            using var rootConnection = new SqliteConnection(connectionString);
            rootConnection.Open();
            using (var command = rootConnection.CreateCommand())
            {
                command.CommandText = "CREATE TABLE Item (Id INTEGER PRIMARY KEY, Name TEXT NOT NULL); INSERT INTO Item VALUES (1, 'Alpha');";
                command.ExecuteNonQuery();
            }

            using var watcherConnection = new SqliteConnection(connectionString);
            var watcher = new TestDatabaseWatcher(watcherConnection, CreateReader, o =>
            {
                o.DueTime = Timeout.InfiniteTimeSpan;
                o.Period = Timeout.InfiniteTimeSpan;
            });
            var changedSignals = 0;
            watcher.Changed += (_, _) => changedSignals++;

            await watcher.SignalAsync();
            Assert.NotNull(watcher.Checksum);
            Assert.Equal(0, changedSignals);
            Assert.Equal(ConnectionState.Closed, watcherConnection.State);

            using (var command = rootConnection.CreateCommand())
            {
                command.CommandText = "UPDATE Item SET Name = 'Beta' WHERE Id = 1;";
                command.ExecuteNonQuery();
            }

            await watcher.SignalAsync();
            Assert.Equal(1, changedSignals);
            Assert.Equal(ConnectionState.Closed, watcherConnection.State);

            var dependencyChanged = new TaskCompletionSource<DateTime?>(TaskCreationOptions.RunContinuationsAsynchronously);
            var dependency = new DatabaseDependency(new Lazy<DatabaseWatcher>(() => watcher));
            dependency.DependencyChanged += (_, e) => dependencyChanged.TrySetResult(e.UtcLastModified);
            await dependency.StartAsync();

            using (var command = rootConnection.CreateCommand())
            {
                command.CommandText = "INSERT INTO Item VALUES (2, 'Gamma');";
                command.ExecuteNonQuery();
            }

            watcher.ChangeSignaling(TimeSpan.Zero, Timeout.InfiniteTimeSpan);
            var modified = await WaitOrThrowAsync(dependencyChanged.Task, TimeSpan.FromSeconds(5));
            Assert.True(dependency.HasChanged);
            Assert.Equal(modified, dependency.UtcLastModified);
            Assert.Throws<ArgumentNullException>(() => new DatabaseWatcher(null, CreateReader));
            Assert.Throws<ArgumentNullException>(() => new DatabaseWatcher(watcherConnection, null));
            Assert.Throws<ArgumentNullException>(() => new DatabaseDependency((Lazy<DatabaseWatcher>)null));

            static IDataReader CreateReader(IDbConnection connection)
            {
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Id, Name FROM Item ORDER BY Id";
                return command.ExecuteReader();
            }
        }

        private static async Task<T> WaitOrThrowAsync<T>(Task<T> task, TimeSpan timeout)
        {
            var timeoutTask = Task.Delay(timeout);
            if (await Task.WhenAny(task, timeoutTask) != task) { throw new TimeoutException(); }
            return await task;
        }

        private static Assets.FakeDataManager CreateManager()
        {
            var manager = new Assets.FakeDataManager(o =>
            {
                o.LeaveConnectionOpen = true;
                o.LeaveCommandOpen = true;
                o.ConnectionString = $"Data Source=coverage-{Guid.NewGuid():N};Mode=Memory;Cache=Shared";
            });
            Assets.SqliteDatabase.Create(manager, null);
            return manager;
        }

        private sealed class TestDatabaseWatcher : DatabaseWatcher
        {
            public TestDatabaseWatcher(IDbConnection connection, Func<IDbConnection, IDataReader> readerFactory, Action<WatcherOptions> setup = null) : base(connection, readerFactory, setup)
            {
            }

            public Task SignalAsync()
            {
                return HandleSignalingAsync();
            }
        }
    }
}
