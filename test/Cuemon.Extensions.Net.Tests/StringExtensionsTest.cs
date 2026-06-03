using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Extensions.Net
{
    public class StringExtensionsTest : Test
    {
        public StringExtensionsTest(ITestOutputHelper output) : base(output)
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
    }
}
