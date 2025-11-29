using System;
using System.Security.Cryptography;

namespace Cuemon.Security.Cryptography
{
    /// <summary>
    /// Represents the base class from which all implementations of cryptographic hashing algorithm should derive.
    /// Implements the <see cref="Hash{ConvertibleOptions}" />
    /// </summary>
    /// <typeparam name="TAlgorithm">The type of the <see cref="HashAlgorithm"/> to implement.</typeparam>
    /// <seealso cref="ConvertibleOptions" />
    public abstract class UnkeyedCryptoHash<TAlgorithm> : Hash<ConvertibleOptions> where TAlgorithm : HashAlgorithm
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnkeyedCryptoHash{TAlgorithm}"/> class.
        /// </summary>
        /// <param name="initializer">The function delegate that will initialize an instance of <see cref="HashAlgorithm"/> derived class.</param>
        /// <param name="setup">The <see cref="ConvertibleOptions"/> which need to be configured.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="initializer"/> cannot be null.
        /// </exception>
        protected UnkeyedCryptoHash(Func<TAlgorithm> initializer, Action<ConvertibleOptions> setup) : base(setup)
        {
            Validator.ThrowIfNull(initializer);
            Initializer = initializer;
        }

        /// <summary>
        /// Gets the function delegate responsible for initializing an instance of a <see cref="HashAlgorithm"/> derived class.
        /// </summary>
        /// <value>The function delegate responsible for initializing an instance of a <see cref="HashAlgorithm"/> derived class.</value>
        protected Func<TAlgorithm> Initializer { get; }

        /// <summary>
        /// Computes the hash value for the specified <see cref="T:byte[]" />.
        /// </summary>
        /// <param name="input">The <see cref="T:byte[]" /> to compute the hash code for.</param>
        /// <returns>A <see cref="HashResult" /> containing the computed hash code of the specified <paramref name="input" />.</returns>
        public override HashResult ComputeHash(byte[] input)
        {
            Validator.ThrowIfNull(input);
            using var h = Initializer();
            return new HashResult(h.ComputeHash(input));
        }
    }
}
