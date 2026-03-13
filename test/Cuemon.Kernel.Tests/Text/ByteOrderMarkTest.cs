using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Text
{
    public class ByteOrderMarkTest : Test
    {
        public static IEnumerable<object[]> GetKnownByteOrderMarks()
        {
            yield return new object[] { Encoding.UTF8.GetPreamble(), Encoding.UTF8.CodePage, Encoding.UTF8.GetPreamble() };
            yield return new object[] { new UTF32Encoding(true, true).GetPreamble(), Encoding.GetEncoding("UTF-32BE").CodePage, new UTF32Encoding(true, true).GetPreamble() };
            yield return new object[] { Encoding.UTF32.GetPreamble(), Encoding.UTF32.CodePage, Encoding.UTF32.GetPreamble() };
            yield return new object[] { Encoding.BigEndianUnicode.GetPreamble(), Encoding.BigEndianUnicode.CodePage, Encoding.BigEndianUnicode.GetPreamble() };
            yield return new object[] { Encoding.Unicode.GetPreamble(), Encoding.Unicode.CodePage, Encoding.Unicode.GetPreamble() };
        }

        public static IEnumerable<object[]> GetUnknownByteOrderMarks()
        {
            yield return new object[] { Array.Empty<byte>() };
            yield return new object[] { new byte[] { 0xEF, 0xBB } };
            yield return new object[] { new byte[] { 0x00, 0x00, 0xFE } };
            yield return new object[] { new byte[] { 0x01 } };
            yield return new object[] { new byte[] { 0xAA, 0xBB, 0xCC, 0xDD } };
        }

        [Theory]
        [MemberData(nameof(GetKnownByteOrderMarks))]
        public void Decode_ShouldReturnEncoding_WhenByteOrderMarkIsKnown(byte[] bytes, int expectedCodePage, byte[] expectedPreamble)
        {
            var result = ByteOrderMark.Decode(bytes);

            Assert.Equal(expectedCodePage, result.CodePage);
            Assert.Equal(expectedPreamble, result.GetPreamble());
        }

        [Theory]
        [MemberData(nameof(GetUnknownByteOrderMarks))]
        public void Decode_ShouldThrowArgumentException_WhenByteOrderMarkIsUnknown(byte[] bytes)
        {
            Assert.Throws<ArgumentException>(() => ByteOrderMark.Decode(bytes));
        }

        [Fact]
        public void Decode_ShouldThrowArgumentNullException_WhenBytesIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => ByteOrderMark.Decode(null));
        }

        [Fact]
        public void DetectEncodingOrDefault_ShouldReturnDetectedEncoding_WhenByteArrayContainsPreamble()
        {
            var result = ByteOrderMark.DetectEncodingOrDefault(Encoding.UTF8.GetPreamble(), Encoding.Unicode);

            Assert.Equal(Encoding.UTF8.CodePage, result.CodePage);
        }

        [Fact]
        public void DetectEncodingOrDefault_ShouldReturnFallbackEncoding_WhenByteArrayDoesNotContainPreamble()
        {
            var fallback = Encoding.BigEndianUnicode;

            var result = ByteOrderMark.DetectEncodingOrDefault(new byte[] { 0x01, 0x02, 0x03 }, fallback);

            Assert.Same(fallback, result);
        }

        [Fact]
        public void DetectEncodingOrDefault_ShouldReturnDefaultEncoding_WhenByteArrayDoesNotContainPreambleAndFallbackIsNull()
        {
            var result = ByteOrderMark.DetectEncodingOrDefault(new byte[] { 0x01, 0x02, 0x03 }, null);

            Assert.Same(EncodingOptions.DefaultEncoding, result);
        }

        [Fact]
        public void DetectEncodingOrDefault_ShouldReturnDetectedEncoding_WhenStreamContainsPreamble()
        {
            using var stream = new MemoryStream(Encoding.Unicode.GetPreamble());

            var result = ByteOrderMark.DetectEncodingOrDefault(stream, Encoding.UTF8);

            Assert.Equal(Encoding.Unicode.CodePage, result.CodePage);
        }

        [Fact]
        public void DetectEncodingOrDefault_ShouldReturnFallbackEncoding_WhenStreamDoesNotContainPreamble()
        {
            var fallback = Encoding.UTF8;
            using var stream = new MemoryStream(new byte[] { 0x01, 0x02, 0x03, 0x04 });

            var result = ByteOrderMark.DetectEncodingOrDefault(stream, fallback);

            Assert.Same(fallback, result);
        }

        [Fact]
        public void TryDetectEncoding_ShouldReturnTrueAndEncoding_WhenByteArrayContainsPreamble()
        {
            var result = ByteOrderMark.TryDetectEncoding(Encoding.BigEndianUnicode.GetPreamble(), out var encoding);

            Assert.True(result);
            Assert.NotNull(encoding);
            Assert.Equal(Encoding.BigEndianUnicode.CodePage, encoding.CodePage);
        }

        [Fact]
        public void TryDetectEncoding_ShouldReturnFalseAndNull_WhenByteArrayDoesNotContainPreamble()
        {
            var result = ByteOrderMark.TryDetectEncoding(new byte[] { 0x01 }, out var encoding);

            Assert.False(result);
            Assert.Null(encoding);
        }

        [Fact]
        public void TryDetectEncoding_ShouldReturnFalseAndNull_WhenStreamIsNull()
        {
            var result = ByteOrderMark.TryDetectEncoding((Stream)null, out var encoding);

            Assert.False(result);
            Assert.Null(encoding);
        }

        [Fact]
        public void TryDetectEncoding_ShouldReturnFalseAndNull_WhenStreamCannotSeek()
        {
            using var stream = new NonSeekableMemoryStream(Encoding.UTF8.GetPreamble());

            var result = ByteOrderMark.TryDetectEncoding(stream, out var encoding);

            Assert.False(result);
            Assert.Null(encoding);
        }

        [Fact]
        public void TryDetectEncoding_ShouldPreserveStreamPosition_WhenStreamCanSeek()
        {
            var bytes = CreateByteArrayWithPreamble(Encoding.UTF8, 0x41, 0x42, 0x43);
            using var stream = new MemoryStream(bytes);
            stream.Position = 2;

            var result = ByteOrderMark.TryDetectEncoding(stream, out var encoding);

            Assert.True(result);
            Assert.NotNull(encoding);
            Assert.Equal(Encoding.UTF8.CodePage, encoding.CodePage);
            Assert.Equal(2, stream.Position);
        }

        [Fact]
        public void Remove_ShouldThrowArgumentNullException_WhenStreamIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => ByteOrderMark.Remove((Stream)null, Encoding.UTF8));
        }

        [Fact]
        public void Remove_ShouldThrowArgumentNullException_WhenStreamEncodingIsNull()
        {
            using var stream = new MemoryStream();

            Assert.Throws<ArgumentNullException>(() => ByteOrderMark.Remove(stream, null));
        }

        [Fact]
        public void Remove_ShouldReturnMemoryStreamWithoutPreamble_AndDisposeInputByDefault()
        {
            var source = new MemoryStream(CreateByteArrayWithPreamble(Encoding.UTF8, 0x41, 0x42, 0x43));

            using var result = ByteOrderMark.Remove(source, Encoding.UTF8);

            Assert.IsType<MemoryStream>(result);
            Assert.Equal(0, result.Position);
            Assert.Equal(new byte[] { 0x41, 0x42, 0x43 }, ReadAllBytes(result));
            Assert.Throws<ObjectDisposedException>(() => source.ReadByte());
        }

        [Fact]
        public void Remove_ShouldLeaveInputOpen_WhenConfiguredToDoSo()
        {
            using var source = new MemoryStream(CreateByteArrayWithPreamble(Encoding.UTF8, 0x41, 0x42));

            using var result = ByteOrderMark.Remove(source, Encoding.UTF8, o => o.LeaveOpen = true);

            Assert.Equal(new byte[] { 0x41, 0x42 }, ReadAllBytes(result));
            Assert.True(source.CanRead);
            source.Position = 0;
            Assert.Equal(0xEF, source.ReadByte());
        }

        [Fact]
        public void Remove_ShouldThrowArgumentNullException_WhenBytesIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => ByteOrderMark.Remove((byte[])null, Encoding.UTF8));
        }

        [Fact]
        public void Remove_ShouldThrowArgumentNullException_WhenByteArrayEncodingIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => ByteOrderMark.Remove(new byte[] { 0x01 }, null));
        }

        [Fact]
        public void Remove_ShouldReturnSameReference_WhenByteArrayLengthIsLessThanTwo()
        {
            var bytes = new byte[] { 0xEF };

            var result = ByteOrderMark.Remove(bytes, Encoding.UTF8);

            Assert.Same(bytes, result);
        }

        [Fact]
        public void Remove_ShouldReturnSameReference_WhenEncodingHasNoPreamble()
        {
            var bytes = new byte[] { 0x41, 0x42, 0x43 };
            var encoding = new UTF8Encoding(false);

            var result = ByteOrderMark.Remove(bytes, encoding);

            Assert.Same(bytes, result);
        }

        [Fact]
        public void Remove_ShouldReturnSameReference_WhenByteArrayIsShorterThanPreamble()
        {
            var bytes = new byte[] { 0xEF, 0xBB };

            var result = ByteOrderMark.Remove(bytes, Encoding.UTF8);

            Assert.Same(bytes, result);
        }

        [Fact]
        public void Remove_ShouldReturnSameReference_WhenByteArrayDoesNotStartWithExactPreamble()
        {
            var bytes = new byte[] { 0xBB, 0xEF, 0xBF, 0x41 };

            var result = ByteOrderMark.Remove(bytes, Encoding.UTF8);

            Assert.Same(bytes, result);
        }

        [Fact]
        public void Remove_ShouldStripPreamble_WhenByteArrayStartsWithExactPreamble()
        {
            var bytes = CreateByteArrayWithPreamble(Encoding.UTF8, 0x41, 0x42, 0x43);

            var result = ByteOrderMark.Remove(bytes, Encoding.UTF8);

            Assert.Equal(new byte[] { 0x41, 0x42, 0x43 }, result);
        }

        private static byte[] CreateByteArrayWithPreamble(Encoding encoding, params byte[] content)
        {
            var preamble = encoding.GetPreamble();
            var bytes = new byte[preamble.Length + content.Length];
            Array.Copy(preamble, 0, bytes, 0, preamble.Length);
            Array.Copy(content, 0, bytes, preamble.Length, content.Length);
            return bytes;
        }

        private static byte[] ReadAllBytes(Stream stream)
        {
            using var copy = new MemoryStream();
            stream.CopyTo(copy);
            return copy.ToArray();
        }

        private sealed class NonSeekableMemoryStream : MemoryStream
        {
            public NonSeekableMemoryStream(byte[] buffer) : base(buffer)
            {
            }

            public override bool CanSeek => false;
        }
    }
}
