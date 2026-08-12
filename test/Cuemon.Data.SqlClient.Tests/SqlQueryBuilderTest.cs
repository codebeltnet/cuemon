using System.Collections.Generic;
using System.Linq;
using Cuemon.Data.SqlClient.Assets;
using Cuemon.Extensions.Data;
using Codebelt.Extensions.Xunit.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Cuemon.Data.SqlClient;
public class SqlQueryBuilderTest : HostTest<UserSecretsHostFixture>
{
    private readonly SqlDataManager _manager;

    public SqlQueryBuilderTest(UserSecretsHostFixture hostFixture, ITestOutputHelper output) : base(hostFixture, output)
    {
        _manager = hostFixture.Host.Services.GetRequiredService<SqlDataManager>();
    }

    [Fact]
    public void BuildSelectQuery_ShouldSelectStateProvince()
    {
        var builder = new SqlQueryBuilder("[Person].[StateProvince]", new Dictionary<string, string>(), new Dictionary<string, string>() { { "name", null } })
        {
            EnableDirtyReads = true,
            EnableReadLimit = true,
            ReadLimit = 10,
            EnableTableAndColumnEncapsulation = true
        };

        Assert.True(builder.EnableDirtyReads);
        Assert.True(builder.EnableReadLimit);
        Assert.True(builder.EnableTableAndColumnEncapsulation);
        Assert.Equal(10, builder.ReadLimit);

        var sql = builder.GetQuery(QueryType.Select);

        Assert.Contains("WITH(NOLOCK)", sql);
        Assert.Contains("TOP 10", sql);

        using (var reader = _manager.ExecuteReader(new DataStatement(sql)))
        {
            var rows = reader.ToRows();
            Assert.Equal(10, rows.Count);

            TestOutput.WriteLine(DelimitedString.Create(rows.Select(dtr => dtr["name"])));

        }
    }

    public override void ConfigureServices(IServiceCollection services)
    {
        var cnn = Configuration.GetConnectionString("AdventureWorks");
        services.AddSingleton(new SqlDataManager(o => o.ConnectionString = cnn));
    }
}
