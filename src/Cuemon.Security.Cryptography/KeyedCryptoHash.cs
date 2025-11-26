using System;
using System.Security.Cryptography;

namespace Cuemon.Security.Cryptography
{
    /// <summary>
    /// Represents the base class from which all implementations of Hash-based Message Authentication Code (HMAC) should derive.
    /// Implements the <see cref="UnkeyedCryptoHash{TAlgorithm}" />
    /// </summary>
    /// <typeparam name="TAlgorithm">The type of the <see cref="KeyedHashAlgorithm"/> to implement.</typeparam>
    /// <seealso cref="ConvertibleOptions" />
    /// <seealso cref="Hash{TOptions}"/>
    public abstract class KeyedCryptoHash<TAlgorithm> : UnkeyedCryptoHash<TAlgorithm> where TAlgorithm : KeyedHashAlgorithm
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyedCryptoHash{TAlgorithm}"/> class.
        /// </summary>
        /// <param name="secret">The secret.</param>
        /// <param name="setup">The setup.</param>
        protected KeyedCryptoHash(byte[] secret, Action<ConvertibleOptions> setup) : base(() => (TAlgorithm)Activator.CreateInstance(typeof(TAlgorithm), secret), setup)
        {
        }

        /// <summary>
        /// Computes the hash value for the specified <see cref="T:byte[]" />.
        /// </summary>
        /// <param name="input">The <see cref="T:byte[]" /> to compute the hash code for.</param>
        /// <returns>A <see cref="HashResult" /> containing the computed hash code of the specified <paramref name="input" />.</returns>
        public override HashResult ComputeHash(byte[] input)
        {
            Validator.ThrowIfNull(input);
            using (var h = Initializer())
            {
                return new HashResult(h.ComputeHash(input));
            }
        }
    }
}