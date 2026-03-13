using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using Cuemon.Text;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon
{
    public class ConvertibleTest : Test
    {
        public ConvertibleTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Constants_ShouldHaveExpectedValues()
        {
            Assert.Equal(0, Convertible.NullValue);
            Assert.Equal(8, Convertible.BitsPerByte);
            Assert.Equal(4, Convertible.BitsPerNibble);
        }

        [Fact]
        public void RegisterConvertible_ShouldThrowArgumentNullException_WhenConverterIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => Convertible.RegisterConvertible<RegisteredCustomConvertible>(null));
        }

        [Fact]
        public void RegisterConvertible_ShouldUseRegisteredConverter()
        {
            ClearCustomConverters();
            try
            {
                Convertible.RegisterConvertible<RegisteredCustomConvertible>(input => Encoding.UTF8.GetBytes(input.Value));
                var sut = new RegisteredCustomConvertible("custom");

                var result = Convertible.GetBytes((IConvertible)sut);

                Assert.Equal(Encoding.UTF8.GetBytes("custom"), result);
            }
            finally
            {
                ClearCustomConverters();
            }
        }

        [Fact]
        public void ReverseBits_ShouldReturnExpectedValues()
        {
            Assert.Equal((byte)0x80, Convertible.ReverseBits8(0x01));
            Assert.Equal((ushort)0x8000, Convertible.ReverseBits16(0x0001));
            Assert.Equal((uint)0x80000000, Convertible.ReverseBits32(0x00000001));
            Assert.Equal(0x8000000000000000UL, Convertible.ReverseBits64(0x0000000000000001UL));
        }

        [Fact]
        public void ReverseEndianness_ShouldKeepInput_WhenRequestedByteOrderMatchesPlatform()
        {
            var bytes = new byte[] { 1, 2, 3, 4 };
            var expected = bytes.ToArray();
            var targetOrder = BitConverter.IsLittleEndian ? Endianness.LittleEndian : Endianness.BigEndian;

            var result = Convertible.ReverseEndianness(bytes, o => o.ByteOrder = targetOrder);

            Assert.Same(bytes, result);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ReverseEndianness_ShouldReverseInput_WhenRequestedByteOrderDiffersFromPlatform()
        {
            var bytes = new byte[] { 1, 2, 3, 4 };
            var expected = new byte[] { 4, 3, 2, 1 };
            var targetOrder = BitConverter.IsLittleEndian ? Endianness.BigEndian : Endianness.LittleEndian;

            var result = Convertible.ReverseEndianness(bytes, o => o.ByteOrder = targetOrder);

            Assert.Same(bytes, result);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetBytes_IConvertible_ShouldReturnNullValueBytes_WhenInputIsNull()
        {
            Assert.Equal(BitConverter.GetBytes(Convertible.NullValue), Convertible.GetBytes((IConvertible)null));
        }

        [Fact]
        public void GetBytes_IConvertible_ShouldUseLocalConverter_WhenConfigured()
        {
            var result = Convertible.GetBytes((IConvertible)42, o => o.Converters.Add(typeof(int), _ => new byte[] { 9, 8, 7 }));

            Assert.Equal(new byte[] { 9, 8, 7 }, result);
        }

        [Fact]
        public void GetBytes_IConvertible_ShouldUsePrimitiveConverter()
        {
            var input = 0x01020304;
            var result = Convertible.GetBytes((IConvertible)input, o => o.ByteOrder = Endianness.BigEndian);
            var expected = BitConverter.GetBytes(input);
            if (BitConverter.IsLittleEndian) { Array.Reverse(expected); }

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetBytes_IConvertible_ShouldUseEnumConverter()
        {
            Enum input = UInt16Enum.One;

            var result = Convertible.GetBytes((IConvertible)input, o => o.ByteOrder = Endianness.BigEndian);
            var expected = Convertible.GetBytes(input, o => o.ByteOrder = Endianness.BigEndian);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetBytes_IConvertible_ShouldUseKnownNonPrimitiveConverters()
        {
            var dateTime = new DateTime(2024, 1, 2, 3, 4, 5, DateTimeKind.Utc);
            const string text = "kernel";
            const decimal number = 12.34m;

            Assert.Equal(Convertible.GetBytes(text), Convertible.GetBytes((IConvertible)text));
            Assert.Equal(Convertible.GetBytes(dateTime), Convertible.GetBytes((IConvertible)dateTime));
            Assert.Equal(Convertible.GetBytes(number), Convertible.GetBytes((IConvertible)number));
            Assert.Equal(Convertible.GetBytes(DBNull.Value), Convertible.GetBytes((IConvertible)DBNull.Value));
        }

        [Fact]
        public void GetBytes_IConvertible_ShouldThrowArgumentOutOfRangeException_WhenConverterIsUnknown()
        {
            ClearCustomConverters();
            var input = new UnregisteredCustomConvertible("unknown");

            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => Convertible.GetBytes((IConvertible)input));

            Assert.Equal("input", ex.ParamName);
            Assert.Same(input, ex.ActualValue);
        }

        [Fact]
        public void GetBytes_Enumerable_ShouldAggregateAllValues()
        {
            IEnumerable<IConvertible> input = new IConvertible[] { 1, "A", null, UInt16Enum.One };

            var result = Convertible.GetBytes(input);
            var expected = Convertible.GetBytes(1)
                .Concat(Convertible.GetBytes("A"))
                .Concat(BitConverter.GetBytes(Convertible.NullValue))
                .Concat(Convertible.GetBytes((Enum)UInt16Enum.One))
                .ToArray();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetBytes_PrimitiveOverloads_ShouldReturnExpectedBytes()
        {
            Assert.Equal(BitConverter.GetBytes(true), Convertible.GetBytes(true));
            Assert.Equal(new byte[] { 0x2A }, Convertible.GetBytes((byte)0x2A));
            Assert.Equal(BitConverter.GetBytes('K'), Convertible.GetBytes('K'));
            Assert.Equal(BitConverter.GetBytes(123.456d), Convertible.GetBytes(123.456d));
            Assert.Equal(BitConverter.GetBytes((short)-1234), Convertible.GetBytes((short)-1234));
            Assert.Equal(BitConverter.GetBytes(123456), Convertible.GetBytes(123456));
            Assert.Equal(BitConverter.GetBytes(1234567890123L), Convertible.GetBytes(1234567890123L));
            Assert.NotEmpty(Convertible.GetBytes((sbyte)-12));
            Assert.Equal(BitConverter.GetBytes(3.14f), Convertible.GetBytes(3.14f));
            Assert.Equal(BitConverter.GetBytes((ushort)65000), Convertible.GetBytes((ushort)65000));
            Assert.Equal(BitConverter.GetBytes((uint)1234567890), Convertible.GetBytes((uint)1234567890));
            Assert.Equal(BitConverter.GetBytes((ulong)1234567890123456789), Convertible.GetBytes((ulong)1234567890123456789));
        }

        [Fact]
        public void GetBytes_String_ShouldRespectPreambleConfiguration()
        {
            var text = "abc";

            var keep = Convertible.GetBytes(text, o =>
            {
                o.Encoding = Encoding.UTF8;
                o.Preamble = PreambleSequence.Keep;
            });
            var remove = Convertible.GetBytes(text, o =>
            {
                o.Encoding = Encoding.UTF8;
                o.Preamble = PreambleSequence.Remove;
            });

            Assert.Equal(Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(text)).ToArray(), keep);
            Assert.Equal(Encoding.UTF8.GetBytes(text), remove);
        }

        [Fact]
        public void GetBytes_String_ShouldThrowArgumentNullException_WhenInputIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => Convertible.GetBytes((string)null));
        }

        [Fact]
        public void GetBytes_String_ShouldThrowInvalidEnumArgumentException_WhenPreambleIsInvalid()
        {
            Assert.Throws<InvalidEnumArgumentException>(() => Convertible.GetBytes("abc", o => o.Preamble = (PreambleSequence)42));
        }

        [Fact]
        public void GetBytes_DateTime_Decimal_AndDBNull_ShouldReturnExpectedBytes()
        {
            var dateTime = new DateTime(2024, 2, 3, 4, 5, 6, DateTimeKind.Utc);
            var expectedDateTime = Encoding.ASCII.GetBytes(dateTime.ToString("u", CultureInfo.InvariantCulture));
            var expectedDecimal = Encoding.ASCII.GetBytes(12.34m.ToString(CultureInfo.InvariantCulture));

            Assert.Equal(expectedDateTime, Convertible.GetBytes(dateTime));
            Assert.Equal(expectedDecimal, Convertible.GetBytes(12.34m));
            Assert.Equal(BitConverter.GetBytes(Convertible.NullValue), Convertible.GetBytes(DBNull.Value));
        }

        [Fact]
        public void GetBytes_Enum_ShouldCoverUnderlyingTypeBranches()
        {
            Assert.Equal(Convertible.GetBytes((byte)1), Convertible.GetBytes((Enum)ByteEnum.One));
            Assert.Equal(Convertible.GetBytes((short)1), Convertible.GetBytes((Enum)Int16Enum.One));
            Assert.Equal(Convertible.GetBytes(1), Convertible.GetBytes((Enum)Int32Enum.One));
            Assert.Equal(Convertible.GetBytes((long)1), Convertible.GetBytes((Enum)Int64Enum.One));
            Assert.Equal(Convertible.GetBytes((sbyte)1), Convertible.GetBytes((Enum)SByteEnum.One));
            Assert.Equal(Convertible.GetBytes((ushort)1), Convertible.GetBytes((Enum)UInt16Enum.One));
            Assert.Equal(Convertible.GetBytes((uint)1), Convertible.GetBytes((Enum)UInt32Enum.One));
            Assert.Equal(Convertible.GetBytes((ulong)1), Convertible.GetBytes((Enum)UInt64Enum.One));
        }

        [Fact]
        public void ToString_ShouldThrowArgumentNullException_WhenInputIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => Convertible.ToString(null));
        }

        [Fact]
        public void ToString_ShouldDetectEncodingAndRemovePreambleByDefault()
        {
            var expected = "hello world";
            var bytes = Encoding.Unicode.GetPreamble().Concat(Encoding.Unicode.GetBytes(expected)).ToArray();

            var result = Convertible.ToString(bytes);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToString_ShouldRespectPreambleConfiguration()
        {
            const string expected = "payload";
            var bytes = Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(expected)).ToArray();

            var keep = Convertible.ToString(bytes, o =>
            {
                o.Encoding = Encoding.UTF8;
                o.Preamble = PreambleSequence.Keep;
            });
            var remove = Convertible.ToString(bytes, o =>
            {
                o.Encoding = Encoding.UTF8;
                o.Preamble = PreambleSequence.Remove;
            });

            Assert.Equal("\uFEFF" + expected, keep);
            Assert.Equal(expected, remove);
        }

        [Fact]
        public void ToString_ShouldThrowInvalidEnumArgumentException_WhenPreambleIsInvalid()
        {
            Assert.Throws<InvalidEnumArgumentException>(() => Convertible.ToString(Encoding.UTF8.GetBytes("abc"), o => o.Preamble = (PreambleSequence)42));
        }

        private sealed class RegisteredCustomConvertible : ConvertibleBase
        {
            public RegisteredCustomConvertible(string value)
            {
                Value = value;
            }

            public string Value { get; }
        }

        private sealed class UnregisteredCustomConvertible : ConvertibleBase
        {
            public UnregisteredCustomConvertible(string value)
            {
                Value = value;
            }

            public string Value { get; }
        }

        private abstract class ConvertibleBase : IConvertible
        {
            public virtual TypeCode GetTypeCode() => TypeCode.Object;
            public virtual bool ToBoolean(IFormatProvider provider) => throw new NotImplementedException();
            public virtual byte ToByte(IFormatProvider provider) => throw new NotImplementedException();
            public virtual char ToChar(IFormatProvider provider) => throw new NotImplementedException();
            public virtual DateTime ToDateTime(IFormatProvider provider) => throw new NotImplementedException();
            public virtual decimal ToDecimal(IFormatProvider provider) => throw new NotImplementedException();
            public virtual double ToDouble(IFormatProvider provider) => throw new NotImplementedException();
            public virtual short ToInt16(IFormatProvider provider) => throw new NotImplementedException();
            public virtual int ToInt32(IFormatProvider provider) => throw new NotImplementedException();
            public virtual long ToInt64(IFormatProvider provider) => throw new NotImplementedException();
            public virtual sbyte ToSByte(IFormatProvider provider) => throw new NotImplementedException();
            public virtual float ToSingle(IFormatProvider provider) => throw new NotImplementedException();
            public virtual string ToString(IFormatProvider provider) => base.ToString();
            public virtual object ToType(Type conversionType, IFormatProvider provider) => throw new NotImplementedException();
            public virtual ushort ToUInt16(IFormatProvider provider) => throw new NotImplementedException();
            public virtual uint ToUInt32(IFormatProvider provider) => throw new NotImplementedException();
            public virtual ulong ToUInt64(IFormatProvider provider) => throw new NotImplementedException();
        }

        private enum ByteEnum : byte
        {
            One = 1
        }

        private enum Int16Enum : short
        {
            One = 1
        }

        private enum Int32Enum
        {
            One = 1
        }

        private enum Int64Enum : long
        {
            One = 1
        }

        private enum SByteEnum : sbyte
        {
            One = 1
        }

        private enum UInt16Enum : ushort
        {
            One = 1
        }

        private enum UInt32Enum : uint
        {
            One = 1
        }

        private enum UInt64Enum : ulong
        {
            One = 1
        }

        private static void ClearCustomConverters()
        {
            var field = typeof(Convertible).GetField("ByteArrayConverters", BindingFlags.Static | BindingFlags.NonPublic);
            var converters = Assert.IsType<Dictionary<Type, Func<IConvertible, byte[]>>>(field?.GetValue(null));
            converters.Remove(typeof(RegisteredCustomConvertible));
        }
    }
}
