using System;
using System.Security.Cryptography;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Security.Cryptography
{
    public class MessageDigest5Test : Test
    {
        public MessageDigest5Test(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void BitSize_ShouldBe128()
        {
            Assert.Equal(128, MessageDigest5.BitSize);
        }

        [Fact]
        public void ComputeHash_ShouldReturnExpectedMd5_ForGivenInput()
        {
            var sut = new MessageDigest5(null);
            var input = "hello"u8.ToArray();
            var result = sut.ComputeHash(input);

            Assert.True(result.HasValue);

            byte[] expected;
            using (var md5 = MD5.Create())
            {
                expected = md5.ComputeHash(input);
            }

            Assert.Equal(expected, result.GetBytes());
        }

        [Fact]
        public void ComputeHash_ShouldReturnExpectedMd5_ForEmptyArray()
        {
            var sut = new MessageDigest5(null);
            var input = Array.Empty<byte>();
            var result = sut.ComputeHash(input);

            Assert.True(result.HasValue);

            byte[] expected;
            using (var md5 = MD5.Create())
            {
                expected = md5.ComputeHash(input);
            }

            Assert.Equal(expected, result.GetBytes());
        }

        [Fact]
        public void Constructor_ShouldAllowNullSetup()
        {
            var ex = Record.Exception(() => new MessageDigest5(null));
            Assert.Null(ex);
        }

        [Fact]
        public void ComputeHash_ShouldThrowArgumentNullException_WhenInputIsNull()
        {
            var sut = new MessageDigest5(null);
            Assert.Throws<ArgumentNullException>(() => sut.ComputeHash((byte[])null));
        }
    }
}
