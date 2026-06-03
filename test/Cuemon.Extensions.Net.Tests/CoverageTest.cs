using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Extensions.Net
{
    public class CoverageTest : Test
    {
        public CoverageTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Extensions_ShouldEncodeAndFormatQueryValues()
        {
            var bytes = Encoding.ASCII.GetBytes("a &");
            var dictionary = new Dictionary<string, string[]>
            {
                { "name", new[] { "Jane Doe" } },
                { "tag", new[] { "one", "two" } }
            };
            var values = new NameValueCollection()
            {
                { "message", "hello world" },
                { "tag", "alpha,beta" }
            };

            Assert.Equal("a+%26", Encoding.ASCII.GetString(bytes.UrlEncode(0, bytes.Length)));
            Assert.Equal("?name=Jane+Doe&tag=one&tag=two", dictionary.ToQueryString(true));
            Assert.Equal("?message=hello+world&tag=alpha&tag=beta", values.ToQueryString(true));
            Assert.Equal("hello+world", "hello world".UrlEncode());
            Assert.Equal("hello world", "hello+world".UrlDecode());
            Assert.Null(((string)null).UrlEncode());
            Assert.Throws<ArgumentNullException>(() => ByteArrayExtensions.UrlEncode(null, 0, 0));
            Assert.Throws<ArgumentNullException>(() => DictionaryExtensions.ToQueryString(null));
            Assert.Throws<ArgumentNullException>(() => NameValueCollectionExtensions.ToQueryString(null));
        }

        [Theory]
        [InlineData((HttpStatusCode)99, false, false, false, false, false)]
        [InlineData((HttpStatusCode)100, true, false, false, false, false)]
        [InlineData((HttpStatusCode)199, true, false, false, false, false)]
        [InlineData((HttpStatusCode)200, false, true, false, false, false)]
        [InlineData((HttpStatusCode)299, false, true, false, false, false)]
        [InlineData((HttpStatusCode)300, false, false, true, false, false)]
        [InlineData((HttpStatusCode)399, false, false, true, false, false)]
        [InlineData((HttpStatusCode)400, false, false, false, true, false)]
        [InlineData((HttpStatusCode)499, false, false, false, true, false)]
        [InlineData((HttpStatusCode)500, false, false, false, false, true)]
        [InlineData((HttpStatusCode)599, false, false, false, false, true)]
        [InlineData((HttpStatusCode)600, false, false, false, false, false)]
        public void HttpStatusCodeExtensions_ShouldMatchExpectedRanges(HttpStatusCode statusCode, bool information, bool success, bool redirection, bool clientError, bool serverError)
        {
            Assert.Equal(information, statusCode.IsInformationStatusCode());
            Assert.Equal(success, statusCode.IsSuccessStatusCode());
            Assert.Equal(redirection, statusCode.IsRedirectionStatusCode());
            Assert.Equal(clientError, statusCode.IsClientErrorStatusCode());
            Assert.Equal(serverError, statusCode.IsServerErrorStatusCode());
        }
    }
}
