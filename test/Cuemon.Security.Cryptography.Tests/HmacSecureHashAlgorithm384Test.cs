using System;
using System.Security.Cryptography;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Security.Cryptography
{
    public class HmacSecureHashAlgorithm384Test : Test
    {
        public HmacSecureHashAlgorithm384Test(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void ComputeHash_ShouldReturnExpectedHmacSha384_ForGivenInput()
        {
            var secret = "unittest-secret"u8.ToArray();
            var sut = new HmacSecureHashAlgorithm384(secret, null);
            var input = "hello"u8.ToArray();
            var result = sut.ComputeHash(input);

            Assert.True(result.HasValue);

            byte[] expected;
            using (var h = new HMACSHA384(secret))
            {
                expected = h.ComputeHash(input);
            }

            Assert.Equal(expected, result.GetBytes());
        }

        [Fact]
        public void ComputeHash_ShouldReturnExpectedHmacSha384_ForEmptyArray()
        {
            var secret = "another-secret"u8.ToArray();
            var sut = new HmacSecureHashAlgorithm384(secret, null);
            var input = Array.Empty<byte>();
            var result = sut.ComputeHash(input);

            Assert.True(result.HasValue);

            byte[] expected;
            using (var h = new HMACSHA384(secret))
            {
                expected = h.ComputeHash(input);
            }

            Assert.Equal(expected, result.GetBytes());
        }

        [Fact]
        public void Constructor_ShouldAllowNullSecret()
        {
            byte[] secret = null;
            var sut = Record.Exception(() => new HmacSecureHashAlgorithm384(secret, null));
            Assert.Null(sut);
        }

        [Fact]
        public void ComputeHash_ShouldThrowArgumentNullException_WhenInputIsNull()
        {
            var secret = "somesercret"u8.ToArray();
            var sut = new HmacSecureHashAlgorithm384(secret, null);
            Assert.Throws<ArgumentNullException>(() => sut.ComputeHash((byte[])null));
        }
    }
}
