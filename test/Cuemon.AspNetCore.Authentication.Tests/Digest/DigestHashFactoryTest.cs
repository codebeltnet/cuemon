using System;
using Cuemon.Security.Cryptography;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.AspNetCore.Authentication.Digest
{
    public class DigestHashFactoryTest : Test
    {
        public DigestHashFactoryTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void CreateCrypto_ShouldDefaultToSha256()
        {
            var sut = DigestHashFactory.CreateCrypto();
            var expected = UnkeyedHashFactory.CreateCrypto(UnkeyedCryptoAlgorithm.Sha256);

            Assert.Equal(expected.GetType(), sut.GetType());
        }

        [Theory]
        [InlineData(DigestCryptoAlgorithm.Md5, UnkeyedCryptoAlgorithm.Md5)]
        [InlineData(DigestCryptoAlgorithm.Md5Session, UnkeyedCryptoAlgorithm.Md5)]
        [InlineData(DigestCryptoAlgorithm.Sha256, UnkeyedCryptoAlgorithm.Sha256)]
        [InlineData(DigestCryptoAlgorithm.Sha256Session, UnkeyedCryptoAlgorithm.Sha256)]
        [InlineData(DigestCryptoAlgorithm.Sha512Slash256, UnkeyedCryptoAlgorithm.Sha512Slash256)]
        [InlineData(DigestCryptoAlgorithm.Sha512Slash256Session, UnkeyedCryptoAlgorithm.Sha512Slash256)]
        public void CreateCrypto_ShouldMapDigestAlgorithmsToExpectedHashImplementations(DigestCryptoAlgorithm algorithm, UnkeyedCryptoAlgorithm expectedAlgorithm)
        {
            var sut = DigestHashFactory.CreateCrypto(algorithm);
            var expected = UnkeyedHashFactory.CreateCrypto(expectedAlgorithm);

            Assert.Equal(expected.GetType(), sut.GetType());
        }

        [Fact]
        public void CreateCrypto_ShouldThrowArgumentOutOfRangeException_WhenAlgorithmIsUnsupported()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => DigestHashFactory.CreateCrypto((DigestCryptoAlgorithm)42));
        }
    }
}
