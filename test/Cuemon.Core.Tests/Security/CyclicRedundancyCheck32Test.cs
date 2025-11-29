using System;
using System.Reflection;
using System.Text;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Security
{
    public class CyclicRedundancyCheck32Test : Test
    {
        public CyclicRedundancyCheck32Test(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Constructor_ShouldSetDefaults()
        {
            var sut = new CyclicRedundancyCheck32();
            Assert.Equal((ulong)0xFFFFFFFF, sut.InitialValue);
            Assert.Equal((ulong)0xFFFFFFFF, sut.FinalXor);
        }

        [Fact]
        public void PolynomialIndexInitializer_ShouldReturnExpected()
        {
            var sut = new CyclicRedundancyCheck32();
            var mi = typeof(CyclicRedundancyCheck32).GetMethod("PolynomialIndexInitializer", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(mi);

            var value0 = (ulong)mi.Invoke(sut, new object[] { (byte)0 });
            var value1 = (ulong)mi.Invoke(sut, new object[] { (byte)1 });
            var value255 = (ulong)mi.Invoke(sut, new object[] { (byte)255 });

            Assert.Equal((ulong)((uint)0 << 24), value0);
            Assert.Equal((ulong)((uint)1 << 24), value1);
            Assert.Equal((ulong)((uint)255 << 24), value255);
        }

        [Fact]
        public void PolynomialSlotCalculator_ShouldMutateChecksumAsExpected()
        {
            var sut = new CyclicRedundancyCheck32();
            var mi = typeof(CyclicRedundancyCheck32).GetMethod("PolynomialSlotCalculator", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(mi);

            var polynomial = (ulong)0x4C11DB7;

            // case where highest bit is set
            object[] argsSet = { (ulong)0x80000000, polynomial };
            mi.Invoke(sut, argsSet);
            var resultSet = (ulong)argsSet[0];
            var expectedSet = (((ulong)0x80000000 << 1) ^ polynomial);
            Assert.Equal(expectedSet, resultSet);

            // case where highest bit is not set
            object[] argsNotSet = { (ulong)0x7FFFFFFF, polynomial };
            mi.Invoke(sut, argsNotSet);
            var resultNotSet = (ulong)argsNotSet[0];
            var expectedNotSet = ((ulong)0x7FFFFFFF << 1);
            Assert.Equal(expectedNotSet, resultNotSet);
        }

        [Fact]
        public void LookupTable_ShouldContainComputedValues()
        {
            var sut = new CyclicRedundancyCheck32();
            var prop = typeof(CyclicRedundancyCheck).GetProperty("LookupTable", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            Assert.NotNull(prop);

            var table = (ulong[])prop.GetValue(sut);
            Assert.Equal(256, table.Length);

            // compute expected table using same algorithm
            var expected = new ulong[256];
            var polynomial = (ulong)0x4C11DB7;
            for (int i = 0; i < 256; i++)
            {
                var checksum = (ulong)((uint)((byte)i << 24));
                for (int b = 0; b < 8; b++)
                {
                    if ((checksum & 0x80000000) != 0)
                    {
                        checksum <<= 1;
                        checksum ^= polynomial;
                    }
                    else
                    {
                        checksum <<= 1;
                    }
                }
                expected[i] = checksum;
            }

            Assert.True(expected.SequenceEqual(table));
        }

        [Fact]
        public void ComputeHash_ShouldProduceKnownCrc32_ForStandardInput()
        {
            // Standard CRC-32 (IEEE 802.3) for ASCII "123456789" is 0xCBF43926
            var sut = new CyclicRedundancyCheck32(setup: o =>
            {
                o.ReflectInput = true;
                o.ReflectOutput = true;
            });

            var input = Encoding.ASCII.GetBytes("123456789");
            var result = sut.ComputeHash(input);
            var hex = result.ToHexadecimalString().ToLowerInvariant();

            Assert.Equal("cbf43926", hex);
        }
    }
}
