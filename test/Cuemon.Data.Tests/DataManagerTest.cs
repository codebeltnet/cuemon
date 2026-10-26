using System;
using System.Linq;
using System.Threading.Tasks;
using Cuemon.Collections.Generic;
using Cuemon.Data.Assets;
using Cuemon.Extensions;
using Cuemon.Extensions.Data;
using Codebelt.Extensions.Xunit.Hosting;
using Cuemon.Resilience;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Cuemon.Data;
public class DataManagerTest : HostTest<ManagedHostFixture>
{
    private readonly DataManager _manager;

    public DataManagerTest(ManagedHostFixture hostFixture, ITestOutputHelper output) : base(hostFixture, output)
    {
        _manager = hostFixture.Host.Services.GetRequiredService<FakeDataManager>();
    }

    [Fact]
    public void Execute_ShouldReturnNumberOfRowsAffectedByNonQuery()
    {
        var sut = TransientOperation.WithFunc(() => _manager.Execute(new DataStatement("UPDATE Product SET DiscontinuedDate = @expired", o =>
        {
            o.Timeout = TimeSpan.FromSeconds(2);
            o.Parameters = Arguments.ToArrayOf(new SqliteParameter("@expired", DateTime.UtcNow));
        })), o =>
        {
            o.RetryAttempts = 2;
            o.DetectionStrategy = ex => ex is SqliteException;
        });
        Assert.Equal(504, sut);
    }

    [Fact]
    public void Clone_ShouldCloneCurrentDataManager()
    {
        var sut = _manager.Clone();

        Assert.NotNull(sut);
        Assert.Equal(_manager.Options.LeaveConnectionOpen, sut.Options.LeaveConnectionOpen);
        Assert.Equal(_manager.Options.ConnectionString, sut.Options.ConnectionString);
        Assert.Equal(_manager.Options.PreferredReaderBehavior, sut.Options.PreferredReaderBehavior);
    }

    [Fact]
    public void ExecuteExists_ShouldVerifyBothExistingAndNonExistingRows()
    {
        var sut1 = _manager.ExecuteExists("SELECT * FROM Product WHERE ProductID = 1");
        var sut2 = _manager.ExecuteExists("SELECT * FROM Product WHERE ProductID = -1");

        Assert.True(sut1);
        Assert.False(sut2);
    }

    [Fact]
    public void ExecuteReader_ShouldReadAllProducts()
    {
        using (var reader = _manager.ExecuteReader(new DataStatement("SELECT * FROM Product")))
        {
            var rows = reader.ToRows();
            var columns = rows.ColumnNames.ToList();
            Assert.Equal(504, rows.Count);
            Assert.Equal(25, columns.Count);
            Assert.InRange(rows.Select(row => row["ModifiedDate"].As<DateTime>().Year).Distinct().Single(), 2013, 2015);
        }
    }

    [Fact]
    public void ExecuteString_ShouldReturnOneStringFromMultipleRows()
    {
        var sut = _manager.ExecuteString("SELECT ProductNumber FROM Product LIMIT 5");
        Assert.Equal("AR-5381BA-8327BE-2349BE-2908BL-2036", sut);
    }

    [Fact]
    public void ExecuteScalar_ShouldReturnTheFirstValueOfTheFirstRow()
    {
        var sut = _manager.ExecuteScalar("SELECT ListPrice, Size FROM Product WHERE ProductID > 780");
        Assert.Equal(2319.99, sut);
    }

    [Fact]
    public void ExecuteScalarAsType_ShouldReturnTheFirstValueOfTheFirstRow()
    {
        var sut = _manager.ExecuteScalarAsType("SELECT ListPrice, Size FROM Product WHERE ProductID > 780", typeof(double));
        Assert.Equal(2319.99, Convert.ToDouble(sut));

        var aggregateException = Assert.Throws<AggregateException>(() => _manager.ExecuteScalarAsType("SELECT ListPrice, Size FROM Product WHERE ProductID > 780", typeof(Guid)));
        Assert.Collection(aggregateException.InnerExceptions,
            ex => Assert.True(ex is InvalidCastException),
            ex => Assert.True(ex is NotSupportedException));
    }

    [Fact]
    public void ExecuteScalarAsGenericDouble_ShouldReturnTheFirstValueOfTheFirstRow()
    {
        var sut = _manager.ExecuteScalarAs<double>("SELECT ListPrice, Size FROM Product WHERE ProductID > 780");
        Assert.Equal(2319.99, sut);
    }

