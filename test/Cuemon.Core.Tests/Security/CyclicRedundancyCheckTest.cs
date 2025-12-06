using System;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Security
{
    public class CyclicRedundancyCheckTest : Test
    {
        public CyclicRedundancyCheckTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Constructor_ShouldSetInitialAndFinal()
        {
            var sut = new TestCrc(1, 123, 456);
            Assert.Equal((ulong)123, sut.InitialValue);
            Assert.Equal((ulong)456, sut.FinalXor);
        }

        [Fact]
        public void LookupTable_ShouldContainComputedValues()
        {
            var sut = new TestCrc(1, 0, 0);
            var table = sut.PublicLookupTable;

            Assert.Equal(256, table.Length);
            Assert.Equal((ulong)8, table[0]); // 0 + 1*8
            Assert.Equal((ulong)18, table[10]); // 10 + 1*8
            Assert.Equal((ulong)263, table[255]); // 255 + 1*8

            TestOutput.WriteLine($"First: {table[0]}, Tenth: {table[10]}, Last: {table[255]}");
        }

        [Fact]
        public void LookupTable_ShouldRespectPolynomial()
        {
            var sut = new TestCrc(2, 0, 0);
            var table = sut.PublicLookupTable;

            Assert.Equal((ulong)16 + 1, table[1]); // 1 + 2*8 => 17
            Assert.Equal((ulong)(255 + 2 * 8), table[255]); // 255 + 16 => 271

            TestOutput.WriteLine($"Index1: {table[1]}, Index255: {table[255]}");
        }

        [Fact]
        public void LookupTable_MultipleAccessesReturnEqualContent()
        {
            var sut = new TestCrc(1, 0, 0);
            var t1 = sut.PublicLookupTable;
            var t2 = sut.PublicLookupTable;

            Assert.Equal(t1, t2);
        }

        private sealed class TestCrc : CyclicRedundancyCheck
        {
            public TestCrc(ulong polynomial, ulong initialValue, ulong finalXor) : base(polynomial, initialValue, finalXor, null)
            {
            }

            public ulong[] PublicLookupTable => LookupTable;

            protected override ulong PolynomialIndexInitializer(byte index)
            {
                return index;
            }

            protected override void PolynomialSlotCalculator(ref ulong checksum, ulong polynomial)
            {
                // simple deterministic operation: add polynomial
                checksum += polynomial;
            }

            public override HashResult ComputeHash(byte[] input)
            {
                // trivial implementation for testing purposes
                return new HashResult(Array.Empty<byte>());
            }
        }
    }
}
