using System;
using System.Numerics;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Security
{
    public class FowlerNollVo1024Test : Test
    {
        public FowlerNollVo1024Test(ITestOutputHelper output) : base(output) { }

        [Fact]
        public void Constructor_Default_SetsBits()
        {
            var sut = new FowlerNollVo1024();
            Assert.Equal(1024, sut.Bits);
        }

        [Fact]
        public void ComputeHash_Empty_ReturnsOffsetBasis_LengthIs128Bytes()
        {
            var sut = new FowlerNollVo1024();
            var result = sut.ComputeHash(Array.Empty<byte>()).GetBytes();
            Assert.Equal(128, result.Length);
        }

        [Fact]
        public void ComputeHash_NonEmpty_EqualsIndependentBigIntegerImplementation()
        {
            var data = "abc"u8.ToArray();
            var sut = new FowlerNollVo1024();
            sut.Options.Algorithm = FowlerNollVoAlgorithm.Fnv1a;
            var expected = IndependentFNVMultiWord(data, sut.Prime, sut.OffsetBasis, sut.Bits, sut.Options);
            var actual = sut.ComputeHash(data).GetBytes();
            Assert.Equal(expected, actual);
        }

        private static byte[] IndependentFNVMultiWord(byte[] input, BigInteger prime, BigInteger offsetBasis, short bits, FowlerNollVoOptions options)
        {
            BigInteger mask = (BigInteger.One << bits) - 1;
            BigInteger a = offsetBasis & mask;
            for (int idx = 0; idx < input.Length; idx++)
            {
                if (options.Algorithm == FowlerNollVoAlgorithm.Fnv1a)
                {
                    a ^= input[idx];
                    a = (a * prime) & mask;
                }
                else
                {
                    a = (a * prime) & mask;
                    a ^= input[idx];
                }
            }

            int unitCount = bits / 32;
            int needed = unitCount * 4;
            var bytes = a.ToByteArray();
            if (bytes.Length < needed)
            {
                var ext = new byte[needed];
                Array.Copy(bytes, ext, bytes.Length);
                bytes = ext;
            }

            var wordsBytes = new byte[unitCount * 4];
            for (int i = 0; i < unitCount; i++)
            {
                int j = i * 4;
                wordsBytes[j] = bytes[j];
                wordsBytes[j + 1] = bytes[j + 1];
                wordsBytes[j + 2] = bytes[j + 2];
                wordsBytes[j + 3] = bytes[j + 3];
            }

            return Convertible.ReverseEndianness(wordsBytes, o => o.ByteOrder = options.ByteOrder);
        }
    }
}
