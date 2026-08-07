using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Cuemon.Reflection;
using Xunit;

namespace Cuemon.Data;
public class DsvDataReaderTest : Test
{
    public DsvDataReaderTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Read_ShouldReadEmbeddedResourceLineByLine()
    {
        var file = Decorator.Enclose(typeof(DsvDataReaderTest).Assembly).GetManifestResources("DsvDataReaderTest_Wiki.csv", ManifestResourceMatch.ContainsName).Values.Single();
        DsvDataReader reader = null;
        using (reader = new DsvDataReader(new StreamReader(file)))
        {
            while (reader.Read())
            {
                TestOutput.WriteLine(reader.ToString());
            }
        }
        Assert.Equal(5, reader.FieldCount);
        Assert.Equal(4, reader.RowCount);
        Assert.True(reader.Disposed);
        Assert.Throws<ObjectDisposedException>(() => reader.Read());
    }

    [Fact]
    public void Read_ShouldReadLargeEmbeddedResourceLineByLine()
    {
        var file = Decorator.Enclose(typeof(DsvDataReaderTest).Assembly).GetManifestResources("DsvDataReaderTest_100000_SalesRecords.csv", ManifestResourceMatch.ContainsName).Values.Single();
        DsvDataReader reader = null;
        using (reader = new DsvDataReader(new StreamReader(file)))
        {
            while (reader.Read())
            {
                TestOutput.WriteLine(reader.ToString());
            }
        }
        Assert.Equal(14, reader.FieldCount);
        Assert.Equal(100000, reader.RowCount);
        Assert.True(reader.Disposed);
        Assert.Throws<ObjectDisposedException>(() => reader.Read());
    }

    [Fact]
    public async Task ReadAsync_ShouldReadEmbeddedResourceLineByLine()
    {
        var file = Decorator.Enclose(typeof(DsvDataReaderTest).Assembly).GetManifestResources("DsvDataReaderTest_Wiki.csv", ManifestResourceMatch.ContainsName).Values.Single();
        DsvDataReader reader = null;
        using (reader = new DsvDataReader(new StreamReader(file)))
        {
            while (await reader.ReadAsync())
            {
                TestOutput.WriteLine(reader.ToString());
            }
        }
        Assert.Equal(5, reader.FieldCount);
        Assert.Equal(4, reader.RowCount);
        Assert.True(reader.Disposed);
        await Assert.ThrowsAsync<ObjectDisposedException>(() => reader.ReadAsync());
    }

    [Fact]
    public async Task ReadAsync_ShouldReadLargeEmbeddedResourceLineByLineAsync()
    {
        var file = Decorator.Enclose(typeof(DsvDataReaderTest).Assembly).GetManifestResources("DsvDataReaderTest_100000_SalesRecords.csv", ManifestResourceMatch.ContainsName).Values.Single();
        DsvDataReader reader = null;
        using (reader = new DsvDataReader(new StreamReader(file)))
        {
            while (await reader.ReadAsync())
            {
                TestOutput.WriteLine(reader.ToString());
            }
        }
        Assert.Equal(14, reader.FieldCount);
        Assert.Equal(100000, reader.RowCount);
        Assert.True(reader.Disposed);
        await Assert.ThrowsAsync<ObjectDisposedException>(() => reader.ReadAsync());
    }
}