    [Fact]
    public void ExecuteScalarAsBoolean_ShouldReturnTheFirstValueOfTheFirstRow()
    {
        var sut = _manager.ExecuteScalarAs<bool>("SELECT MakeFlag, Size FROM Product WHERE ProductID > 780");
        Assert.True(sut);
    }

    [Fact]
    public void ExecuteScalarAsDateTime_ShouldReturnTheFirstValueOfTheFirstRow()
    {
        var sut = _manager.ExecuteScalarAs<DateTime>("SELECT SellStartDate, Size FROM Product WHERE ProductID > 780");
        Assert.Equal(DateTime.Parse("2012-05-30T00:00:00.0000000"), sut);
    }

    [Fact]
    public void ExecuteScalarAsInt16_ShouldReturnTheFirstValueOfTheFirstRow()
    {
        var sut = _manager.ExecuteScalarAs<short>("SELECT ProductID, Size FROM Product WHERE ProductID > 780");
        Assert.Equal(781, sut);
        Assert.IsType<short>(sut);
    }

    [Fact]
    public void ExecuteScalarAsInt32_ShouldReturnTheFirstValueOfTheFirstRow()
    {
        var sut = _manager.ExecuteScalarAs<int>("SELECT ProductID, Size FROM Product WHERE ProductID > 780");
        Assert.Equal(781, sut);
        Assert.IsType<int>(sut);
    }

    [Fact]
    public void ExecuteScalarAsInt64_ShouldReturnTheFirstValueOfTheFirstRow()
    {
        var sut = _manager.ExecuteScalarAs<long>("SELECT ProductID, Size FROM Product WHERE ProductID > 780");
        Assert.Equal(781, sut);
        Assert.IsType<long>(sut);
    }

    [Fact]
    public void ExecuteScalarAsByte_ShouldReturnTheFirstValueOfTheFirstRow()
    {
        var sut = _manager.ExecuteScalarAs<byte>("SELECT ProductID, Size FROM Product WHERE ProductID = 4");
        Assert.Equal(4, sut);
        Assert.IsType<byte>(sut);
    }

    [Fact]
    public void ExecuteScalarAsSByte_ShouldReturnTheFirstValueOfTheFirstRow()
    {
        var sut = _manager.ExecuteScalarAs<sbyte>("SELECT ProductID, Size FROM Product WHERE ProductID = 4");
        Assert.Equal(4, sut);
        Assert.IsType<sbyte>(sut);
    }

    [Fact]
    public void ExecuteScalarAsDecimal_ShouldReturnTheFirstValueOfTheFirstRow()
    {
        var sut = _manager.ExecuteScalarAs<decimal>("SELECT ListPrice, Size FROM Product WHERE ProductID > 780");
        Assert.Equal(2319.99m, decimal.Round(sut, 2));
        Assert.IsType<decimal>(sut);
    }

    [Fact]
    public void ExecuteScalarAsDouble_ShouldReturnTheFirstValueOfTheFirstRow()
    {
        var sut = _manager.ExecuteScalarAs<double>("SELECT ListPrice, Size FROM Product WHERE ProductID > 780");
        Assert.Equal(2319.99, sut);
        Assert.IsType<double>(sut);
    }

    [Fact]
    public void ExecuteScalarAsUInt16_ShouldReturnTheFirstValueOfTheFirstRow()
    {
        var sut = _manager.ExecuteScalarAs<ushort>("SELECT ProductID, Size FROM Product WHERE ProductID > 780");
        Assert.Equal((ushort)781, sut);
        Assert.IsType<ushort>(sut);
    }

    [Fact]
    public void ExecuteScalarAsUInt32_ShouldReturnTheFirstValueOfTheFirstRow()
    {
        var sut = _manager.ExecuteScalarAs<uint>("SELECT ProductID, Size FROM Product WHERE ProductID > 780");
        Assert.Equal((uint)781, sut);
        Assert.IsType<uint>(sut);
    }

    [Fact]
    public void ExecuteScalarAsUInt64_ShouldReturnTheFirstValueOfTheFirstRow()
    {
        var sut = _manager.ExecuteScalarAs<ulong>("SELECT ProductID, Size FROM Product WHERE ProductID > 780");
        Assert.Equal((ulong)781, sut);
        Assert.IsType<ulong>(sut);
    }

    [Fact]
    public void ExecuteScalarAsString_ShouldReturnTheFirstValueOfTheFirstRow()
    {
        var sut = _manager.ExecuteScalarAs<string>("SELECT Name, Size FROM Product WHERE ProductID > 780");
        Assert.Equal("Mountain-200 Silver, 46", sut);
        Assert.IsType<string>(sut);
    }

