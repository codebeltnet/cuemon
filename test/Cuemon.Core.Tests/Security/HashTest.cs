using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Codebelt.Extensions.Xunit;
using Cuemon.Collections.Generic;
using Xunit;

namespace Cuemon.Security
{
    public class HashTest : Test
    {
        public HashTest(ITestOutputHelper output) : base(output)
        {
        }

        private sealed class PassthroughHash : Hash
        {
            // Return the input bytes as the computed "hash" so tests can inspect what bytes were provided.
            public override HashResult ComputeHash(byte[] input)
            {
                return new HashResult(input ?? Array.Empty<byte>());
            }

            // No-op endian initializer to mirror default behavior used in tests.
            protected override void EndianInitializer(EndianOptions options)
            {
                // Intentionally left blank - preserves options default.
            }
        }

        private static readonly PassthroughHash Sut = new();

        [Fact]
        public void ComputeHash_ByteArray_ReturnsSameBytes()
        {
            var input = new byte[] { 1, 2, 3, 4 };
            var hr = Sut.ComputeHash(input);
            Assert.Equal(input, hr.GetBytes());
        }

        [Fact]
        public void ComputeHash_Bool_UsesConvertible()
        {
            var input = true;
            var expected = Convertible.GetBytes(input);
            var hr = Sut.ComputeHash(input);
            Assert.Equal(expected, hr.GetBytes());
        }

        [Fact]
        public void ComputeHash_Byte_UsesConvertible()
        {
            byte input = 0x7F;
            var expected = Convertible.GetBytes(input);
            var hr = Sut.ComputeHash(input);
            Assert.Equal(expected, hr.GetBytes());
        }

        [Fact]
        public void ComputeHash_Char_UsesConvertible()
        {
            char input = 'Z';
            var expected = Convertible.GetBytes(input);
            var hr = Sut.ComputeHash(input);
            Assert.Equal(expected, hr.GetBytes());
        }

        [Fact]
        public void ComputeHash_DateTime_UsesConvertible()
        {
            var input = new DateTime(2020, 10, 20, 23, 13, 40, DateTimeKind.Utc);
            var expected = Convertible.GetBytes(input);
            var hr = Sut.ComputeHash(input);
            Assert.Equal(expected, hr.GetBytes());
        }

        [Fact]
        public void ComputeHash_DBNull_UsesConvertible()
        {
            var input = DBNull.Value;
            var expected = Convertible.GetBytes(input);
            var hr = Sut.ComputeHash(input);
            Assert.Equal(expected, hr.GetBytes());
        }

        [Fact]
        public void ComputeHash_Decimal_UsesConvertible()
        {
            decimal input = 12345.6789m;
            var expected = Convertible.GetBytes(input);
            var hr = Sut.ComputeHash(input);
            Assert.Equal(expected, hr.GetBytes());
        }

        [Fact]
        public void ComputeHash_Double_UsesConvertible()
        {
            double input = Math.PI;
            var expected = Convertible.GetBytes(input);
            var hr = Sut.ComputeHash(input);
            Assert.Equal(expected, hr.GetBytes());
        }

        [Fact]
        public void ComputeHash_Short_UsesConvertible()
        {
            short input = -1234;
            var expected = Convertible.GetBytes(input);
            var hr = Sut.ComputeHash(input);
            Assert.Equal(expected, hr.GetBytes());
        }

        [Fact]
        public void ComputeHash_Int_UsesConvertible()
        {
            int input = 42;
            var expected = Convertible.GetBytes(input);
            var hr = Sut.ComputeHash(input);
            Assert.Equal(expected, hr.GetBytes());
        }

        [Fact]
        public void ComputeHash_Long_UsesConvertible()
        {
            long input = 12345678901234L;
            var expected = Convertible.GetBytes(input);
            var hr = Sut.ComputeHash(input);
            Assert.Equal(expected, hr.GetBytes());
        }

        [Fact]
        public void ComputeHash_SByte_UsesConvertible()
        {
            sbyte input = -12;
            var expected = Convertible.GetBytes(input);
            var hr = Sut.ComputeHash(input);
            Assert.Equal(expected, hr.GetBytes());
        }

        [Fact]
        public void ComputeHash_Float_UsesConvertible()
        {
            float input = 3.14f;
            var expected = Convertible.GetBytes(input);
            var hr = Sut.ComputeHash(input);
            Assert.Equal(expected, hr.GetBytes());
        }

