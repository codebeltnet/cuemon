using System;
using System.Numerics;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Security
{
    public class FowlerNollVo32Test : Test
    {
        public FowlerNollVo32Test(ITestOutputHelper output) : base(output) { }

        [Fact]
        public void Constructor_Default_SetsBits()
        {
            var sut = new FowlerNollVo32();
            Assert.Equal(32, sut.Bits);
        }

        [Fact]
        public void ComputeHash_Empty_ReturnsOffsetBasis_BigEndianByDefault()
        {
            var sut = new FowlerNollVo32();
            var result = sut.ComputeHash(Array.Empty<byte>()).GetBytes();
            var expected = new byte[] { 0x81, 0x1C, 0x9D, 0xC5 }; // 0x811C9DC5
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ComputeHash_Empty_ReturnsOffsetBasis_LittleEndianWhenConfigured()
        {
            var sut = new FowlerNollVo32(o => o.ByteOrder = Endianness.LittleEndian);
            var result = sut.ComputeHash(Array.Empty<byte>()).GetBytes();
            var expected = new byte[] { 0xC5, 0x9D, 0x1C, 0x81 };
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ComputeHash_NonEmpty_EqualsIndependentImplementation_Fnv1aAndFnv1()
        {
            var data = "hello"u8.ToArray();
            var sut1a = new FowlerNollVo32();
            sut1a.Options.Algorithm = FowlerNollVoAlgorithm.Fnv1a;
            var expected1a = IndependentFNV32(data, (uint)new BigInteger(16777619), (uint)new BigInteger(2166136261), FowlerNollVoAlgorithm.Fnv1a, sut1a.Options.ByteOrder);
            var actual1a = sut1a.ComputeHash(data).GetBytes();
            Assert.Equal(expected1a, actual1a);

            var sut1 = new FowlerNollVo32();
            sut1.Options.Algorithm = FowlerNollVoAlgorithm.Fnv1;
            var expected1 = IndependentFNV32(data, (uint)new BigInteger(16777619), (uint)new BigInteger(2166136261), FowlerNollVoAlgorithm.Fnv1, sut1.Options.ByteOrder);
            var actual1 = sut1.ComputeHash(data).GetBytes();
            Assert.Equal(expected1, actual1);
        }

        private static byte[] IndependentFNV32(byte[] input, uint prime, uint offsetBasis, FowlerNollVoAlgorithm algorithm, Endianness byteOrder)
        {
            unchecked
            {
                uint h = offsetBasis;
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

                var result = new byte[4];
                if (byteOrder == Endianness.LittleEndian)
                {
                    result[0] = (byte)h;
                    result[1] = (byte)(h >> 8);
                    result[2] = (byte)(h >> 16);
                    result[3] = (byte)(h >> 24);
                }
                else
                {
                    result[3] = (byte)h;
                    result[2] = (byte)(h >> 8);
                    result[1] = (byte)(h >> 16);
                    result[0] = (byte)(h >> 24);
                }
                return result;
            }
        }
    }
}
