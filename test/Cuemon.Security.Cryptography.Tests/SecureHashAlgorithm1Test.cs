using System;
using System.Security.Cryptography;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Security.Cryptography
{
    public class SecureHashAlgorithm1Test : Test
    {
        public SecureHashAlgorithm1Test(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void BitSize_ShouldBe160()
        {
            Assert.Equal(160, SecureHashAlgorithm1.BitSize);
        }

        [Fact]
        public void Ctor_ShouldConfigureOptions_WhenSetupIsProvided()
        {
            var sut = new SecureHashAlgorithm1(o => o.ByteOrder = Endianness.BigEndian);

            Assert.NotNull(sut.Options);
            Assert.Equal(Endianness.BigEndian, sut.Options.ByteOrder);
        }

        [Fact]
        public void ComputeHash_ShouldReturnExpectedSha1_ForGivenInput()
        {
            var sut = new SecureHashAlgorithm1();
            var input = "hello"u8.ToArray();
            var result = sut.ComputeHash(input);

            Assert.True(result.HasValue);

            byte[] expected;
            using (var sha = SHA1.Create())
            {
                expected = sha.ComputeHash(input);
            }

            Assert.Equal(expected, result.GetBytes());
        }

        [Fact]
        public void ComputeHash_ShouldReturnExpectedSha1_ForEmptyArray()
        {
            var sut = new SecureHashAlgorithm1();
            var input = Array.Empty<byte>();
            var result = sut.ComputeHash(input);

            Assert.True(result.HasValue);

            byte[] expected;
            using (var sha = SHA1.Create())
            {
                expected = sha.ComputeHash(input);
            }

            Assert.Equal(expected, result.GetBytes());
        }
    }
}
