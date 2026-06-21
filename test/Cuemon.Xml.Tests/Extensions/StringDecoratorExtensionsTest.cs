using System;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Xml
{
    public class StringDecoratorExtensionsTest : Test
    {
        public StringDecoratorExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void EscapeXml_ShouldEscapeSpecialCharacters()
        {
            var result = Decorator.Enclose("<hello & 'world' \"test\">").EscapeXml();
            Assert.Equal("&lt;hello &amp; &apos;world&apos; &quot;test&quot;&gt;", result);
            TestOutput.WriteLine(result);
        }

        [Fact]
        public void EscapeXml_ShouldReturnSameString_WhenNoSpecialChars()
        {
            var result = Decorator.Enclose("hello world").EscapeXml();
            Assert.Equal("hello world", result);
        }

        [Fact]
        public void EscapeXml_Null_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => StringDecoratorExtensions.EscapeXml(null));
        }

        [Fact]
        public void UnescapeXml_ShouldUnescapeEntities()
        {
            var result = Decorator.Enclose("&lt;hello &amp; &apos;world&apos; &quot;test&quot;&gt;").UnescapeXml();
            Assert.Equal("<hello & 'world' \"test\">", result);
            TestOutput.WriteLine(result);
        }

        [Fact]
        public void UnescapeXml_ShouldReturnSameString_WhenNoEntities()
        {
            var result = Decorator.Enclose("hello world").UnescapeXml();
            Assert.Equal("hello world", result);
        }

        [Fact]
        public void UnescapeXml_Null_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => StringDecoratorExtensions.UnescapeXml(null));
        }

        [Fact]
        public void SanitizeXmlElementName_ShouldRemoveInvalidCharacters()
        {
            var result = Decorator.Enclose("hello world! @#test").SanitizeXmlElementName();
            Assert.Equal("helloworldtest", result);
            TestOutput.WriteLine(result);
        }

        [Fact]
        public void SanitizeXmlElementName_ShouldTrimLeadingNumbers()
        {
            var result = Decorator.Enclose("123abc").SanitizeXmlElementName();
            Assert.Equal("abc", result);
            TestOutput.WriteLine(result);
        }

        [Fact]
        public void SanitizeXmlElementName_ShouldTrimLeadingDots()
        {
            var result = Decorator.Enclose(".abc").SanitizeXmlElementName();
            Assert.Equal("abc", result);
        }

        [Fact]
        public void SanitizeXmlElementName_ShouldAllowValidCharacters()
        {
            var result = Decorator.Enclose("valid-element_name.123").SanitizeXmlElementName();
            Assert.Equal("valid-element_name.123", result);
        }

        [Fact]
        public void SanitizeXmlElementName_Null_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => StringDecoratorExtensions.SanitizeXmlElementName(null));
        }

        [Fact]
        public void SanitizeXmlElementText_ShouldRemoveControlCharacters()
        {
            var input = "hello\x0001\x0002\x0005world";
            var result = Decorator.Enclose(input).SanitizeXmlElementText();
            Assert.Equal("helloworld", result);
            TestOutput.WriteLine(result);
        }

        [Fact]
        public void SanitizeXmlElementText_ShouldReturnEmpty_WhenInputIsEmpty()
        {
            var result = Decorator.Enclose("").SanitizeXmlElementText();
            Assert.Equal("", result);
        }

        [Fact]
        public void SanitizeXmlElementText_WithCdataSection_ShouldRemoveCdataClosingSequence()
        {
            var input = "hello]]>world";
            var result = Decorator.Enclose(input).SanitizeXmlElementText(cdataSection: true);
            Assert.Equal("helloworld", result);
            TestOutput.WriteLine(result);
        }

        [Fact]
        public void SanitizeXmlElementText_WithoutCdataSection_ShouldPreserveCdataSequence()
        {
            var input = "hello]]>world";
            var result = Decorator.Enclose(input).SanitizeXmlElementText(cdataSection: false);
            Assert.Equal("hello]]>world", result);
        }

        [Fact]
        public void SanitizeXmlElementText_Null_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => StringDecoratorExtensions.SanitizeXmlElementText(null));
        }
    }
}
