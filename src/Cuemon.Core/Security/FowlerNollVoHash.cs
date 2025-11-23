using System;
using System.Numerics;

namespace Cuemon.Security
{
    /// <summary>
    /// Represents the base class from which all implementations of the Fowler–Noll–Vo non-cryptographic hashing algorithm must derive.
    /// </summary>
    public abstract class FowlerNollVoHash : Hash<FowlerNollVoOptions>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FowlerNollVoHash"/> class.
        /// </summary>
        /// <param name="bits">The size in bits.</param>
        /// <param name="prime">The prime number of the algorithm.</param>
        /// <param name="offsetBasis">The initial value of the hash.</param>
        /// <param name="setup">The <see cref="FowlerNollVoOptions"/> which need to be configured.</param>
        protected FowlerNollVoHash(short bits, BigInteger prime, BigInteger offsetBasis, Action<FowlerNollVoOptions> setup) : base(setup)
        {
            switch (bits)
            {
                case 32:
                case 64:
                case 128:
                case 256:
                case 512:
                case 1024:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(bits), bits, $"Unsupported Fowler–Noll–Vo hash size: {bits}. Supported sizes are: 32, 64, 128, 256, 512, 1024 bits.");
            }
            Bits = bits;
            Prime = prime;
            OffsetBasis = offsetBasis;
        }

        /// <summary>
        /// Gets the prime number of the algorithm.
        /// </summary>
        /// <value>The prime number of the algorithm.</value>
        public BigInteger Prime { get; }

        /// <summary>
        /// Gets the offset basis used as the initial value of the hash.
        /// </summary>
        /// <value>The offset basis used as the initial value of the hash.</value>
        public BigInteger OffsetBasis { get; }

        /// <summary>
        /// Gets the size of the implementation in bits.
        /// </summary>
        /// <value>The size of the implementation in bits.</value>
        public short Bits { get; }

        /// <summary>
        /// Computes the hash value for the specified <see cref="T:byte[]" />.
        /// </summary>
        /// <param name="input">The <see cref="T:byte[]" /> to compute the hash code for.</param>
        /// <returns>A <see cref="HashResult" /> containing the computed hash code of the specified <paramref name="input" />.</returns>
        public override HashResult ComputeHash(byte[] input)
        {
            Validator.ThrowIfNull(input);
            if (Bits == 32) { return ComputeHash32(input, (uint)Prime, (uint)OffsetBasis, Options); }
            if (Bits == 64) { return ComputeHash64(input, (ulong)Prime, (ulong)OffsetBasis, Options); }
            if (Bits > 64 && (Bits % 32) == 0) { return ComputeHashMultiWord(input, Prime, OffsetBasis, Bits, Options); }
            throw new InvalidOperationException($"Unsupported Fowler–Noll–Vo hash size: {Bits}. Supported sizes are: 32, 64, 128, 256, 512, 1024 bits.");
        }

        private static HashResult ComputeHash32(byte[] input, uint prime, uint offsetBasis, FowlerNollVoOptions options)
        {
            unchecked
            {
                if (options.Algorithm == FowlerNollVoAlgorithm.Fnv1a)
                {
                    for (int i = 0; i < input.Length; i++)
                    {
                        offsetBasis ^= input[i];
                        offsetBasis *= prime;
                    }
                }
                else
                {
                    for (int i = 0; i < input.Length; i++)
                    {
                        offsetBasis *= prime;
                        offsetBasis ^= input[i];
                    }
                }

                var result = new byte[4];
                if (options.ByteOrder == Endianness.LittleEndian)
                {
                    result[0] = (byte)offsetBasis;
                    result[1] = (byte)(offsetBasis >> 8);
                    result[2] = (byte)(offsetBasis >> 16);
                    result[3] = (byte)(offsetBasis >> 24);
                }
                else
                {
                    result[3] = (byte)offsetBasis;
                    result[2] = (byte)(offsetBasis >> 8);
                    result[1] = (byte)(offsetBasis >> 16);
                    result[0] = (byte)(offsetBasis >> 24);
                }

                return new HashResult(result);
            }
        }

        private static HashResult ComputeHash64(byte[] input, ulong prime, ulong offsetBasis, FowlerNollVoOptions options)
        {
            unchecked
            {
                if (options.Algorithm == FowlerNollVoAlgorithm.Fnv1a)
                {
                    for (int i = 0; i < input.Length; i++)
                    {
                        offsetBasis ^= input[i];
                        offsetBasis *= prime;
                    }
                }
                else
                {
                    for (int i = 0; i < input.Length; i++)
                    {
                        offsetBasis *= prime;
                        offsetBasis ^= input[i];
                    }
                }

                var result = new byte[8];
                if (options.ByteOrder == Endianness.LittleEndian)
                {
                    result[0] = (byte)offsetBasis;
                    result[1] = (byte)(offsetBasis >> 8);
                    result[2] = (byte)(offsetBasis >> 16);
                    result[3] = (byte)(offsetBasis >> 24);
                    result[4] = (byte)(offsetBasis >> 32);
                    result[5] = (byte)(offsetBasis >> 40);
                    result[6] = (byte)(offsetBasis >> 48);
                    result[7] = (byte)(offsetBasis >> 56);
                }
                else
                {
                    result[7] = (byte)offsetBasis;
                    result[6] = (byte)(offsetBasis >> 8);
                    result[5] = (byte)(offsetBasis >> 16);
                    result[4] = (byte)(offsetBasis >> 24);
                    result[3] = (byte)(offsetBasis >> 32);
                    result[2] = (byte)(offsetBasis >> 40);
                    result[1] = (byte)(offsetBasis >> 48);
                    result[0] = (byte)(offsetBasis >> 56);
                }

                return new HashResult(result);
            }
        }

        private static HashResult ComputeHashMultiWord(byte[] input, BigInteger prime, BigInteger offsetBasis, short bits, FowlerNollVoOptions options)
        {
            int unitCount = bits / 32;
            var p = ToUInt32LittleEndian(prime, unitCount);
            var a = ToUInt32LittleEndian(offsetBasis, unitCount);
            var tmp = new uint[unitCount];

            if (options.Algorithm == FowlerNollVoAlgorithm.Fnv1a)
            {
                for (int idx = 0; idx < input.Length; idx++)
                {
                    a[0] ^= input[idx];
                    MultiplyMod32(a, p, tmp);
                }
            }
            else
            {
                for (int idx = 0; idx < input.Length; idx++)
                {
                    MultiplyMod32(a, p, tmp);
                    a[0] ^= input[idx];
                }
            }

            var bytes = new byte[unitCount * 4];
            for (int i = 0; i < unitCount; i++)
            {
                int j = i * 4;
                var v = a[i];
                bytes[j] = (byte)(v & 0xFF);
                bytes[j + 1] = (byte)((v >> 8) & 0xFF);
                bytes[j + 2] = (byte)((v >> 16) & 0xFF);
                bytes[j + 3] = (byte)((v >> 24) & 0xFF);
            }

            var resultBytes = Convertible.ReverseEndianness(bytes, o => o.ByteOrder = options.ByteOrder);
            return new HashResult(resultBytes);
        }

        private static uint[] ToUInt32LittleEndian(BigInteger value, int unitCount)
        {
            var bytes = value.ToByteArray();
            var needed = unitCount * 4;
            if (bytes.Length < needed)
            {
                var extended = new byte[needed];
                Array.Copy(bytes, extended, bytes.Length);
                bytes = extended;
            }
            var res = new uint[unitCount];
            for (int i = 0; i < unitCount; i++)
            {
                int j = i * 4;
                res[i] = (uint)(bytes[j] | (bytes[j + 1] << 8) | (bytes[j + 2] << 16) | (bytes[j + 3] << 24));
            }
            return res;
        }

        private static void MultiplyMod32(uint[] a, uint[] p, uint[] tmp)
        {
            int L = a.Length;
            for (int k = 0; k < L; k++) tmp[k] = 0u;

            for (int i = 0; i < L; i++)
            {
                if (a[i] == 0) continue;
                ulong carry = 0UL;
                for (int j = 0; j < L - i; j++)
                {
                    int k = i + j;
                    ulong acc = tmp[k];
                    acc += (ulong)a[i] * p[j] + carry;
                    tmp[k] = (uint)acc;
                    carry = acc >> 32;
                }
            }

            for (int i = 0; i < L; i++) a[i] = tmp[i];
        }
    }
}
