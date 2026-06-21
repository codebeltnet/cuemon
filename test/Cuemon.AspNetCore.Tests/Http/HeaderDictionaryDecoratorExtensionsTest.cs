using System.Net.Http;
using Codebelt.Extensions.Xunit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Xunit;

namespace Cuemon.AspNetCore.Http
{
    public class HeaderDictionaryDecoratorExtensionsTest : Test
    {
        public HeaderDictionaryDecoratorExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void AddRange_ShouldAddOnlyMissingHeaders_WhenPredicateIsNotSpecified()
        {
            var target = new HeaderDictionary { { "X-Existing", "1" } };
            var source = new HeaderDictionary
            {
                { "X-New", "3" }
            };

            var sut = Decorator.Enclose<IHeaderDictionary>(target).AddRange(source);

            Assert.Same(target, sut);
            Assert.Equal("1", target["X-Existing"]);
            Assert.Equal("3", target["X-New"]);
        }

        [Fact]
        public void AddOrUpdateHeader_ShouldSanitizeControlCharacters()
        {
            var sut = new HeaderDictionary();
            var decorator = Decorator.Enclose<IHeaderDictionary>(sut);

            decorator.AddOrUpdateHeader("X-Test", new StringValues("value\r\n"), useAsciiEncodingConversion: false);

            Assert.Equal("value", sut["X-Test"]);
        }

        [Fact]
        public void AddOrUpdateHeaders_ShouldIgnoreNullArguments_AndCopyResponseHeaders()
        {
            var sut = new HeaderDictionary();
            var response = new HttpResponseMessage();
            response.Headers.Add("X-Test", new[] { "one", "two" });

            HeaderDictionaryDecoratorExtensions.AddOrUpdateHeaders(null, response.Headers);
            Decorator.Enclose<IHeaderDictionary>(sut).AddOrUpdateHeaders(null);
            Decorator.Enclose<IHeaderDictionary>(sut).AddOrUpdateHeaders(response.Headers);

            Assert.Equal("one,two", sut["X-Test"]);
        }
    }
}