        [Fact]
        public void ComputeHash_UShort_UsesConvertible()
        {
            ushort input = 65000;
            var expected = Convertible.GetBytes(input);
            var hr = Sut.ComputeHash(input);
            Assert.Equal(expected, hr.GetBytes());
        }

        [Fact]
        public void ComputeHash_UInt_UsesConvertible()
        {
            uint input = 0xDEADBEEF;
            var expected = Convertible.GetBytes(input);
            var hr = Sut.ComputeHash(input);
            Assert.Equal(expected, hr.GetBytes());
        }

        [Fact]
        public void ComputeHash_ULong_UsesConvertible()
        {
            ulong input = 0xDEADBEEFCAFEBABEUL;
            var expected = Convertible.GetBytes(input);
            var hr = Sut.ComputeHash(input);
            Assert.Equal(expected, hr.GetBytes());
        }

        private enum TestEnum : byte { Zero = 0, One = 1 }

        [Fact]
        public void ComputeHash_String_UsesConvertible_DefaultEncoding()
        {
            var input = "hello world";
            var expected = Convertible.GetBytes(input);
            var hr = Sut.ComputeHash(input);
            Assert.Equal(expected, hr.GetBytes());
        }

        [Fact]
        public void ComputeHash_Enum_UsesConvertible()
        {
            var input = TestEnum.One;
            var expected = Convertible.GetBytes(input);
            var hr = Sut.ComputeHash(input);
            Assert.Equal(expected, hr.GetBytes());
        }

        [Fact]
        public void ComputeHash_ParamsIConvertible_AggregatesValues()
        {
            IConvertible[] input = { 1, "abc", (short)7 };
            var expected = Convertible.GetBytes(Arguments.ToEnumerableOf(input));
            var hr = Sut.ComputeHash(input);
            Assert.Equal(expected, hr.GetBytes());
        }

        [Fact]
        public void ComputeHash_IEnumerableIConvertible_AggregatesValues()
        {
            var input = new List<IConvertible> { 2, "xyz", 9m };
            var expected = Convertible.GetBytes(input);
            var hr = Sut.ComputeHash((IEnumerable<IConvertible>)input);
            Assert.Equal(expected, hr.GetBytes());
        }

        [Fact]
        public void ComputeHash_Stream_CopiesStreamAndComputes()
        {
            var payload = new byte[] { 11, 22, 33, 44, 55 };
            using var ms = new MemoryStream(payload);
            // rewind to ensure stream is readable from start
            ms.Position = 0;

            var hr = Sut.ComputeHash(ms);

            // underlying implementation copies stream content into a MemoryStream and then ComputeHash(byte[]) is invoked.
            Assert.Equal(payload, hr.GetBytes());
        }

        private sealed class TestHash : Hash<Cuemon.ConvertibleOptions>
        {
            public TestHash(Action<Cuemon.ConvertibleOptions> setup = null) : base(setup)
            {
            }

            public override HashResult ComputeHash(byte[] input)
            {
                // For testing we simply wrap the incoming byte[] into a HashResult
                return new HashResult(input);
            }
        }

        [Fact]
        public void Ctor_ShouldConfigureOptions()
        {
            var sut = new TestHash(o => o.ByteOrder = Endianness.BigEndian);
            Assert.NotNull(sut.Options);
            Assert.Equal(Endianness.BigEndian, sut.Options.ByteOrder);
        }

        [Fact]
        public void ComputeHash_Int_ShouldRespectEndianInitializer()
        {
            // Configure to big endian explicitly
            var sut = new TestHash(o => o.ByteOrder = Endianness.BigEndian);

            var value = 0x01020304;
            var hr = sut.ComputeHash(value);

            var expected = Cuemon.Convertible.GetBytes(value, o => o.ByteOrder = sut.Options.ByteOrder);

            Assert.Equal(expected, hr.GetBytes());
        }

        [Fact]
        public void ComputeHash_Stream_ShouldReturnStreamBytes()
        {
            var sut = new TestHash();

            var bytes = new byte[] { 0x10, 0x20, 0x30, 0x40 };
            using var ms = new MemoryStream(bytes);

            var hr = sut.ComputeHash(ms);

            Assert.Equal(bytes, hr.GetBytes());
        }

