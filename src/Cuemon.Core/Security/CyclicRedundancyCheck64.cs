using System;

namespace Cuemon.Security
{
    /// <summary>
    /// Provides a CRC-64 implementation of the CRC (Cyclic Redundancy Check) checksum algorithm for 64-bit hash values. This class cannot be inherited.
    /// </summary>
    /// Implements the <see cref="CyclicRedundancyCheck" />
    /// <seealso cref="CyclicRedundancyCheck"/>
    public sealed class CyclicRedundancyCheck64 : CyclicRedundancyCheck
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CyclicRedundancyCheck64"/> class.
        /// </summary>
        /// <param name="polynomial">This is a binary value that should be specified as a hexadecimal number. Default is <c>0x42F0E1EBA9EA3693</c>.</param>
        /// <param name="initialValue">This parameter specifies the initial value of the register when the algorithm starts. Default is <c>0x0000000000000000</c>.</param>
        /// <param name="finalXor">This is an W-bit value that should be specified as a hexadecimal number. Default is <c>0x0000000000000000</c>.</param>
        /// <param name="setup">The <see cref="CyclicRedundancyCheckOptions" /> which may be configured.</param>
        public CyclicRedundancyCheck64(ulong polynomial = 0x42F0E1EBA9EA3693, ulong initialValue = 0x0000000000000000, ulong finalXor = 0x0000000000000000, Action<CyclicRedundancyCheckOptions> setup = null) : base(polynomial, initialValue, finalXor, setup)
        {
        }

        /// <summary>
        /// Returns the initial value for the specified <paramref name="index" /> of the polynomial <see cref="CyclicRedundancyCheck.LookupTable" />.
        /// </summary>
        /// <param name="index">The index of the array of polynomial values ranging from 0 to 255.</param>
        /// <returns>The initial value for the specified <paramref name="index" />.</returns>
        protected override ulong PolynomialIndexInitializer(byte index)
        {
            return ((ulong)index << 56);
        }

        /// <summary>
        /// Computes the polynomial <paramref name="checksum" /> value in steps of 8 (0-7) x 256 (0-255) giving a total of 2048 times.
        /// </summary>
        /// <param name="checksum">The checksum that is iterated 8 times (0-7) in 256 steps (0-255).</param>
        /// <param name="polynomial">The polynomial value.</param>
        protected override void PolynomialSlotCalculator(ref ulong checksum, ulong polynomial)
        {
            if ((checksum & 0x8000000000000000) != 0)
            {
                checksum <<= 1;
                checksum ^= polynomial;
            }
            else
            {
                checksum <<= 1;
            }
        }

        /// <summary>
        /// Computes the hash value for the specified <see cref="T:byte[]" />.
        /// </summary>
        /// <param name="input">The <see cref="T:byte[]" /> to compute the hash code for.</param>
        /// <returns>A <see cref="HashResult" /> containing the computed hash code of the specified <paramref name="input" />.</returns>
        /// <remarks>Inspiration and praises goes to http://www.sunshine2k.de/articles/coding/crc/understanding_crc.html</remarks>
        public override HashResult ComputeHash(byte[] input)
        {
            Validator.ThrowIfNull(input);

            var crc = InitialValue;
            for (var i = 0; i < input.Length; i++)
            {
                var cb = Options.ReflectInput ? Convertible.ReverseBits8(input[i]) : input[i];
                crc ^= ((ulong)cb << 56);
                var index = (byte)(crc >> 56);
                crc <<= 8;
                crc ^= LookupTable[index];
            }
            crc = Options.ReflectOutput ? Convertible.ReverseBits64(crc) : crc;
            crc ^= FinalXor;
            return new HashResult(Convertible.GetBytes(crc, o => o.ByteOrder = Options.ByteOrder));
        }
    }
}