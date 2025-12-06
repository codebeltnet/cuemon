using System;
using System.Linq;
using System.Reflection;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Security.Cryptography
{
    public class SHA512256Test : Test
    {
        public SHA512256Test(ITestOutputHelper output) : base(output) { }

        [Fact]
        public void Ctor_ShouldSetHashSizeTo256()
        {
            using var sut = new SHA512256();
            Assert.Equal(256, sut.HashSize);
        }

        [Fact]
        public void WriteULongBE_ShouldWriteBigEndianBytes()
        {
            // Arrange
            var mi = typeof(SHA512256).GetMethod("WriteULongBE", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(mi);

            ulong value = 0x1122334455667788UL;
            var buffer = new byte[16];

            // Act
            mi.Invoke(null, new object[] { value, buffer, 4 });

            // Assert: bytes at offset 4..11 are big-endian representation of value
            var expected = new byte[] { 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88 };
            Assert.Equal(expected, buffer.Skip(4).Take(8).ToArray());
        }

        [Fact]
        public void RotateRight_ShouldRotateBitsCorrectly()
        {
            var mi = typeof(SHA512256).GetMethod("RotateRight", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(mi);

            ulong x = 0x8000000000000001UL; // bits at both ends
            int n = 1;

            var result = (ulong)mi.Invoke(null, new object[] { x, n });
            // manual rotate right by 1: (x >> 1) | (x << 63)
            ulong expected = (x >> 1) | (x << (64 - n));
            Assert.Equal(expected, result);

            // another arbitrary rotation
            x = 0x0123456789ABCDEFUL;
            n = 20;
            result = (ulong)mi.Invoke(null, new object[] { x, n });
            expected = (x >> n) | (x << (64 - n));
            Assert.Equal(expected, result);
        }

        [Fact]
        public void AddLength_ShouldAccumulateBitCountsAndCarryToHigh()
        {
            var sut = new SHA512256();
            var mi = typeof(SHA512256).GetMethod("AddLength", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(mi);

            var lowField = typeof(SHA512256).GetField("_bitCountLow", BindingFlags.NonPublic | BindingFlags.Instance);
            var highField = typeof(SHA512256).GetField("_bitCountHigh", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(lowField);
            Assert.NotNull(highField);

            // Set low to near overflow
            lowField.SetValue(sut, ulong.MaxValue - 7UL);
            highField.SetValue(sut, 5UL);

            // Add 8 bits -> causes low to wrap and high to increment
            mi.Invoke(sut, new object[] { 8UL });

            var low = (ulong)lowField.GetValue(sut);
            var high = (ulong)highField.GetValue(sut);

            unchecked
            {
                Assert.Equal(ulong.MaxValue - 7UL + 8UL, low); // wraps by ulong arithmetic
            }

            Assert.Equal(6UL, high); // carried 1
        }

        [Fact]
        public void ProcessBlock_ShouldChangeInternalState()
        {
            var sut = new SHA512256();

            var hField = typeof(SHA512256).GetField("_H", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(hField);

            // Get initial H (after constructor Initialize ran)
            var initialH = (ulong[])hField.GetValue(sut);
            Assert.NotNull(initialH);

            // Clone initial for comparison
            var before = (ulong[])initialH.Clone();

            // Prepare a single block with non-zero content
            var block = Enumerable.Range(0, 128).Select(i => (byte)(i & 0xFF)).ToArray();

            var mi = typeof(SHA512256).GetMethod("ProcessBlock", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(mi);

            // Act
            mi.Invoke(sut, new object[] { block, 0 });

            var after = (ulong[])hField.GetValue(sut);

            // Assert that at least one H word changed
            Assert.True(before.Where((v, i) => v != after[i]).Any());
        }

        [Fact]
        public void Initialize_ShouldResetInternalState()
        {
            var sut = new SHA512256();

            var hField = typeof(SHA512256).GetField("_H", BindingFlags.NonPublic | BindingFlags.Instance);
            var bufferPosField = typeof(SHA512256).GetField("_bufferPos", BindingFlags.NonPublic | BindingFlags.Instance);
            var lowField = typeof(SHA512256).GetField("_bitCountLow", BindingFlags.NonPublic | BindingFlags.Instance);
            var highField = typeof(SHA512256).GetField("_bitCountHigh", BindingFlags.NonPublic | BindingFlags.Instance);
            var ivField = typeof(SHA512256).GetField("IV512_256", BindingFlags.NonPublic | BindingFlags.Static);

            Assert.NotNull(hField);
            Assert.NotNull(bufferPosField);
            Assert.NotNull(lowField);
            Assert.NotNull(highField);
            Assert.NotNull(ivField);

            // Compute a hash to ensure the algorithm runs (don't assert internal change - implementations may reset state)
            var result = sut.ComputeHash("hello"u8.ToArray());
            Assert.NotNull(result);

            // Act: initialize and assert reset
            sut.Initialize();

            var resetH = (ulong[])hField.GetValue(sut);
            var bufferPos = (int)bufferPosField.GetValue(sut);
            var low = (ulong)lowField.GetValue(sut);
            var high = (ulong)highField.GetValue(sut);
            var iv = (ulong[])ivField.GetValue(null);

            Assert.Equal(0, bufferPos);
            Assert.Equal(0UL, low);
            Assert.Equal(0UL, high);
            Assert.Equal(iv, resetH);
        }

        [Fact]
        public void ComputeHash_ShouldReturn32ByteDigest_AndBeDeterministic()
        {
            using var sut = new SHA512256();

            var input = "hello"u8.ToArray();

            var one = sut.ComputeHash(input);
            var two = sut.ComputeHash(input);

            Assert.NotNull(one);
            Assert.NotNull(two);
            Assert.Equal(32, one.Length);
            Assert.Equal(32, two.Length);
            Assert.Equal(one, two); // deterministic
        }

        [Fact]
        public void ComputeHash_EmptyArray_ShouldReturn32Bytes()
        {
            using var sut = new SHA512256();

            var input = Array.Empty<byte>();

            var hash = sut.ComputeHash(input);

            Assert.NotNull(hash);
            Assert.Equal(32, hash.Length);
        }
    }
}
