    using System;
using System.Security.Cryptography;
using System.Text;
using Cuemon.Text;

namespace Cuemon.Security.Cryptography
{
    /// <summary>
    /// Provides an implementation of the Advanced Encryption Standard (AES) symmetric algorithm.
    /// </summary>
    public class AesCryptor
    {
        /// <summary>
        /// Gets the block size (bits) for the Advanced Encryption Standard (AES) symmetric algorithm.
        /// </summary>
        public const byte BlockSize = 128;

        /// <summary>
        /// Initializes a new instance of the <see cref="AesCryptor"/> class.
        /// </summary>
        public AesCryptor() : this(GenerateKey(), GenerateInitializationVector())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AesCryptor"/> class.
        /// </summary>
        /// <param name="key">The secret key of this instance.</param>
        /// <param name="initializationVector">The initialization vector (IV) of this instance.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="key"/> cannot be null -or-
        /// <paramref name="initializationVector"/> cannot be null.
        /// </exception>
        /// <exception cref="CryptographicException">
        /// <paramref name="key"/> does not meet the required fixed size of either 128 bits, 192 bits or 256 bits -or-
        /// <paramref name="initializationVector"/> does not meet the required fixed size of 128 bits.
        /// </exception>
        public AesCryptor(byte[] key, byte[] initializationVector)
        {
            Validator.ThrowIfNull(key);
            Validator.ThrowIfNull(initializationVector);
            var keyBits = key.Length * Convertible.BitsPerByte;
            var initializationVectorBits = initializationVector.Length * Convertible.BitsPerByte;
            if (!(keyBits == 128 || keyBits == 192 || keyBits == 256)) { throw new CryptographicException("The key does not meet the required fixed size of either 128 bits, 192 bits or 256 bits."); }
            if (initializationVectorBits != BlockSize) { throw new CryptographicException("The initialization vector does not meet the required fixed size of 128 bits."); }
            Key = key;
            InitializationVector = initializationVector;
        }

        /// <summary>
        /// Gets the secret key of this instance.
        /// </summary>
        /// <value>The secret key of this instance.</value>
        public byte[] Key { get; }

        /// <summary>
        /// Gets the initialization vector (IV) of this instance.
        /// </summary>
        /// <value>The initialization vector (IV) of this instance.</value>
        public byte[] InitializationVector { get; }

        /// <summary>
        /// Encrypts the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value to encrypt.</param>
        /// <param name="setup">The <see cref="AesCryptorOptions" /> which may be configured.</param>
        /// <returns>The encrypted value.</returns>
        public byte[] Encrypt(byte[] value, Action<AesCryptorOptions> setup = null)
        {
            return CryptoTransformCore(value, AesMode.Encrypt, setup);
        }

        /// <summary>
        /// Decrypts the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The encrypted value that needs to be decrypted.</param>
        /// <param name="setup">The <see cref="AesCryptorOptions" /> which may be configured.</param>
        /// <returns>The decrypted value.</returns>
        public byte[] Decrypt(byte[] value, Action<AesCryptorOptions> setup = null)
        {
            return CryptoTransformCore(value, AesMode.Decrypt, setup);
        }

        private byte[] CryptoTransformCore(byte[] value, AesMode mode, Action<AesCryptorOptions> setup)
        {
            var options = Patterns.Configure(setup);

            using (var aes = Aes.Create())
            {
                aes.BlockSize = BlockSize;
                aes.Key = Key;
                aes.IV = InitializationVector;
                aes.Padding = options.Padding;
                aes.Mode = options.Mode;

                using (var transform = mode == AesMode.Encrypt
                           ? aes.CreateEncryptor()
                           : aes.CreateDecryptor())
                {
                    return transform.TransformFinalBlock(value, 0, value.Length);
                }
            }
        }

        /// <summary>
        /// Generates a random 128 bit initialization vector (IV).
        /// </summary>
        /// <returns>A random 128 bit generated initialization vector (IV).</returns>
        public static byte[] GenerateInitializationVector()
        {
            return Convertible.GetBytes(Generate.RandomString(BlockSize / Convertible.BitsPerByte, Alphanumeric.LettersAndNumbers, Alphanumeric.PunctuationMarks), options =>
            {
                options.Encoding = Encoding.UTF8;
                options.Preamble = PreambleSequence.Remove;
            });
        }

        /// <summary>
        /// Generates a secret key from the options defined in <paramref name="setup"/>.
        /// </summary>
        /// <param name="setup">The <see cref="AesKeyOptions" /> which may be configured.</param>
        /// <returns>A secret key from the options defined in <paramref name="setup"/>.</returns>
        public static byte[] GenerateKey(Action<AesKeyOptions> setup = null)
        {
            var options = Patterns.Configure(setup);
            return Convertible.GetBytes(options.RandomStringProvider(options.Size), o =>
            {
                o.Encoding = Encoding.UTF8;
                o.Preamble = PreambleSequence.Remove;
            });
        }

        private enum AesMode
        {
            Encrypt,
            Decrypt
        }
    }
}
