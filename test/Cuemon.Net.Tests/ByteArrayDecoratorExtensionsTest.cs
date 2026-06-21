using System;
using System.Text;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Net
{
    public class ByteArrayDecoratorExtensionsTest : Test
    {
        public ByteArrayDecoratorExtensionsTest(ITestOutputHelper output) : base(output)
        {
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
    }
}
