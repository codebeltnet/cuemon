using System;
using System.Globalization;
#if NET10_0_OR_GREATER
using System.Buffers;
#endif
using System.IO;
using System.Text;
using Codebelt.Extensions.Xunit;
using Cuemon.Text;
using Xunit;

namespace Cuemon.IO
{
    public class StreamOptionsTest : Test
    {
        public StreamOptionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void StreamCopyOptions_ShouldHaveExpectedDefaultsAndValidateBufferSize()
        {
            var sut = new StreamCopyOptions();

            Assert.False(sut.LeaveOpen);
            Assert.Equal(81920, sut.BufferSize);
            Assert.Throws<ArgumentOutOfRangeException>(() => sut.BufferSize = 0);
        }

        [Fact]
        public void StreamEncodingOptions_ShouldHaveExpectedDefaults()
        {
            var sut = new StreamEncodingOptions();

            Assert.Equal(EncodingOptions.DefaultEncoding, sut.Encoding);
            Assert.Equal(EncodingOptions.DefaultPreambleSequence, sut.Preamble);
            Assert.False(sut.LeaveOpen);
        }

        [Fact]
        public void StreamWriterOptions_ShouldHaveExpectedDefaultsAndAllowCustomization()
        {
            var sut = new StreamWriterOptions();
            var culture = CultureInfo.GetCultureInfo("da-DK");

            Assert.False(sut.AutoFlush);
            Assert.Equal(1024, sut.BufferSize);
            Assert.Equal(EncodingOptions.DefaultEncoding, sut.Encoding);
            Assert.Equal(PreambleSequence.Keep, sut.Preamble);
            Assert.Equal(CultureInfo.InvariantCulture, sut.FormatProvider);
            Assert.Equal(Environment.NewLine, sut.NewLine);

            sut.AutoFlush = true;
            sut.BufferSize = 256;
            sut.Encoding = Encoding.Unicode;
            sut.Preamble = PreambleSequence.Remove;
            sut.FormatProvider = culture;
            sut.NewLine = "\n";

            Assert.True(sut.AutoFlush);
            Assert.Equal(256, sut.BufferSize);
            Assert.Equal(Encoding.Unicode, sut.Encoding);
            Assert.Equal(PreambleSequence.Remove, sut.Preamble);
            Assert.Equal(culture, sut.FormatProvider);
            Assert.Equal("\n", sut.NewLine);
        }

        [Fact]
        public void StreamReaderOptions_ShouldHaveExpectedDefaultsAndAllowCustomization()
        {
            var sut = new StreamReaderOptions();

            Assert.Equal(81920, sut.BufferSize);
            Assert.Equal(EncodingOptions.DefaultEncoding, sut.Encoding);
            Assert.Equal(EncodingOptions.DefaultPreambleSequence, sut.Preamble);

            sut.BufferSize = 2048;
            sut.Encoding = Encoding.Unicode;
            sut.Preamble = PreambleSequence.Remove;

            Assert.Equal(2048, sut.BufferSize);
            Assert.Equal(Encoding.Unicode, sut.Encoding);
            Assert.Equal(PreambleSequence.Remove, sut.Preamble);
        }

#if NET10_0_OR_GREATER

        [Fact]
        public void BufferWriterOptions_ShouldHaveExpectedDefaultsAndAllowCustomization()
        {
            var sut = new BufferWriterOptions();

            Assert.Equal(256, sut.BufferSize);
            Assert.Equal(EncodingOptions.DefaultEncoding, sut.Encoding);
            Assert.Equal(PreambleSequence.Keep, sut.Preamble);
            Assert.False(sut.LeaveOpen);

            sut.BufferSize = 32;
            sut.Encoding = Encoding.Unicode;
            sut.Preamble = PreambleSequence.Remove;

            Assert.Equal(32, sut.BufferSize);
            Assert.Equal(Encoding.Unicode, sut.Encoding);
            Assert.Equal(PreambleSequence.Remove, sut.Preamble);
        }

#endif

        [Fact]
        public void FileInfoOptions_ShouldHaveExpectedDefaultsAndValidateBytesToRead()
        {
            var sut = new FileInfoOptions();

            Assert.Equal(0, sut.BytesToRead);

            sut.BytesToRead = 128;

            Assert.Equal(128, sut.BytesToRead);
            Assert.Throws<ArgumentOutOfRangeException>(() => sut.BytesToRead = -1);
        }
    }
}
