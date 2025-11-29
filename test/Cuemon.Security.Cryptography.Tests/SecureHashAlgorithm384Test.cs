using System;
using System.Security.Cryptography;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Security.Cryptography
{
    public class SecureHashAlgorithm384Test : Test
    {
        public SecureHashAlgorithm384Test(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void BitSize_ShouldBe384()
        {
            Assert.Equal(384, SecureHashAlgorithm384.BitSize);
        }

        [Fact]
        public void Ctor_ShouldConfigureOptions_WhenSetupIsProvided()
        {
            var sut = new SecureHashAlgorithm384(o => o.ByteOrder = Endianness.BigEndian);

            Assert.NotNull(sut.Options);
            Assert.Equal(Endianness.BigEndian, sut.Options.ByteOrder);
        }

        [Fact]
        public void ComputeHash_ShouldReturnExpectedSha384_ForGivenInput()
        {
            var sut = new SecureHashAlgorithm384();
            var input = "hello"u8.ToArray();
            var result = sut.ComputeHash(input);

            Assert.True(result.HasValue);

            byte[] expected;
            using (var sha = SHA384.Create())
            {
                expected = sha.ComputeHash(input);
            }

            Assert.Equal(expected, result.GetBytes());
        }

        [Fact]
        public void ComputeHash_ShouldReturnExpectedSha384_ForEmptyArray()
        {
            var sut = new SecureHashAlgorithm384();
            var input = Array.Empty<byte>();
            var result = sut.ComputeHash(input);

            Assert.True(result.HasValue);

            byte[] expected;
            using (var sha = SHA384.Create())
            {
                expected = sha.ComputeHash(input);
            }

            Assert.Equal(expected, result.GetBytes());
        }
    }
}
