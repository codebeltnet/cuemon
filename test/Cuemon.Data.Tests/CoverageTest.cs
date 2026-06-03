using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Cuemon.Runtime;
using Microsoft.Data.Sqlite;
using Xunit;

namespace Cuemon.Data
{
    public class CoverageTest : Test
    {
        public CoverageTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void DataTransfer_ShouldExposeRowsColumnsAndTypedValues()
        {
            using var reader = CreateDataTable().CreateDataReader();

            var rows = DataTransfer.GetRows(reader);
            var first = rows[0];
            var second = rows[1];
            var columns = first.Columns;
            var enumerable = (IEnumerable)rows;

            Assert.Equal(2, rows.Count);
            Assert.Equal(new[] { "Id", "Name", "Created", "Notes" }, rows.ColumnNames.ToArray());
            Assert.True(rows.Contains(first));
            Assert.Equal(0, rows.IndexOf(first));
            Assert.True(enumerable.GetEnumerator().MoveNext());

            Assert.Equal(1, first.Number);
            Assert.Equal(4, columns.Count);
            Assert.Equal("Id", columns[0].Name);
            Assert.Equal(typeof(int), columns[0].DataType);
            Assert.Equal("Id", columns[0].ToString());
            Assert.Same(columns[0], columns["Id"]);
            Assert.Null(columns["Missing"]);

            Assert.Equal(1, first[(DataTransferColumn)columns[0]]);
            Assert.Equal("Alice", first["Name"]);
            Assert.Null(first[(DataTransferColumn)null]);
            Assert.Null(first["Missing"]);
            Assert.Null(first[-1]);
            Assert.Equal(1, first.As<int>(0));
            Assert.Equal(1, first.As<int>(columns[0]));
            Assert.Equal(new DateTime(2024, 1, 2, 3, 4, 5, DateTimeKind.Utc), first.As<DateTime>("Created"));
            Assert.Throws<TypeArgumentOutOfRangeException>(() => first.As<Guid>(0));
            Assert.Null(second["Notes"]);

            var rowText = first.ToString();
            TestOutput.WriteLine(rowText);
            Assert.Contains("Id=1 [Int32]", rowText);
            Assert.Contains("Name=Alice [String]", rowText);
        }

        [Fact]
        public void DataTransfer_ShouldValidateReaderArguments()
        {
            Assert.Throws<ArgumentNullException>(() => DataTransfer.GetRows(null));
            Assert.Throws<ArgumentNullException>(() => DataTransfer.GetColumns(null));

            using var closedReader = CreateDataTable().CreateDataReader();
            closedReader.Close();

            Assert.Throws<ArgumentException>(() => DataTransfer.GetRows(closedReader));
            Assert.Throws<ArgumentException>(() => DataTransfer.GetColumns(closedReader));
        }

        [Fact]
        public void DataTransfer_ShouldReturnColumns_WhenReaderHasBeenRead()
        {
            using var reader = CreateDataTable().CreateDataReader();

            Assert.True(reader.Read());

            var columns = DataTransfer.GetColumns(reader);

            Assert.Equal(4, columns.Count);
            Assert.Equal("Created", columns[2].Name);
            Assert.Equal(typeof(DateTime), columns[2].DataType);
        }

        [Fact]
        public void InOperator_ShouldCreateSafeResultFromExpressions()
        {
            var sut = new TestInOperator(() => "@p");

            var result = sut.ToSafeResult(new[] { 4, 9 }, args => string.Join(";", args));
            var fromParams = sut.ToSafeResult(1, 2);
            var parameters = result.ToParametersArray().Cast<SqliteParameter>().ToArray();

            Assert.Equal("@p", sut.ExposedPrefix);
            Assert.Equal(new[] { "@p0", "@p1" }, result.Arguments.ToArray());
            Assert.Equal("@p0;@p1", result.ToString());
            Assert.Equal(2, parameters.Length);
            Assert.Equal("@p0", parameters[0].ParameterName);
            Assert.Equal(4L, Convert.ToInt64(parameters[0].Value));
            Assert.Equal("@p1", parameters[1].ParameterName);
            Assert.Equal(9L, Convert.ToInt64(parameters[1].Value));
            Assert.Equal("@p0,@p1", fromParams.ToString());
            Assert.Equal(parameters.Length, result.Parameters.Count());
            Assert.Throws<ArgumentNullException>(() => sut.ToSafeResult((System.Collections.Generic.IEnumerable<int>)null));
        }

