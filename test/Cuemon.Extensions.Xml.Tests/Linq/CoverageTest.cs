using System.Xml.Linq;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Extensions.Xml.Linq
{
    public class CoverageTest : Test
    {
        public CoverageTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void StringExtensions_ShouldParseAndValidateXmlStrings()
        {
            Assert.True("<root> <item /> </root>".TryParseXElement(LoadOptions.PreserveWhitespace, out var element));
            Assert.Equal("root", element.Name.LocalName);
            Assert.True("<root />".IsXmlString());
            Assert.False("not-xml".TryParseXElement(out _));
            Assert.False(string.Empty.IsXmlString());
        }
    }
}
