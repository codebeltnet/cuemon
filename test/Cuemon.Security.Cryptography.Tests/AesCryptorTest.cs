using Codebelt.Extensions.Xunit;
using System;
using System.Security.Cryptography;
using System.Text;
using Xunit;

namespace Cuemon.Security.Cryptography
{
    public class AesCryptorTest : Test
    {
        private readonly byte[] _secretKey;
        private readonly byte[] _iv;

        public AesCryptorTest(ITestOutputHelper output) : base(output)
        {
            _secretKey = AesCryptor.GenerateKey();
            _iv = AesCryptor.GenerateInitializationVector();
        }

        [Fact]
        public void AesCryptor_ShouldEncryptAndDecrypt()
        {
            var cryptor = new AesCryptor(_secretKey, _iv);
            var secretMessage = Decorator.Enclose("This is my secret message that needs encryption!").ToByteArray();

            Assert.True(_secretKey.SequenceEqual(cryptor.Key));
            Assert.True(_iv.SequenceEqual(cryptor.InitializationVector));

            var enc = cryptor.Encrypt(secretMessage);
            TestOutput.WriteLine(Convert.ToBase64String(enc));

            var dec = cryptor.Decrypt(enc);
            TestOutput.WriteLine(Convert.ToBase64String(dec));

            Assert.True(dec.SequenceEqual(secretMessage));
        }

        [Fact]
        public void DefaultConstructor_ShouldProduceValidKeyAndIvLengths()
        {
            var sut = new AesCryptor();

            Assert.NotNull(sut.Key);
            Assert.NotNull(sut.InitializationVector);

            // Default GenerateKey uses Aes256 => 256 bits => 32 bytes
            Assert.Equal(256 / Convertible.BitsPerByte, sut.Key.Length);
            // BlockSize is 128 bits => 16 bytes
            Assert.Equal(AesCryptor.BlockSize / Convertible.BitsPerByte, sut.InitializationVector.Length);
        }

        [Fact]
        public void ParameterizedConstructor_ShouldSetProperties()
        {
            using var aes = Aes.Create();
            var key = aes.Key;
            var iv = aes.IV;

            var sut = new AesCryptor(key, iv);

            Assert.Same(key, sut.Key);
            Assert.Same(iv, sut.InitializationVector);
        }

        [Fact]
        public void Constructor_WithNullKey_ShouldThrowArgumentNullException()
        {
            var iv = AesCryptor.GenerateInitializationVector();
            Assert.Throws<ArgumentNullException>(() => new AesCryptor(null!, iv));
        }

        [Fact]
        public void Constructor_WithNullIv_ShouldThrowArgumentNullException()
        {
            var key = AesCryptor.GenerateKey();
            Assert.Throws<ArgumentNullException>(() => new AesCryptor(key, null!));
        }

        [Fact]
        public void Constructor_WithInvalidKeySize_ShouldThrowCryptographicException()
        {
            var invalidKey = new byte[10]; // not 128/192/256 bits
            var iv = AesCryptor.GenerateInitializationVector();

            var ex = Assert.Throws<CryptographicException>(() => new AesCryptor(invalidKey, iv));
            Assert.Contains("The key does not meet the required fixed size", ex.Message);
        }

        [Fact]
        public void Constructor_WithInvalidIvSize_ShouldThrowCryptographicException()
        {
            using var aes = Aes.Create();
            var key = aes.Key;
            var invalidIv = new byte[8]; // not 128 bits

            var ex = Assert.Throws<CryptographicException>(() => new AesCryptor(key, invalidIv));
            Assert.Contains("The initialization vector does not meet the required fixed size of 128 bits.", ex.Message);
        }

        [Fact]
        public void EncryptAndDecrypt_ShouldReturnOriginalPayload()
        {
            using var aes = Aes.Create();
            var key = aes.Key;
            var iv = aes.IV;

            var sut = new AesCryptor(key, iv);

            var plain = Encoding.UTF8.GetBytes("Hello, Cuemon! Encryption round-trip test.");
            var encrypted = sut.Encrypt(plain);
            Assert.NotNull(encrypted);
            Assert.NotEmpty(encrypted);
            Assert.False(plain.SequenceEqual(encrypted));

            var decrypted = sut.Decrypt(encrypted);
            Assert.NotNull(decrypted);
            Assert.Equal(plain, decrypted);
        }

        [Fact]
        public void EncryptAndDecrypt_WithOptionsDelegate_ShouldReturnOriginalPayload()
        {
            using var aes = Aes.Create();
            var key = aes.Key;
            var iv = aes.IV;

            var sut = new AesCryptor(key, iv);

            var plain = Encoding.UTF8.GetBytes("Hello, Cuemon! Options overload test.");
            // provide explicit options (same as defaults) to ensure the overload code path runs
            var encrypted = sut.Encrypt(plain, o => { o.Mode = System.Security.Cryptography.CipherMode.CBC; o.Padding = System.Security.Cryptography.PaddingMode.PKCS7; });
            Assert.NotNull(encrypted);
            Assert.NotEmpty(encrypted);

            var decrypted = sut.Decrypt(encrypted, o => { o.Mode = System.Security.Cryptography.CipherMode.CBC; o.Padding = System.Security.Cryptography.PaddingMode.PKCS7; });
            Assert.Equal(plain, decrypted);
        }

        [Fact]
        public void GenerateInitializationVector_ShouldReturn16Bytes_AndDifferentValues()
        {
            var iv1 = AesCryptor.GenerateInitializationVector();
            var iv2 = AesCryptor.GenerateInitializationVector();

            Assert.NotNull(iv1);
            Assert.NotNull(iv2);
            Assert.Equal(AesCryptor.BlockSize / Convertible.BitsPerByte, iv1.Length);
            Assert.Equal(AesCryptor.BlockSize / Convertible.BitsPerByte, iv2.Length);

            // Very small chance of collision; assert that typical calls produce different values
            Assert.False(iv1.SequenceEqual(iv2));
        }

        [Fact]
        public void GenerateKey_DefaultAndCustomSizes_ShouldReturnExpectedLengths()
        {
            // default (Aes256)
            var defaultKey = AesCryptor.GenerateKey();
            Assert.NotNull(defaultKey);
            Assert.Equal(256 / Convertible.BitsPerByte, defaultKey.Length);

            // Aes128
            var key128 = AesCryptor.GenerateKey(o => o.Size = AesSize.Aes128);
            Assert.NotNull(key128);
            Assert.Equal(128 / Convertible.BitsPerByte, key128.Length);

            // Custom RandomStringProvider returning predictable string -> expected bytes length
            var custom = AesCryptor.GenerateKey(o =>
            {
                o.Size = AesSize.Aes128;
                o.RandomStringProvider = size => new string('x', 128 / Convertible.BitsPerByte);
            });
            Assert.Equal(128 / Convertible.BitsPerByte, custom.Length);
        }
    }
}