using System.IO;
using System.Linq;
using Cuemon.Data;
using Cuemon.Extensions.Reflection;
using Codebelt.Extensions.Xunit;
using Cuemon.Reflection;
using Xunit;

namespace Cuemon.Extensions.Data;
public class DataReaderExtensionsTest : Test
{
    public DataReaderExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void ToRows_ShouldConvertDataReaderToDataTransferRowCollection()
    {
        var sut1 = this.GetType().GetEmbeddedResources("DsvDataReaderTest_Wiki.csv", ManifestResourceMatch.ContainsName).Values.Single();
        var sut2 = new DsvDataReader(new StreamReader(sut1));
        var sut3 = sut2.ToRows();

        foreach (var sut4 in sut3)
        {
            TestOutput.WriteLine(sut4.ToString());
        }

        sut2.Dispose();

        Assert.Equal(4, sut3.Count);
        Assert.Equal(5, sut3.ColumnNames.Count());
    }

    [Fact]
    public void ToColumns_ShouldConvertDataReaderTDataTransferColumnCollection()
    {
        var sut1 = this.GetType().GetEmbeddedResources("DsvDataReaderTest_Wiki.csv", ManifestResourceMatch.ContainsName).Values.Single();
        var sut2 = new DsvDataReader(new StreamReader(sut1));

        sut2.Read(); // we have to read at least once to have columns (reason is, that IDataReader source can be anything .. and does not necessary know before hand what columns are exposed per row

        var sut3 = sut2.ToColumns();

        foreach (var sut4 in sut3)
        {
            TestOutput.WriteLine(sut4.ToString());
        }

        sut2.Dispose();

        Assert.Equal(5, sut3.Count);
    }
}
