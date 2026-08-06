using Cuemon.Extensions.IO;
using Cuemon.Extensions.Xml.Assets;
using Codebelt.Extensions.Xunit;
using Cuemon.Xml.Serialization.Formatters;
using Xunit;

namespace Cuemon.Extensions.Xml;
public class StringExtensionsTest : Test
{
    public StringExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void EscapeXml_ShouldEscapeXmlAndViceVersa()
    {
        var sut1 = new XmlFormatter(o => o.Settings.Writer.Indent = true).Serialize(new HierarchyExample());
        var sut2 = sut1.ToEncodedString();
        var sut3 = sut2.EscapeXml();
        var sut4 = sut3.UnescapeXml();

        TestOutput.WriteLine(sut3);

        Assert.NotEqual(sut2, sut3);
        Assert.Equal(sut2, sut4);
    }

    [Fact]
    public void SanitizeXmlElementName_ShouldEnsureValidXmlElementName()
    {
        var sut1 = "validXmlElementName".SanitizeXmlElementName();
        var sut2 = "1nvalidXmlElementName".SanitizeXmlElementName();
        var sut3 = "invalidXml ElementName".SanitizeXmlElementName();

        Assert.Equal("validXmlElementName", sut1);
        Assert.Equal("nvalidXmlElementName", sut2);
        Assert.Equal("invalidXmlElementName", sut3);
    }

    [Fact]
    public void SanitizeXmlElementText_ShouldEnsureValidXmlText()
    {
        var sut1 = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. \x0001 \x0002 \x0003 \x0004 \x0005 \x0006 \x0007 \x0008 \x0011 \x0012 \x0014 \x0015 \x0016 \x0017 \x0018 \x0019]]>";
        var sut2 = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. \x0001 \x0002 \x0003 \x0004 \x0005 \x0006 \x0007 \x0008 \x0011 \x0012 \x0014 \x0015 \x0016 \x0017 \x0018 \x0019]]>";
        var sut3 = sut1.SanitizeXmlElementText();
        var sut4 = sut2.SanitizeXmlElementText(true);

        TestOutput.WriteLine(sut4);

        Assert.NotEqual(sut1, sut3);
        Assert.NotEqual(sut2, sut4);
        Assert.Equal("Lorem Ipsum is simply dummy text of the printing and typesetting industry.                ]]>", sut3);
        Assert.Equal("Lorem Ipsum is simply dummy text of the printing and typesetting industry.                ", sut4);
    }
}
