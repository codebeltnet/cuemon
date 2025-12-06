using System;
using System.Numerics;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Security
{
    public class FowlerNollVoHashTest : Test
    {
        public FowlerNollVoHashTest(ITestOutputHelper output) : base(output)
        {
        }

        // Minimal concrete implementation so we can instantiate the abstract class.
        private sealed class TestFowlerNollVoHash : FowlerNollVoHash
        {
            public TestFowlerNollVoHash(short bits, BigInteger prime, BigInteger offsetBasis, Action<FowlerNollVoOptions> setup = null)
                : base(bits, prime, offsetBasis, setup ?? (o => { })) { }
        }

        [Fact]
        public void Constructor_UnsupportedBits_ThrowsArgumentOutOfRangeException()
        {
            var prime = new BigInteger(1);
            var offset = new BigInteger(0);
            Assert.Throws<ArgumentOutOfRangeException>(() => new TestFowlerNollVoHash(16, prime, offset));
        }

        [Fact]
        public void Properties_AreSet_FromConstructor()
        {
            short bits = 32;
            var prime = new BigInteger(16777619);
            var offset = new BigInteger(2166136261);
            var sut = new TestFowlerNollVoHash(bits, prime, offset);

            Assert.Equal(bits, sut.Bits);
            Assert.Equal(prime, sut.Prime);
            Assert.Equal(offset, sut.OffsetBasis);
        }

        [Fact]
        public void ComputeHash_Null_ThrowsArgumentNullException()
        {
            var sut = new TestFowlerNollVoHash(32, new BigInteger(16777619), new BigInteger(2166136261));
            Assert.Throws<ArgumentNullException>(() => sut.ComputeHash((byte[])null!));
        }

        [Fact]
        public void ComputeHash32_EmptyInput_ReturnsOffsetBasis_BigEndianByDefault()
        {
            var prime = new BigInteger(16777619); // FNV-1/1a 32-bit prime
            var offset = new BigInteger(2166136261); // FNV-1/1a 32-bit offset basis (0x811C9DC5)
            var sut = new TestFowlerNollVoHash(32, prime, offset); // default options: BigEndian + Fnv1a

            var result = sut.ComputeHash(Array.Empty<byte>());
            var bytes = result.GetBytes();

            // Big-endian representation of 0x811C9DC5
            var expected = new byte[] { 0x81, 0x1C, 0x9D, 0xC5 };
            Assert.Equal(expected, bytes);
        }

        [Fact]
        public void ComputeHash32_EmptyInput_ReturnsOffsetBasis_LittleEndianWhenConfigured()
        {
            var prime = new BigInteger(16777619);
            var offset = new BigInteger(2166136261);
            var sut = new TestFowlerNollVoHash(32, prime, offset, o => o.ByteOrder = Endianness.LittleEndian);

            var result = sut.ComputeHash(Array.Empty<byte>());
            var bytes = result.GetBytes();

            // Little-endian representation of 0x811C9DC5
            var expected = new byte[] { 0xC5, 0x9D, 0x1C, 0x81 };
            Assert.Equal(expected, bytes);
        }

        [Fact]
        public void ComputeHash32_NonEmpty_EqualsIndependentImplementation_Fnv1aAndFnv1()
        {
            var prime = 16777619u;
            var offset = 2166136261u;
            var data = "hello"u8.ToArray();

            // FNV-1a
            var sut1a = new TestFowlerNollVoHash(32, new BigInteger(prime), new BigInteger(offset));
            sut1a.Options.Algorithm = FowlerNollVoAlgorithm.Fnv1a;
            var expected1a = IndependentFNV32(data, prime, offset, FowlerNollVoAlgorithm.Fnv1a, sut1a.Options.ByteOrder);
            var actual1a = sut1a.ComputeHash(data).GetBytes();
            Assert.Equal(expected1a, actual1a);

            // FNV-1
            var sut1 = new TestFowlerNollVoHash(32, new BigInteger(prime), new BigInteger(offset));
            sut1.Options.Algorithm = FowlerNollVoAlgorithm.Fnv1;
            var expected1 = IndependentFNV32(data, prime, offset, FowlerNollVoAlgorithm.Fnv1, sut1.Options.ByteOrder);
            var actual1 = sut1.ComputeHash(data).GetBytes();
            Assert.Equal(expected1, actual1);
        }

        [Fact]
        public void ComputeHash64_NonEmpty_EqualsIndependentImplementation()
        {
            // Standard 64-bit FNV constants
            var prime = 1099511628211ul;
            var offset = 14695981039346656037ul;
            var data = "hello"u8.ToArray();

            var sut = new TestFowlerNollVoHash(64, new BigInteger(prime), new BigInteger(offset));
            sut.Options.Algorithm = FowlerNollVoAlgorithm.Fnv1a;

            var expected = IndependentFNV64(data, prime, offset, FowlerNollVoAlgorithm.Fnv1a, sut.Options.ByteOrder);
            var actual = sut.ComputeHash(data).GetBytes();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ComputeHashMultiWord_EmptyInput_ReturnsOffsetBasis_WithSelectedEndianness()
        {
            short bits = 128;
            // Use simple arbitrary 128-bit prime and offset basis for test determinism
            var primeBytes = new byte[16];
            var offsetBytes = new byte[16];
            for (int i = 0; i < 16; i++) { primeBytes[i] = (byte)(i + 1); offsetBytes[i] = (byte)(0xA0 + i); }
            var prime = new BigInteger(primeBytes);
            var offset = new BigInteger(offsetBytes);

            var sut = new TestFowlerNollVoHash(bits, prime, offset);
            sut.Options.Algorithm = FowlerNollVoAlgorithm.Fnv1a; // default but explicit

            var result = sut.ComputeHash(Array.Empty<byte>());
            var actualBytes = result.GetBytes();

            // Build expected bytes using the same conversion described in the implementation:
            int unitCount = bits / 32;
            var needed = unitCount * 4;
            var offBytes = offset.ToByteArray();
            if (offBytes.Length < needed)
            {
                var ext = new byte[needed];
                Array.Copy(offBytes, ext, offBytes.Length);
                offBytes = ext;
            }

            // convert to uint[] little-endian
            var a = new uint[unitCount];
            for (int i = 0; i < unitCount; i++)
            {
                int j = i * 4;
                a[i] = (uint)(offBytes[j] | (offBytes[j + 1] << 8) | (offBytes[j + 2] << 16) | (offBytes[j + 3] << 24));
            }

            // bytes from the uint[] (little-endian words)
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

            var expected = Convertible.ReverseEndianness(bytes, o => o.ByteOrder = sut.Options.ByteOrder);
            Assert.Equal(expected, actualBytes);
        }

        [Fact]
        public void ComputeHashMultiWord_NonEmpty_EqualsIndependentBigIntegerImplementation()
        {
            short bits = 128;
            // deterministic prime & offset
            var primeBytes = new byte[16];
            var offsetBytes = new byte[16];
            for (int i = 0; i < 16; i++) { primeBytes[i] = (byte)(i + 1); offsetBytes[i] = (byte)(0xB0 + i); }
            var prime = new BigInteger(primeBytes);
            var offset = new BigInteger(offsetBytes);
            var data = "abc"u8.ToArray();

            var sut = new TestFowlerNollVoHash(bits, prime, offset);
            sut.Options.Algorithm = FowlerNollVoAlgorithm.Fnv1a;

            var expected = IndependentFNVMultiWord(data, prime, offset, bits, sut.Options);
            var actual = sut.ComputeHash(data).GetBytes();
            Assert.Equal(expected, actual);
        }

        #region Independent reference implementations used by tests

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

            // Convert little-endian uint words to byte array (same as production code)
            var wordsBytes = new byte[unitCount * 4];
            for (int i = 0; i < unitCount; i++)
            {
                int j = i * 4;
                // ensure indices exist in bytes (they do due to padding)
                wordsBytes[j] = bytes[j];
                wordsBytes[j + 1] = bytes[j + 1];
                wordsBytes[j + 2] = bytes[j + 2];
                wordsBytes[j + 3] = bytes[j + 3];
            }

            // Apply endianness conversion as production code does
            return Convertible.ReverseEndianness(wordsBytes, o => o.ByteOrder = options.ByteOrder);
        }

        #endregion
    }
}