        [Fact]
        public void ComputeHash_ParamsAndEnumerable_ShouldAggregateBytes()
        {
            var sut = new TestHash(o => o.ByteOrder = Endianness.BigEndian);

            var hrFromParams = sut.ComputeHash(1, "A", true);
            var hrFromEnumerable = sut.ComputeHash(Arguments.ToEnumerableOf<IConvertible>(1, "A", true));

            var expected = Convertible.GetBytes(Arguments.ToEnumerableOf<IConvertible>(1, "A", true));

            Assert.Equal(expected, hrFromParams.GetBytes());
            Assert.Equal(expected, hrFromEnumerable.GetBytes());
        }

        [Fact]
        public void ComputeHash_AllOverloads_ShouldReturnExpectedBytes()
        {
            var sut = new TestHash();

            var dt = new DateTime(2020, 01, 02, 03, 04, 05, DateTimeKind.Utc);
            var dec = 1234.5678m;
            var dbl = 1234.5678d;
            var flt = 1234.5f;
            var sb = (sbyte)-5;
            var us = (ushort)0xABCD;
            var ui = (uint)0xDEADBEEF;
            var ul = (ulong)0x0123456789ABCDEF;
            var ch = 'X';
            var b = (byte)0x7F;
            var sh = (short)0x1234;
            var lng = (long)0x0102030405060708;
            var boolean = true;
            var enumVal = DayOfWeek.Wednesday;
            var str = "hello";
            var dbnull = DBNull.Value;

            var cases = new List<(Func<HashResult> call, Func<byte[]> expected)>
            {
                (() => sut.ComputeHash(boolean), () => Convertible.GetBytes(boolean, o => o.ByteOrder = sut.Options.ByteOrder)),
                (() => sut.ComputeHash(b), () => Convertible.GetBytes(b, o => o.ByteOrder = sut.Options.ByteOrder)),
                (() => sut.ComputeHash(ch), () => Convertible.GetBytes(ch, o => o.ByteOrder = sut.Options.ByteOrder)),
                (() => sut.ComputeHash(dt), () => Convertible.GetBytes(dt)),
                (() => sut.ComputeHash(dbnull), () => Convertible.GetBytes(dbnull)),
                (() => sut.ComputeHash(dec), () => Convertible.GetBytes(dec)),
                (() => sut.ComputeHash(dbl), () => Convertible.GetBytes(dbl, o => o.ByteOrder = sut.Options.ByteOrder)),
                (() => sut.ComputeHash(sh), () => Convertible.GetBytes(sh, o => o.ByteOrder = sut.Options.ByteOrder)),
                (() => sut.ComputeHash(12345), () => Convertible.GetBytes(12345, o => o.ByteOrder = sut.Options.ByteOrder)),
                (() => sut.ComputeHash(lng), () => Convertible.GetBytes(lng, o => o.ByteOrder = sut.Options.ByteOrder)),
                (() => sut.ComputeHash(sb), () => Convertible.GetBytes(sb, o => o.ByteOrder = sut.Options.ByteOrder)),
                (() => sut.ComputeHash(flt), () => Convertible.GetBytes(flt, o => o.ByteOrder = sut.Options.ByteOrder)),
                (() => sut.ComputeHash(us), () => Convertible.GetBytes(us, o => o.ByteOrder = sut.Options.ByteOrder)),
                (() => sut.ComputeHash(ui), () => Convertible.GetBytes(ui, o => o.ByteOrder = sut.Options.ByteOrder)),
                (() => sut.ComputeHash(ul), () => Convertible.GetBytes(ul, o => o.ByteOrder = sut.Options.ByteOrder)),
                (() => sut.ComputeHash(str, o => o.Encoding = Encoding.UTF8), () => Convertible.GetBytes(str, o => o.Encoding = Encoding.UTF8)),
                (() => sut.ComputeHash(enumVal), () => Convertible.GetBytes(enumVal, o => o.ByteOrder = sut.Options.ByteOrder)),
                (() => sut.ComputeHash(new byte[] { 1, 2, 3 }), () => new byte[] { 1, 2, 3 }), // ComputeHash(byte[]) implemented to return raw bytes
            };

            foreach (var (call, expected) in cases)
            {
                var result = call();
                var expectedBytes = expected();
                Assert.Equal(expectedBytes, result.GetBytes());
            }
        }
    }
}