    [Fact]
    public void ExecuteScalarAsGuid_ShouldReturnTheFirstValueOfTheFirstRow()
    {
        var sut = _manager.ExecuteScalarAs<Guid>("SELECT RowGuid, Size FROM Product WHERE ProductID > 780");
        Assert.Equal(Guid.Parse("20799030-420C-496A-9922-09189C2B457E"), sut);
        Assert.IsType<Guid>(sut);
    }


    [Fact]
    public async Task ExecuteExistsAsync_ShouldVerifyBothExistingAndNonExistingRows()
    {
        var sut1 = await _manager.ExecuteExistsAsync("SELECT * FROM Product WHERE ProductID = 1");
        var sut2 = await _manager.ExecuteExistsAsync("SELECT * FROM Product WHERE ProductID = -1");

        Assert.True(sut1);
        Assert.False(sut2);
    }

    [Fact]
    public async Task ExecuteReaderAsync_ShouldReadAllProducts()
    {
        using (var reader = await _manager.ExecuteReaderAsync(new DataStatement("SELECT * FROM Product")))
        {
            var rows = reader.ToRows();
            var columns = rows.ColumnNames.ToList();
            Assert.Equal(504, rows.Count);
            Assert.Equal(25, columns.Count);
            Assert.InRange(rows.Select(row => row["ModifiedDate"].As<DateTime>().Year).Distinct().Single(), 2013, 2015);
        }
    }

    [Fact]
    public async Task ExecuteStringAsync_ShouldReturnOneStringFromMultipleRows()
    {
        var sut = await _manager.ExecuteStringAsync("SELECT ProductNumber FROM Product LIMIT 5");
        Assert.Equal("AR-5381BA-8327BE-2349BE-2908BL-2036", sut);
    }

    [Fact]
    public async Task ExecuteScalarAsync_ShouldReturnTheFirstValueOfTheFirstRow()
    {
        var sut = await _manager.ExecuteScalarAsync("SELECT ListPrice, Size FROM Product WHERE ProductID > 780");
        Assert.Equal(2319.99, Convert.ToDouble(sut));
    }

    [Fact]
    public async Task ExecuteScalarAsTypeAsync_ShouldReturnTheFirstValueOfTheFirstRow()
    {
        var sut = await _manager.ExecuteScalarAsTypeAsync("SELECT ListPrice, Size FROM Product WHERE ProductID > 780", typeof(double));
        Assert.Equal(2319.99, Convert.ToDouble(sut));

        var aggregateException = await Assert.ThrowsAsync<AggregateException>(() => _manager.ExecuteScalarAsTypeAsync("SELECT ListPrice, Size FROM Product WHERE ProductID > 780", typeof(Guid)));
        Assert.Collection(aggregateException.InnerExceptions,
            ex => Assert.True(ex is InvalidCastException),
            ex => Assert.True(ex is NotSupportedException));
    }

    [Fact]
    public async Task ExecuteScalarAsGenericDoubleAsync_ShouldReturnTheFirstValueOfTheFirstRow()
    {
        var sut = await _manager.ExecuteScalarAsAsync<double>("SELECT ListPrice, Size FROM Product WHERE ProductID > 780");
        Assert.Equal(2319.99, sut);
    }

    [Fact]
    public async Task ExecuteScalarAsBooleanAsync_ShouldReturnTheFirstValueOfTheFirstRow()
    {
        var sut = await _manager.ExecuteScalarAsAsync<bool>("SELECT MakeFlag, Size FROM Product WHERE ProductID > 780");
        Assert.True(sut);
    }

    [Fact]
    public async Task ExecuteScalarAsDateTimeAsync_ShouldReturnTheFirstValueOfTheFirstRow()
    {
        var sut = await _manager.ExecuteScalarAsAsync<DateTime>("SELECT SellStartDate, Size FROM Product WHERE ProductID > 780");
        Assert.Equal(DateTime.Parse("2012-05-30T00:00:00.0000000"), sut);
    }

    [Fact]
    public async Task ExecuteScalarAsInt16Async_ShouldReturnTheFirstValueOfTheFirstRow()
    {
        var sut = await _manager.ExecuteScalarAsAsync<short>("SELECT ProductID, Size FROM Product WHERE ProductID > 780");
        Assert.Equal(781, sut);
        Assert.IsType<short>(sut);
    }

    [Fact]
    public async Task ExecuteScalarAsInt32Async_ShouldReturnTheFirstValueOfTheFirstRow()
    {
        var sut = await _manager.ExecuteScalarAsAsync<int>("SELECT ProductID, Size FROM Product WHERE ProductID > 780");
        Assert.Equal(781, sut);
        Assert.IsType<int>(sut);
    }

