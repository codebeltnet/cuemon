using System;
using System.Security.Cryptography;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Security.Cryptography
{
    public class SecureHashAlgorithm512Test : Test
    {
        public SecureHashAlgorithm512Test(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void BitSize_ShouldBe512()
        {
            Assert.Equal(512, SecureHashAlgorithm512.BitSize);
        }

        [Fact]
        public void Ctor_ShouldConfigureOptions_WhenSetupIsProvided()
        {
            var sut = new SecureHashAlgorithm512(o => o.ByteOrder = Endianness.BigEndian);

            Assert.NotNull(sut.Options);
            Assert.Equal(Endianness.BigEndian, sut.Options.ByteOrder);
        }

        [Fact]
        public void ComputeHash_ShouldReturnExpectedSha512_ForGivenInput()
        {
            var sut = new SecureHashAlgorithm512();
            var input = "hello"u8.ToArray();
            var result = sut.ComputeHash(input);

            Assert.True(result.HasValue);

            byte[] expected;
            using (var sha = SHA512.Create())
            {
                expected = sha.ComputeHash(input);
            }

            Assert.Equal(expected, result.GetBytes());
        }

        [Fact]
        public void ComputeHash_ShouldReturnExpectedSha512_ForEmptyArray()
        {
            var sut = new SecureHashAlgorithm512();
            var input = Array.Empty<byte>();
            var result = sut.ComputeHash(input);

            Assert.True(result.HasValue);

            byte[] expected;
            using (var sha = SHA512.Create())
            {
                expected = sha.ComputeHash(input);
            }

            Assert.Equal(expected, result.GetBytes());
        }
    }
}
