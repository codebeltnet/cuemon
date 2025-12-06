using System;
using System.Security.Cryptography;
using System.Text;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Security.Cryptography
{
    public class UnkeyedCryptoHashTest : Test
    {
        public UnkeyedCryptoHashTest(ITestOutputHelper output) : base(output)
        {
        }

        private sealed class Sha256TestHash : UnkeyedCryptoHash<SHA256>
        {
            public Sha256TestHash() : base(() => SHA256.Create(), null)
            {
            }
        }

        // This subclass intentionally passes a null initializer to exercise the guard in the base ctor.
        private sealed class NullInitializerHash : UnkeyedCryptoHash<SHA256>
        {
            public NullInitializerHash() : base(null, null)
            {
            }
        }

        [Fact]
        public void Ctor_ShouldThrowArgumentNullException_WhenInitializerIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new NullInitializerHash());
            // The base ctor validates the 'initializer' parameter; ensure param name is propagated.
            Assert.Equal("initializer", ex.ParamName);
        }

        [Fact]
        public void ComputeHash_ShouldReturnExpectedSha256_ForGivenInput()
        {
            var sut = new Sha256TestHash();
            var input = "hello"u8.ToArray();
            var result = sut.ComputeHash(input);

            Assert.True(result.HasValue);

            // Compute expected bytes using the same algorithm to assert correctness.
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
            var sut = new Sha256TestHash();
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
