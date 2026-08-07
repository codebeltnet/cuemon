using System;
using System.IO;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Data;
public class DataReaderVariantsAndExceptionsTest : Test
{
    public DataReaderVariantsAndExceptionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void ShouldCoverPublicBehavior_DsvAndXmlReadersAndExceptions()
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
}