        [Fact]
        public void QueryBuilder_ShouldEncodeFragmentsAndBuildQueryText()
        {
            Assert.Equal("Id,Name", QueryBuilder.EncodeFragment(QueryFormat.Delimited, new[] { "Id", "Name" }));
            Assert.Equal("'Id','Name'", QueryBuilder.EncodeFragment(QueryFormat.DelimitedString, new[] { "Id", "Name" }));
            Assert.Equal("[Id],[Name]", QueryBuilder.EncodeFragment(QueryFormat.DelimitedSquareBracket, new[] { "Id", "Name" }));
            Assert.Equal("Id", QueryBuilder.EncodeFragment(QueryFormat.Delimited, new[] { "Id", "Id" }, true));
            Assert.Throws<ArgumentNullException>(() => QueryBuilder.EncodeFragment(QueryFormat.Delimited, null));
            Assert.Throws<ArgumentException>(() => QueryBuilder.EncodeFragment(QueryFormat.Delimited, Array.Empty<string>()));
            Assert.Throws<System.ComponentModel.InvalidEnumArgumentException>(() => QueryBuilder.EncodeFragment((QueryFormat)999, new[] { "Id" }));

            var defaultBuilder = new DefaultTestQueryBuilder();
            var twoArgumentBuilder = new TwoArgumentTestQueryBuilder("Products");
            var sut = new TestQueryBuilder("Products");
            sut.ReadLimit = 25;
            sut.EnableDirtyReads = true;
            sut.EnableReadLimit = true;
            sut.EnableTableAndColumnEncapsulation = true;
            sut.AppendRaw("SELECT ").AppendFormatted("{0}", "*");

            Assert.Equal(string.Empty, defaultBuilder.GetQuery(QueryType.Select));
            Assert.Equal("Select:Products", twoArgumentBuilder.GetQuery(QueryType.Select));
            Assert.Equal(25, sut.ReadLimit);
            Assert.True(sut.EnableDirtyReads);
            Assert.True(sut.EnableReadLimit);
            Assert.True(sut.EnableTableAndColumnEncapsulation);
            Assert.Equal("SELECT *", sut.ToString());
            Assert.Equal("Select:Products", sut.GetQuery(QueryType.Select));
            Assert.Equal("Update:ArchivedProducts", sut.GetQuery(QueryType.Update, "ArchivedProducts"));
            Assert.Equal("Products", sut.TableName);
            Assert.Single(sut.KeyColumns);
            Assert.Single(sut.Columns);
            Assert.Throws<ArgumentOutOfRangeException>(() => sut.ReadLimit = 0);
            Assert.Equal(0, (int)QueryFormat.Delimited);
            Assert.Equal(4, (int)QueryType.Exists);
        }

        [Fact]
        public void TokenBuilder_ShouldTrackTokensAndQuotedDelimiters()
        {
            var sut = new TokenBuilder(',', '"', 3);

            sut.Append("a,\"b,c\",d,e");

            Assert.True(sut.IsValid);
            Assert.Equal(3, sut.Tokens);
            Assert.Equal(',', sut.Delimiter);
            Assert.Equal('"', sut.Qualifier);
            Assert.Equal("a,\"b,c\",d,", sut.ToString());
        }

        [Fact]
        public void TokenBuilder_ShouldHandleNullAndInvalidStringArguments()
        {
            var sut = new TokenBuilder(",", "\"", 2);

            sut.Append(null).Append("onlyone");

            Assert.False(sut.IsValid);
            Assert.Equal("onlyone", sut.ToString());
            Assert.Throws<FormatException>(() => new TokenBuilder("::", "\"", 1));
            Assert.Throws<FormatException>(() => new TokenBuilder(",", "''", 1));
        }

