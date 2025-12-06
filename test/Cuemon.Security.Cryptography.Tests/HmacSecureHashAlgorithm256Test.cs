using System;
using System.Security.Cryptography;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Security.Cryptography
{
    public class HmacSecureHashAlgorithm256Test : Test
    {
        public HmacSecureHashAlgorithm256Test(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void ComputeHash_ShouldReturnExpectedHmacSha256_ForGivenInput()
        {
            var secret = "unittest-secret"u8.ToArray();
            var sut = new HmacSecureHashAlgorithm256(secret, null);
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
        public void ComputeHash_ShouldReturnExpectedHmacSha256_ForEmptyArray()
        {
            var secret = "another-secret"u8.ToArray();
            var sut = new HmacSecureHashAlgorithm256(secret, null);
            var input = Array.Empty<byte>();
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
        public void Constructor_ShouldAllowNullSecret()
        {
            byte[] secret = null;
            var sut = Record.Exception(() => new HmacSecureHashAlgorithm256(secret, null));
            Assert.Null(sut);
        }

        [Fact]
        public void ComputeHash_ShouldThrowArgumentNullException_WhenInputIsNull()
        {
            var secret = "somesercret"u8.ToArray();
            var sut = new HmacSecureHashAlgorithm256(secret, null);
            Assert.Throws<ArgumentNullException>(() => sut.ComputeHash((byte[])null));
        }
    }
}
