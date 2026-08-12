using System.Linq;
using Cuemon.Collections.Generic;
using Cuemon.Data.SqlClient.Assets;
using Cuemon.Extensions.Data;
using Codebelt.Extensions.Xunit.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Cuemon.Data.SqlClient;
public class SqlInOperatorTest : HostTest<UserSecretsHostFixture>
{
    private readonly SqlDataManager _manager;

    public SqlInOperatorTest(UserSecretsHostFixture hostFixture, ITestOutputHelper output) : base(hostFixture, output)
    {
        _manager = hostFixture.Host.Services.GetRequiredService<SqlDataManager>();
    }

    [Fact]
    public void ShouldSafeGuardInOperation()
    {
        var io = new SqlInOperator<string>();
        var sr = io.ToSafeResult(Arguments.ToEnumerableOf("A", "B", "C"));
        using (var reader = _manager.ExecuteReader(new DataStatement($"SELECT * FROM [Production].[ProductInventory] WHERE Shelf IN ({sr})", o => o.Parameters = sr.ToParametersArray())))
        {
            var rows = reader.ToRows();
            Assert.Equal(172, rows.Count);
            Assert.Equal(sr.Arguments, sr.Parameters.Select(dbp => dbp.ParameterName));
        }
    }

    public override void ConfigureServices(IServiceCollection services)
    {
        var cnn = Configuration.GetConnectionString("AdventureWorks");
        services.AddSingleton(new SqlDataManager(o => o.ConnectionString = cnn));
    }
}