        [Fact]
        public void DataReader_ShouldExposeIDataReaderMembers()
        {
            var sut = new TestDataReader();

            Assert.True(sut.Read());
            Assert.Equal(1, sut.RowCount);
            Assert.True(sut.Contains("Boolean"));
            Assert.Equal(true, sut["Boolean"]);
            Assert.Equal(true, sut[0]);
            Assert.Equal(12, sut.FieldCount);
            Assert.Equal("Boolean", sut.GetName(0));
            Assert.Equal(string.Empty, sut.GetName(99));
            Assert.Equal(0, sut.GetOrdinal("boolean"));
            Assert.Throws<ArgumentNullException>(() => sut.GetOrdinal(null));
            Assert.Throws<ArgumentOutOfRangeException>(() => sut.GetOrdinal("missing"));
            Assert.True(sut.GetBoolean(0));
            Assert.Equal((byte)8, sut.GetByte(1));
            Assert.Equal('X', sut.GetChar(2));
            Assert.Equal(new DateTime(2024, 5, 6, 7, 8, 9, DateTimeKind.Utc), sut.GetDateTime(3));
            Assert.Equal(10.5m, sut.GetDecimal(4));
            Assert.Equal(12.5d, sut.GetDouble(5));
            Assert.Equal(typeof(Guid), sut.GetFieldType(6));
            Assert.Equal(14.5f, sut.GetFloat(7));
            Assert.Equal(Guid.Parse("11111111-1111-1111-1111-111111111111"), sut.GetGuid(6));
            Assert.Equal((short)16, sut.GetInt16(8));
            Assert.Equal(32, sut.GetInt32(9));
            Assert.Equal(64L, sut.GetInt64(10));
            Assert.Equal("alpha", sut.GetString(11));
            Assert.Equal(true, sut.GetValue(0));
            Assert.False(sut.IsDBNull(0));
            Assert.Equal(0L, sut.GetBytes(0, 0, Array.Empty<byte>(), 0, 0));
            Assert.Equal(0L, ((IDataRecord)sut).GetChars(0, 0, Array.Empty<char>(), 0, 0));
            Assert.Throws<NotSupportedException>(() => ((IDataRecord)sut).GetData(0));
            Assert.Equal(typeof(string).ToString(), ((IDataRecord)sut).GetDataTypeName(0));
            Assert.Equal(0, sut.Depth);
            Assert.Contains("Boolean=True", sut.ToString());
            Assert.Throws<ArgumentNullException>(() => sut.GetValues(null));

            var values = new object[sut.FieldCount];
            Assert.Equal(sut.FieldCount, sut.GetValues(values));
            Assert.Equal("alpha", values[11]);

            var reader = (IDataReader)sut;
            Assert.Equal(-1, reader.RecordsAffected);
            Assert.Null(reader.GetSchemaTable());
            Assert.False(reader.NextResult());
            reader.Close();
            Assert.True(reader.IsClosed);
        }

        [Fact]
        public void DataStatementAndOptions_ShouldCaptureConfiguredValues()
        {
            var timeout = TimeSpan.FromSeconds(12);
            var parameter = new SqliteParameter("@id", 42);
            DataStatement statement = "SELECT * FROM Items";
            var configured = new DataStatement("SELECT * FROM Items WHERE Id = @id", o =>
            {
                o.Type = CommandType.StoredProcedure;
                o.Timeout = timeout;
                o.Parameters = new IDataParameter[] { parameter };
            });
            var statementOptions = new DataStatementOptions();
            var managerOptions = new DataManagerOptions() { ConnectionString = "Data Source=valid" };

            Assert.Equal("SELECT * FROM Items", statement.Text);
            Assert.Equal(CommandType.StoredProcedure, configured.Type);
            Assert.Equal(timeout, configured.Timeout);
            Assert.Single(configured.Parameters);
            Assert.Equal(parameter, configured.Parameters[0]);
            Assert.Equal(CommandType.Text, statementOptions.Type);
            Assert.Equal(DataStatementOptions.DefaultTimeout, statementOptions.Timeout);
            Assert.Empty(statementOptions.Parameters);
            Assert.False(managerOptions.LeaveCommandOpen);
            Assert.False(managerOptions.LeaveConnectionOpen);
            Assert.Equal(CommandBehavior.CloseConnection, managerOptions.PreferredReaderBehavior);
            managerOptions.ValidateOptions();
            statementOptions.ValidateOptions();

            statementOptions.Parameters = null;
            managerOptions.ConnectionString = null;
            Assert.Throws<InvalidOperationException>(() => statementOptions.ValidateOptions());
            Assert.Throws<InvalidOperationException>(() => managerOptions.ValidateOptions());
            Assert.Throws<ArgumentNullException>(() => new DataStatement(null));
        }

