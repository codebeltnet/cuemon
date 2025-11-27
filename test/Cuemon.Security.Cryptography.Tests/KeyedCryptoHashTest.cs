using System;
using System.Security.Cryptography;
using System.Text;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Security.Cryptography
{
    public class KeyedCryptoHashTest : Test
    {
        public KeyedCryptoHashTest(ITestOutputHelper output) : base(output)
        {
        }

        private sealed class HmacSha256TestHash : KeyedCryptoHash<HMACSHA256>
        {
            public HmacSha256TestHash(byte[] secret) : base(secret, null)
            {
            }
        }

        private sealed class HmacMd5TestHash : KeyedCryptoHash<HMACMD5>
        {
            public HmacMd5TestHash(byte[] secret) : base(secret, null)
            {
            }
        }

        [Fact]
        public void ComputeHash_ShouldReturnExpectedHmacSha256_ForGivenInput()
        {
            var secret = "unittest-secret"u8.ToArray();
            var sut = new HmacSha256TestHash(secret);
            var input = "hello"u8.ToArray();
            var result = sut.ComputeHash(input);

            Assert.True(result.HasValue);

            byte[] expected;
            using (var h = new HMACSHA256(secret))
            {
                expected = h.ComputeHash(input);
            }

            Assert.Equal(expected, result.GetBytes());
        }

        [Fact]
        public void ComputeHash_ShouldReturnExpectedHmacMd5_ForEmptyArray()
        {
            var secret = "another-secret"u8.ToArray();
            var sut = new HmacMd5TestHash(secret);
            var input = Array.Empty<byte>();
            var result = sut.ComputeHash(input);

            Assert.True(result.HasValue);

            byte[] expected;
            using (var h = new HMACMD5(secret))
            {
                expected = h.ComputeHash(input);
            }

            Assert.Equal(expected, result.GetBytes());
        }

        [Fact]
        public void ComputeHash_ShouldThrowArgumentNullException_WhenInputIsNull()
        {
            byte[] secret = null;
            var sut = new HmacSha256TestHash(secret);
            Assert.Throws<ArgumentNullException>(() => sut.ComputeHash(secret));
        }
    }
}