    [Fact]
    public async Task ExecuteScalarAsInt64Async_ShouldReturnTheFirstValueOfTheFirstRow()
    {
        var sut = await _manager.ExecuteScalarAsAsync<long>("SELECT ProductID, Size FROM Product WHERE ProductID > 780");
        Assert.Equal(781, sut);
        Assert.IsType<long>(sut);
    }

    [Fact]
    public async Task ExecuteScalarAsByteAsync_ShouldReturnTheFirstValueOfTheFirstRow()
    {
        var sut = await _manager.ExecuteScalarAsAsync<byte>("SELECT ProductID, Size FROM Product WHERE ProductID = 4");
        Assert.Equal(4, sut);
        Assert.IsType<byte>(sut);
    }

    [Fact]
    public async Task ExecuteScalarAsSByteAsync_ShouldReturnTheFirstValueOfTheFirstRow()
    {
        var sut = await _manager.ExecuteScalarAsAsync<sbyte>("SELECT ProductID, Size FROM Product WHERE ProductID = 4");
        Assert.Equal(4, sut);
        Assert.IsType<sbyte>(sut);
    }

    [Fact]
    public async Task ExecuteScalarAsDecimalAsync_ShouldReturnTheFirstValueOfTheFirstRow()
    {
        var sut = await _manager.ExecuteScalarAsAsync<decimal>("SELECT ListPrice, Size FROM Product WHERE ProductID > 780");
        Assert.Equal(2319.99m, decimal.Round(sut, 2));
        Assert.IsType<decimal>(sut);
    }

    [Fact]
    public async Task ExecuteScalarAsDoubleAsync_ShouldReturnTheFirstValueOfTheFirstRow()
    {
        var sut = await _manager.ExecuteScalarAsAsync<double>("SELECT ListPrice, Size FROM Product WHERE ProductID > 780");
        Assert.Equal(2319.99, sut);
        Assert.IsType<double>(sut);
    }

    [Fact]
    public async Task ExecuteScalarAsUInt16Async_ShouldReturnTheFirstValueOfTheFirstRow()
    {
        var sut = await _manager.ExecuteScalarAsAsync<ushort>("SELECT ProductID, Size FROM Product WHERE ProductID > 780");
        Assert.Equal((ushort)781, sut);
        Assert.IsType<ushort>(sut);
    }

    [Fact]
    public async Task ExecuteScalarAsUInt32Async_ShouldReturnTheFirstValueOfTheFirstRow()
    {
        var sut = await _manager.ExecuteScalarAsAsync<uint>("SELECT ProductID, Size FROM Product WHERE ProductID > 780");
        Assert.Equal((uint)781, sut);
        Assert.IsType<uint>(sut);
    }

    [Fact]
    public async Task ExecuteScalarAsUInt64Async_ShouldReturnTheFirstValueOfTheFirstRow()
    {
        var sut = await _manager.ExecuteScalarAsAsync<ulong>("SELECT ProductID, Size FROM Product WHERE ProductID > 780");
        Assert.Equal((ulong)781, sut);
        Assert.IsType<ulong>(sut);
    }

    [Fact]
    public async Task ExecuteScalarAsStringAsync_ShouldReturnTheFirstValueOfTheFirstRow()
    {
        var sut = await _manager.ExecuteScalarAsAsync<string>("SELECT Name, Size FROM Product WHERE ProductID > 780");
        Assert.Equal("Mountain-200 Silver, 46", sut);
        Assert.IsType<string>(sut);
    }

    [Fact]
    public async Task ExecuteScalarAsGuidAsync_ShouldReturnTheFirstValueOfTheFirstRow()
    {
        var sut = await _manager.ExecuteScalarAsAsync<Guid>("SELECT RowGuid, Size FROM Product WHERE ProductID > 780");
        Assert.Equal(Guid.Parse("20799030-420C-496A-9922-09189C2B457E"), sut);
        Assert.IsType<Guid>(sut);
    }

    public override void ConfigureServices(IServiceCollection services)
    {
        var manager = new FakeDataManager(o =>
        {
            o.LeaveConnectionOpen = true; // never release our connection as it will be closed and our in-mem database will be removed (for normal dbs - always leave false)
            o.LeaveCommandOpen = true; // do not release our command as it will trigger errors from data readers (for normal dbs - always leave false)
            o.ConnectionString = $"Data Source=InMemory-{Guid.NewGuid():N};Mode=Memory;Cache=Shared";
        });
        SqliteDatabase.Create(manager, TestOutput);
        services.AddSingleton(manager);
    }

}
