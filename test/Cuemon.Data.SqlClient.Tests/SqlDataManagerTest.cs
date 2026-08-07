using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using Cuemon.Collections.Generic;
using Cuemon.Data.SqlClient.Assets;
using Cuemon.Extensions;
using Cuemon.Extensions.Data;
using Codebelt.Extensions.Xunit.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Cuemon.Data.SqlClient;
public class SqlDataManagerTest : HostTest<UserSecretsHostFixture>
{
    private readonly SqlDataManager _manager;

    public SqlDataManagerTest(UserSecretsHostFixture hostFixture, ITestOutputHelper output) : base(hostFixture, output)
    {
        _manager = hostFixture.Host.Services.GetRequiredService<SqlDataManager>();
    }

    [Fact]
    public void ExecuteReader_ShouldReadAllProducts()
    {
        using (var reader = _manager.ExecuteReader(new DataStatement("SELECT * FROM [Production].[Product]")))
        {
            var rows = reader.ToRows();
            var columns = rows.ColumnNames.ToList();
            Assert.Equal(504, rows.Count);
            Assert.Equal(25, columns.Count);
            Assert.InRange(rows.Select(row => row["ModifiedDate"].As<DateTime>().Year).Distinct().Single(), 2013, 2015);
        }
    }

    [Fact]
    public void ExecuteScalarAsInt32_ShouldInsertNewRow()
    {
        var existsBefore = _manager.ExecuteExists(new DataStatement("SELECT * FROM [ErrorLog]"));
        var current = _manager.ExecuteScalarAs<int>(new DataStatement("SELECT COUNT(*) FROM [ErrorLog]"));
        var affected = _manager.Execute(new DataStatement(@"INSERT INTO [ErrorLog] ([ErrorTime]
,[UserName]
,[ErrorNumber]
,[ErrorSeverity]
,[ErrorState]
,[ErrorProcedure]
,[ErrorLine]
,[ErrorMessage])
VALUES
(@utcNow, 
@userName,
@errNo,
@errSeverity,
@errState,
@errProcedure,
@errorLine,
@errorMessage
)", o => o.Parameters = Arguments.ToArrayOf(
            new SqlParameter("@utcNow", DateTime.UtcNow),
            new SqlParameter("@userName", "MMORT"),
            new SqlParameter("@errNo", 42),
            new SqlParameter("@errSeverity", 1),
            new SqlParameter("@errState", 5),
            new SqlParameter("@errProcedure", "Do not try this at home."),
            new SqlParameter("@errorLine", 215),
            new SqlParameter("@errorMessage", "Catastrophic failure."))));
        var after = _manager.ExecuteScalarAs<int>(new DataStatement("SELECT COUNT(*) FROM [ErrorLog]"));
        var existsAfter = _manager.ExecuteExists(new DataStatement("SELECT * FROM [ErrorLog]"));

        Assert.False(existsBefore);
        Assert.Equal(0, current);
        Assert.Equal(1, affected);
        Assert.Equal(1, after);
        Assert.True(existsAfter);
    }


    [Fact]
    public void Execute_ShouldUpdateRows()
    {
        DataTransferRowCollection before;
        using (var reader = _manager.ExecuteReader(new DataStatement("SELECT * FROM [HumanResources].[Department]")))
        {
            before = reader.ToRows();
        }

        var affected = _manager.Execute(new DataStatement("UPDATE [HumanResources].[Department] SET [Name] = [Name] + ' XXX'"));

        DataTransferRowCollection after;
        using (var reader = _manager.ExecuteReader(new DataStatement("SELECT * FROM [HumanResources].[Department]")))
        {
            after = reader.ToRows();
        }

        Assert.Equal(16, before.Count);
        Assert.Equal(16, affected);
        Assert.Equal(16, after.Count);

        for (var i = 0; i < before.Count; i++)
        {
            Assert.StartsWith(before[i]["Name"].As<string>(), after[i]["Name"].As<string>());
            Assert.NotEqual(before[i]["Name"].As<string>(), after[i]["Name"].As<string>());
            Assert.EndsWith("XXX", after[i]["Name"].As<string>());
        }
    }

    [Fact]
    public void Execute_ShouldDeleteRows()
    {
        var before = _manager.ExecuteScalarAs<int>(new DataStatement("SELECT COUNT(*) FROM [HumanResources].[EmployeeDepartmentHistory]"));
        var affected = _manager.Execute(new DataStatement("DELETE [HumanResources].[EmployeeDepartmentHistory] WHERE BusinessEntityID >= 270"));
        var after = _manager.ExecuteScalarAs<int>(new DataStatement("SELECT COUNT(*) FROM [HumanResources].[EmployeeDepartmentHistory]"));
        Assert.Equal(296, before);
        Assert.Equal(21, affected);
        Assert.Equal(275, after);
    }

    public override void ConfigureServices(IServiceCollection services)
    {
        var cnn = Configuration.GetConnectionString("AdventureWorks");
        services.AddSingleton(new SqlDataManager(o => o.ConnectionString = cnn));
    }
}
