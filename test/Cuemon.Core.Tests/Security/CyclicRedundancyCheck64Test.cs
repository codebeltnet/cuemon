using System;
using System.Reflection;
using System.Text;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Security
{
    public class CyclicRedundancyCheck64Test : Test
    {
        public CyclicRedundancyCheck64Test(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Constructor_ShouldSetDefaults()
        {
            var sut = new CyclicRedundancyCheck64();
            Assert.Equal((ulong)0x0000000000000000, sut.InitialValue);
            Assert.Equal((ulong)0x0000000000000000, sut.FinalXor);
        }

        [Fact]
        public void PolynomialIndexInitializer_ShouldReturnExpected()
        {
            var sut = new CyclicRedundancyCheck64();
            var mi = typeof(CyclicRedundancyCheck64).GetMethod("PolynomialIndexInitializer", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(mi);

            var value0 = (ulong)mi.Invoke(sut, new object[] { (byte)0 });
            var value1 = (ulong)mi.Invoke(sut, new object[] { (byte)1 });
            var value255 = (ulong)mi.Invoke(sut, new object[] { (byte)255 });

            Assert.Equal((ulong)((ulong)0 << 56), value0);
            Assert.Equal((ulong)((ulong)1 << 56), value1);
            Assert.Equal((ulong)((ulong)255 << 56), value255);
        }

        [Fact]
        public void PolynomialSlotCalculator_ShouldMutateChecksumAsExpected()
        {
            var sut = new CyclicRedundancyCheck64();
            var mi = typeof(CyclicRedundancyCheck64).GetMethod("PolynomialSlotCalculator", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(mi);

            var polynomial = (ulong)0x42F0E1EBA9EA3693;

            // case where highest bit is set
            object[] argsSet = { (ulong)0x8000000000000000, polynomial };
            mi.Invoke(sut, argsSet);
            var resultSet = (ulong)argsSet[0];
            var expectedSet = (((ulong)0x8000000000000000 << 1) ^ polynomial);
            Assert.Equal(expectedSet, resultSet);

            // case where highest bit is not set
            object[] argsNotSet = { (ulong)0x7FFFFFFFFFFFFFFF, polynomial };
            mi.Invoke(sut, argsNotSet);
            var resultNotSet = (ulong)argsNotSet[0];
            var expectedNotSet = ((ulong)0x7FFFFFFFFFFFFFFF << 1);
            Assert.Equal(expectedNotSet, resultNotSet);
        }

        [Fact]
        public void LookupTable_ShouldContainComputedValues()
        {
            var sut = new CyclicRedundancyCheck64();
            var prop = typeof(CyclicRedundancyCheck).GetProperty("LookupTable", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            Assert.NotNull(prop);

            var table = (ulong[])prop.GetValue(sut);
            Assert.Equal(256, table.Length);

            // compute expected table using same algorithm
            var expected = new ulong[256];
            var polynomial = (ulong)0x42F0E1EBA9EA3693;
            for (int i = 0; i < 256; i++)
            {
                var checksum = (ulong)((ulong)((byte)i) << 56);
                for (int b = 0; b < 8; b++)
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
                expected[i] = checksum;
            }

            Assert.True(expected.SequenceEqual(table));
        }

        [Fact]
        public void LookupTable_MultipleAccessesReturnEqualContent()
        {
            var sut = new CyclicRedundancyCheck64();
            var prop = typeof(CyclicRedundancyCheck).GetProperty("LookupTable", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            Assert.NotNull(prop);

            var t1 = (ulong[])prop.GetValue(sut);
            var t2 = (ulong[])prop.GetValue(sut);

            Assert.Equal(t1, t2);
        }

        [Fact]
        public void ComputeHash_ShouldProduceKnownCrc64Ecma_ForStandardInput()
        {
            // CRC-64-ECMA-182 for ASCII "123456789" is 0x6C40DF5F0B497347
            var sut = new CyclicRedundancyCheck64();

            var input = Encoding.ASCII.GetBytes("123456789");
            var result = sut.ComputeHash(input);
            var hex = result.ToHexadecimalString().ToLowerInvariant();

            Assert.Equal("6c40df5f0b497347", hex);
        }
    }
}
