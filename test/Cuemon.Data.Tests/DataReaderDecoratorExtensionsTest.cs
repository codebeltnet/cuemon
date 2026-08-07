using System;
using System.Data;
using System.IO;
using System.Linq;
using Codebelt.Extensions.Xunit;
using Cuemon.Reflection;
using Xunit;

namespace Cuemon.Data;
public class DataReaderDecoratorExtensionsTest : Test
{
    public DataReaderDecoratorExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void ToEncodedString_ShouldThrowArgumentNullException()
    {
        var sut = Assert.Throws<ArgumentNullException>(() => Decorator.Enclose((IDataReader)null, false).ToEncodedString());
        Assert.Equal("decorator", sut.ParamName);
        Assert.StartsWith("Value cannot be null.", sut.Message);
    }

    [Fact]
    public void ToEncodedString_ShouldThrowArgumentException()
    {
        var file = Decorator.Enclose(typeof(DsvDataReaderTest).Assembly).GetManifestResources("AdventureWorks2022_Product.csv", ManifestResourceMatch.ContainsName).Values.Single();
        using var reader = new DsvDataReader(new StreamReader(file), setup: o => o.Delimiter = ";");
        var sut2 = Assert.Throws<ArgumentException>(() => Decorator.Enclose(reader).ToEncodedString());
        Assert.Equal("reader", sut2.ParamName);
        Assert.StartsWith("The executed command statement appears to contain invalid fields. Expected field count is 1. Actually field count was 25. (Expression 'reader.FieldCount > 1')", sut2.Message);
    }
}
