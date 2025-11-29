using System;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Security
{
    public class HashResultTest : Test
    {
        public HashResultTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Constructor_NullInput_HasValueFalse_And_GetBytesEmpty()
        {
            var hr = new HashResult(null);
            Assert.False(hr.HasValue);
            Assert.Empty(hr.GetBytes());
        }

        [Fact]
        public void Constructor_WithBytes_HasValueTrue_And_GetBytesReturnsEqualCopy()
        {
            var input = new byte[] { 0x01, 0x0A, 0xFF };
            var hr = new HashResult(input);
            Assert.True(hr.HasValue);

            var bytesFromHr = hr.GetBytes();
            Assert.Equal(input, bytesFromHr);
            // Ensure returned array is a copy (not the same reference)
            Assert.NotSame(input, bytesFromHr);
        }

        [Fact]
        public void GetBytes_ReturnsIndependentCopies()
        {
            var input = new byte[] { 11, 22, 33 };
            var hr = new HashResult(input);

            var first = hr.GetBytes();
            first[0] = 99; // mutate returned copy

            var second = hr.GetBytes();
            // Mutation of the first copy must not affect subsequent copies
            Assert.NotEqual(first, second);
            Assert.Equal(new byte[] { 11, 22, 33 }, second);
        }

        [Fact]
        public void ToHexadecimalString_ReturnsExpectedLowercaseHex()
        {
            var bytes = new byte[] { 0x0F, 0xA0, 0x01 };
            var hr = new HashResult(bytes);
            // Expected: "0fa001" (StringFactory.CreateHexadecimal uses lowercase)
            Assert.Equal("0fa001", hr.ToHexadecimalString());
            Assert.Equal(hr.ToHexadecimalString(), hr.ToString()); // ToString delegates to hex
        }

        [Fact]
        public void ToBase64String_ReturnsExpected()
        {
            var bytes = new byte[] { 0x01, 0x02, 0x03 };
            var hr = new HashResult(bytes);
            var expected = Convert.ToBase64String(bytes);
            Assert.Equal(expected, hr.ToBase64String());
        }

        [Fact]
        public void ToUrlEncodedBase64String_ReturnsExpectedUrlSafeBase64()
        {
            var bytes = new byte[] { 0xFF, 0xEE, 0xDD, 0xCC };
            var hr = new HashResult(bytes);

            var base64 = Convert.ToBase64String(bytes);
            var expected = base64.Split('=')[0].Replace('+', '-').Replace('/', '_');

            Assert.Equal(expected, hr.ToUrlEncodedBase64String());
        }

        [Fact]
        public void ToBinaryString_ReturnsExpectedConcatenatedBinaryDigits()
        {
            var bytes = new byte[] { 0x01, 0x02 };
            var hr = new HashResult(bytes);

            string ToBin(byte b) => Convert.ToString(b, 2).PadLeft(8, '0');
            var expected = ToBin(0x01) + ToBin(0x02);

            Assert.Equal(expected, hr.ToBinaryString());
        }

        [Fact]
        public void GetHashCode_EqualsUnderlyingArrayHashCode()
        {
            var arr = new byte[] { 7, 8, 9 };
            var hr = new HashResult(arr);
            Assert.Equal(arr.GetHashCode(), hr.GetHashCode());
        }

        [Fact]
        public void Equals_Object_NotHashResult_ReturnsFalse()
        {
            var hr = new HashResult(new byte[] { 1 });
            Assert.False(hr.Equals("not a hashresult"));
        }

        [Fact]
        public void Equals_HashResult_Null_ReturnsFalse()
        {
            var hr = new HashResult(new byte[] { 1, 2 });
            Assert.False(hr.Equals((HashResult)null));
        }

        [Fact]
        public void Equals_SameUnderlyingArrayReference_ReturnsTrue()
        {
            var arr = new byte[] { 5, 6, 7 };
            var a = new HashResult(arr);
            var b = new HashResult(arr);
            Assert.True(a.Equals(b));
            Assert.Equal(a.GetHashCode(), b.GetHashCode());
        }

        [Fact]
        public void Equals_DifferentArrayWithSameContent_ReturnsFalse()
        {
            var a = new HashResult(new byte[] { 2, 3, 4 });
            var b = new HashResult(new byte[] { 2, 3, 4 }); // different instance
            // HashResult.Equals compares GetHashCode which is reference-based on the array,
            // so different array instances with equal content will not be considered equal.
            Assert.False(a.Equals(b));
        }

        [Fact]
        public void ToT_ConverterReceivesUnderlyingBytesAndReturnsConvertedValue()
        {
            var bytes = new byte[] { 10, 20, 30 };
            var hr = new HashResult(bytes);

            var convertedViaTo = hr.To(b => Convert.ToBase64String(b));
            var convertedViaMethod = hr.ToBase64String();

            Assert.Equal(convertedViaMethod, convertedViaTo);
        }
    }
}