        [Fact]
        public void XmlAndDelimitedReadersAndExceptions_ShouldCoverPublicBehavior()
        {
            using (var dsv = new DsvDataReader(new StreamReader(new MemoryStream(System.Text.Encoding.UTF8.GetBytes("Id;Id\n1;2"))), setup: o => o.Delimiter = ";"))
            {
                Assert.True(dsv.Read());
                Assert.Equal(1, dsv.FieldCount);
                Assert.Equal(2, dsv.GetInt32(0));
            }

            using (var xml = new Xml.XmlDataReader(System.Xml.XmlReader.Create(new StringReader("<root><item>1</item><item>2</item></root>"))))
            {
                Assert.True(xml.Read());
                Assert.Equal(1, xml.Depth);
                Assert.Equal(1, xml.GetInt32(0));
                Assert.True(xml.Read());
                Assert.Equal(2, xml.GetInt32(0));
                Assert.False(xml.Read());
            }

            var exception = new UniqueIndexViolationException("duplicate", new InvalidOperationException("inner"));
            Assert.Equal("duplicate", exception.Message);
            Assert.IsType<InvalidOperationException>(exception.InnerException);
        }

        [Fact]
        public async Task DataManagerAsyncAndWatcherDependency_ShouldReactToChanges()
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
            var modified = await dependencyChanged.Task.WaitAsync(TimeSpan.FromSeconds(5));
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

        private static DataTable CreateDataTable()
        {
            var table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Created", typeof(DateTime));
            table.Columns.Add("Notes", typeof(string));
            table.Rows.Add(1, "Alice", new DateTime(2024, 1, 2, 3, 4, 5, DateTimeKind.Utc), "First");
            table.Rows.Add(2, "Bob", new DateTime(2024, 2, 3, 4, 5, 6, DateTimeKind.Utc), DBNull.Value);
            return table;
        }

        private sealed class TestInOperator : InOperator<int>
        {
            public TestInOperator(Func<string> prefixFactory) : base(prefixFactory)
            {
            }

            public string ExposedPrefix => ParameterPrefix;

            protected override IDbDataParameter ParametersSelector(int expression, int index)
            {
                return new SqliteParameter(string.Concat(ParameterPrefix, index), expression);
            }
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

        private sealed class DefaultTestQueryBuilder : QueryBuilder
        {
            public override string GetQuery(QueryType queryType, string tableName)
            {
                return ToString();
            }
        }

        private sealed class TwoArgumentTestQueryBuilder : QueryBuilder
        {
            public TwoArgumentTestQueryBuilder(string tableName) : base(tableName, new System.Collections.Generic.Dictionary<string, string>()
            {
                { "Id", "Id" }
            })
            {
            }

            public override string GetQuery(QueryType queryType, string tableName)
            {
                return $"{queryType}:{tableName ?? TableName}";
            }
        }

        private sealed class TestQueryBuilder : QueryBuilder
        {
            public TestQueryBuilder(string tableName) : base(tableName, new System.Collections.Generic.Dictionary<string, string>()
            {
                { "Id", "Id" }
            }, new System.Collections.Generic.Dictionary<string, string>()
            {
                { "Name", "Name" }
            })
            {
            }

            public TestQueryBuilder AppendRaw(string queryFragment)
            {
                Append(queryFragment);
                return this;
            }

            public TestQueryBuilder AppendFormatted(string queryFragment, params object[] args)
            {
                Append(queryFragment, args);
                return this;
            }

            public override string GetQuery(QueryType queryType, string tableName)
            {
                return $"{queryType}:{tableName ?? TableName}";
            }
        }

        private sealed class TestDataReader : DataReader<IOrderedDictionary>
        {
            private readonly IOrderedDictionary[] _rows;
            private int _position = -1;

            protected override void OnDisposeManagedResources()
            {
            }

            public TestDataReader()
            {
                _rows = new IOrderedDictionary[]
                {
                    new OrderedDictionary(StringComparer.OrdinalIgnoreCase)
                    {
                        { "Boolean", true },
                        { "Byte", (byte)8 },
                        { "Char", 'X' },
                        { "DateTime", new DateTime(2024, 5, 6, 7, 8, 9, DateTimeKind.Utc) },
                        { "Decimal", 10.5m },
                        { "Double", 12.5d },
                        { "Guid", Guid.Parse("11111111-1111-1111-1111-111111111111") },
                        { "Single", 14.5f },
                        { "Int16", (short)16 },
                        { "Int32", 32 },
                        { "Int64", 64L },
                        { "String", "alpha" }
                    }
                };
            }

            public override int RowCount { get; protected set; }

            protected override IOrderedDictionary NullRead => null;

            protected override IOrderedDictionary ReadNext(IOrderedDictionary columns)
            {
                return columns;
            }

            public override bool Read()
            {
                _position++;
                if (_position >= _rows.Length) { return false; }
                SetFields(_rows[_position]);
                RowCount++;
                return true;
            }
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
