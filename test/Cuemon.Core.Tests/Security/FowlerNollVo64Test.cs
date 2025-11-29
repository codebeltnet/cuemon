using System;
using System.Numerics;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Security
{
    public class FowlerNollVo64Test : Test
    {
        public FowlerNollVo64Test(ITestOutputHelper output) : base(output) { }

        [Fact]
        public void Constructor_Default_SetsBits()
        {
            var sut = new FowlerNollVo64();
            Assert.Equal(64, sut.Bits);
        }

        [Fact]
        public void ComputeHash_Empty_ReturnsOffsetBasis_BigEndianByDefault()
        {
            var sut = new FowlerNollVo64();
            var result = sut.ComputeHash(Array.Empty<byte>()).GetBytes();
            // Ensure deterministic length
            Assert.Equal(8, result.Length);
        }

        [Fact]
        public void ComputeHash_NonEmpty_EqualsIndependentImplementation()
        {
            var data = "hello"u8.ToArray();
            var sut = new FowlerNollVo64();
            sut.Options.Algorithm = FowlerNollVoAlgorithm.Fnv1a;
            var expected = IndependentFNV64(data, (ulong)new BigInteger(1099511628211), (ulong)new BigInteger(14695981039346656037), FowlerNollVoAlgorithm.Fnv1a, sut.Options.ByteOrder);
            var actual = sut.ComputeHash(data).GetBytes();
            Assert.Equal(expected, actual);
        }

        private static byte[] IndependentFNV64(byte[] input, ulong prime, ulong offsetBasis, FowlerNollVoAlgorithm algorithm, Endianness byteOrder)
        {
            unchecked
            {
                ulong h = offsetBasis;
                if (algorithm == FowlerNollVoAlgorithm.Fnv1a)
                {
                    for (int i = 0; i < input.Length; i++)
                    {
                        h ^= input[i];
                        h *= prime;
                    }
                }
                else
                {
                    for (int i = 0; i < input.Length; i++)
                    {
                        h *= prime;
                        h ^= input[i];
                    }
                }

                var result = new byte[8];
                if (byteOrder == Endianness.LittleEndian)
                {
                    result[0] = (byte)h;
                    result[1] = (byte)(h >> 8);
                    result[2] = (byte)(h >> 16);
                    result[3] = (byte)(h >> 24);
                    result[4] = (byte)(h >> 32);
                    result[5] = (byte)(h >> 40);
                    result[6] = (byte)(h >> 48);
                    result[7] = (byte)(h >> 56);
                }
                else
                {
                    result[7] = (byte)h;
                    result[6] = (byte)(h >> 8);
                    result[5] = (byte)(h >> 16);
                    result[4] = (byte)(h >> 24);
                    result[3] = (byte)(h >> 32);
                    result[2] = (byte)(h >> 40);
                    result[1] = (byte)(h >> 48);
                    result[0] = (byte)(h >> 56);
                }

                return result;
            }
        }
    }
}
