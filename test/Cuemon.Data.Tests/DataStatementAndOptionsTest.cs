using System;
using System.Data;
using Codebelt.Extensions.Xunit;
using Microsoft.Data.Sqlite;
using Xunit;

namespace Cuemon.Data;
public class DataStatementAndOptionsTest : Test
{
    public DataStatementAndOptionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void ShouldCaptureConfiguredValues()
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
}
