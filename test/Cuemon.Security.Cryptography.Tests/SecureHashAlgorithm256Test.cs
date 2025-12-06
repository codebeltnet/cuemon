using System;
using System.Security.Cryptography;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Security.Cryptography
{
    public class SecureHashAlgorithm256Test : Test
    {
        public SecureHashAlgorithm256Test(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void BitSize_ShouldBe256()
        {
            Assert.Equal(256, SecureHashAlgorithm256.BitSize);
        }

        [Fact]
        public void Ctor_ShouldConfigureOptions_WhenSetupIsProvided()
        {
            var sut = new SecureHashAlgorithm1(o => o.ByteOrder = Endianness.BigEndian);

            Assert.NotNull(sut.Options);
            Assert.Equal(Endianness.BigEndian, sut.Options.ByteOrder);
        }

        [Fact]
        public void ComputeHash_ShouldReturnExpectedSha256_ForGivenInput()
        {
            var sut = new SecureHashAlgorithm256();
            var input = "hello"u8.ToArray();
            var result = sut.ComputeHash(input);

            Assert.True(result.HasValue);

            byte[] expected;
            using (var sha = SHA256.Create())
            {
                expected = sha.ComputeHash(input);
            }

            Assert.Equal(expected, result.GetBytes());
        }

        [Fact]
        public void ComputeHash_ShouldReturnExpectedSha256_ForEmptyArray()
        {
            var sut = new SecureHashAlgorithm256();
            var input = Array.Empty<byte>();
            var result = sut.ComputeHash(input);

            Assert.True(result.HasValue);

            byte[] expected;
            using (var sha = SHA256.Create())
            {
                expected = sha.ComputeHash(input);
            }

            Assert.Equal(expected, result.GetBytes());
        }
    }
}
