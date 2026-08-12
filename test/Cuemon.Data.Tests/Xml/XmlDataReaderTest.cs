using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using Cuemon.Extensions.Reflection;
using Codebelt.Extensions.Xunit;
using Cuemon.IO;
using Cuemon.Reflection;
using Xunit;

namespace Cuemon.Data.Xml;
public class XmlDataReaderTest : Test
{
    public XmlDataReaderTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void XmlDataReader_ShouldReadAllRows()
    {
        var file = typeof(XmlDataReaderTest).GetEmbeddedResources("Professional.xml", ManifestResourceMatch.ContainsName).Values.Single();
        var msXml = new MemoryStream();
        Decorator.Enclose(file).CopyStream(msXml);
        var xp = new XPathDocument(msXml).CreateNavigator();
        var xmlReader = xp.ReadSubtree();
        var sb1 = new StringBuilder();
        var sb2 = new StringBuilder();

        string elementName = null;
        while (xmlReader.Read())
        {
            if (xmlReader.NodeType == XmlNodeType.Element)
            {
                elementName = xmlReader.LocalName;
            }

            if (xmlReader.HasAttributes)
            {
                while (xmlReader.MoveToNextAttribute())
                {
                    sb1.AppendLine($"{xmlReader.LocalName}={xmlReader.Value}");
                }

            }
            else if (!string.IsNullOrEmpty(xmlReader.Value))
            {
                sb1.AppendLine($"{elementName}={xmlReader.Value}");
            }
        }


        XmlDataReader dataReader;
        using (dataReader = new XmlDataReader(XmlReader.Create(file)))
        {
            while (dataReader.Read())
            {
                for (var i = 0; i < dataReader.FieldCount; i++)
                {
                    sb2.AppendLine($"{dataReader.GetName(i)}={dataReader.GetValue(i)}");
                }
            }
            Assert.True(xmlReader.EOF);
        }

        TestOutput.WriteLine(sb1.ToString());

        Assert.Equal(sb1.ToString(), sb2.ToString());
        Assert.Equal(345, dataReader.RowCount);
        Assert.True(dataReader.Disposed);
        Assert.Throws<ObjectDisposedException>(() => dataReader.Read());
    }
}
