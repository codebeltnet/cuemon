using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Net
{
    public class CoverageTest : Test
    {
        public CoverageTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void QueryStringCollection_ShouldSupportCopyConstructionEnumerationAndDecoding()
        {
            var sut = new QueryStringCollection("?name=Jane+Doe&city=Copenhagen&name=John");
            var clone = new QueryStringCollection(sut);
            var decoded = new QueryStringCollection("?message=hello+world&encoded=%26value", true);
            var readOnly = (IReadOnlyCollection<KeyValuePair<string, string>>)clone;
            var nongeneric = (IEnumerable)clone;

            Assert.Equal(2, readOnly.Count);
            Assert.Equal("Jane+Doe,John", clone["name"]);
            Assert.Equal("hello world", decoded["message"]);
            Assert.Equal("&value", decoded["encoded"]);
            Assert.Contains("name=Jane", clone.ToString());
            Assert.Contains("name=John", clone.ToString());
            Assert.Contains("city=Copenhagen", clone.ToString());
            Assert.True(readOnly.GetEnumerator().MoveNext());
            Assert.True(nongeneric.GetEnumerator().MoveNext());
            Assert.Equal(string.Empty, new QueryStringCollection().ToString());
        }

        [Fact]
        public void ByteArrayDecoratorExtensions_ShouldEncodeBytesAndValidateRanges()
        {
            var bytes = Encoding.ASCII.GetBytes("a &");
            IDecorator<byte[]> decorator = null;

            var encoded = Decorator.Enclose(bytes).UrlEncode(0, bytes.Length);

            Assert.Equal("a+%26", Encoding.ASCII.GetString(encoded));
            Assert.Empty(Decorator.Enclose(Array.Empty<byte>()).UrlEncode());
            Assert.Throws<ArgumentNullException>(() => ByteArrayDecoratorExtensions.UrlEncode(decorator, 0, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => Decorator.Enclose(bytes).UrlEncode(-1, bytes.Length));
            Assert.Throws<ArgumentOutOfRangeException>(() => Decorator.Enclose(bytes).UrlEncode(0, bytes.Length + 1));
        }

        [Fact]
        public void StringDecoratorExtensions_ShouldHandleNullEmptyAndMalformedEscapes()
        {
            IDecorator<string> decorator = null;

            Assert.Null(Decorator.Enclose((string)null, false).UrlEncode());
            Assert.Equal(string.Empty, Decorator.Enclose(string.Empty).UrlEncode());
            Assert.Equal("ABC", Decorator.Enclose("ABC").UrlDecode());
            Assert.Equal("A", Decorator.Enclose("%u0041").UrlDecode());
            Assert.Equal("%u", Decorator.Enclose("%u").UrlDecode());
            Assert.Equal("%u00ZZ", Decorator.Enclose("%u00ZZ").UrlDecode());
            Assert.Equal("%GG", Decorator.Enclose("%GG").UrlDecode());
            Assert.Equal(" ", Decorator.Enclose("+").UrlDecode());
            Assert.Throws<ArgumentNullException>(() => StringDecoratorExtensions.UrlEncode(decorator));
            Assert.Throws<ArgumentNullException>(() => StringDecoratorExtensions.UrlDecode(decorator));
        }
    }
}

namespace Cuemon.Net.Collections.Specialized
{
    public class CoverageTest : Test
    {
        public CoverageTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void NameValueCollectionDecoratorExtensions_ShouldFormatCollectionsWithDifferentSeparators()
        {
            var values = new NameValueCollection()
            {
                { "name", "Jane Doe" },
                { "tags", "one,two" }
            };
            IDecorator<NameValueCollection> decorator = null;

            var ampersand = Decorator.Enclose(values).ToString(FieldValueSeparator.Ampersand, true);
            var semicolon = Decorator.Enclose(values).ToString(FieldValueSeparator.Semicolon, false);

            Assert.Equal("?name=Jane+Doe&tags=one&tags=two", ampersand);
            Assert.Equal("name=Jane Doe;tags=one;tags=two;", semicolon);
            Assert.Throws<ArgumentNullException>(() => NameValueCollectionDecoratorExtensions.ToString(decorator, FieldValueSeparator.Ampersand, false));
            Assert.Throws<System.ComponentModel.InvalidEnumArgumentException>(() => Decorator.Enclose(values).ToString((FieldValueSeparator)99, false));
        }
    }
}
