using System;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon
{
    public class EradicateTest : Test
    {
        public EradicateTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void TrailingZeros_WithNullBytes_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Eradicate.TrailingZeros(null));
        }

        [Fact]
        public void TrailingZeros_WithSingleByte_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Eradicate.TrailingZeros(new byte[] { 0 }));
        }

        [Fact]
        public void TrailingZeros_WithTrailingZeros_RemovesAllTrailingZeros()
        {
            var sut = Eradicate.TrailingZeros(new byte[] { 1, 2, 3, 0, 0, 0 });

            Assert.Equal(new byte[] { 1, 2, 3 }, sut);
        }

        [Fact]
        public void TrailingBytes_WithNullTrailingBytes_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Eradicate.TrailingBytes(new byte[] { 1, 2 }, null));
        }

        [Fact]
        public void TrailingBytes_WithSingleInputByte_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Eradicate.TrailingBytes(new byte[] { 1 }, new byte[] { 1 }));
        }

        [Fact]
        public void TrailingBytes_WithRepeatedTrailingPattern_RemovesAllTrailingPatterns()
        {
            var sut = Eradicate.TrailingBytes(new byte[] { 1, 2, 13, 10, 13, 10 }, new byte[] { 13, 10 });

            Assert.Equal(new byte[] { 1, 2 }, sut);
        }

        [Fact]
        public void TrailingBytes_WithoutTrailingPattern_ReturnsSameInstance()
        {
            var bytes = new byte[] { 1, 2, 3, 4 };

            var sut = Eradicate.TrailingBytes(bytes, new byte[] { 5 });

            Assert.Same(bytes, sut);
        }
    }
}
