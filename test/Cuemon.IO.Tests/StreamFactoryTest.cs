using System;
using System.Globalization;
using System.IO;
using System.Linq;
#if NET10_0_OR_GREATER
using System.Buffers;
#endif
using System.Text;
using Codebelt.Extensions.Xunit;
using Cuemon.Text;
using Xunit;

namespace Cuemon.IO;
public class StreamFactoryTest : Test
{
    public StreamFactoryTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Create_ShouldWriteContent_WhenUsingActionOfStreamWriter()
    {
        var culture = CultureInfo.GetCultureInfo("en-US");
        var sut = StreamFactory.Create(writer =>
        {
            Assert.Same(culture, writer.FormatProvider);
            Assert.True(writer.AutoFlush);
            Assert.Equal("\n", writer.NewLine);
            writer.Write("{0:N2}", 42.5m);
            writer.WriteLine();
            writer.Write("done");
        }, o =>
        {
            o.AutoFlush = true;
            o.FormatProvider = culture;
            o.NewLine = "\n";
            o.Encoding = new UTF8Encoding(true);
        });

        Assert.Equal("42.50\ndone", ReadAsString(sut));
    }

    [Fact]
    public void Create_ShouldWriteContent_WhenUsingActionOfStreamWriterAndOneArgument()
    {
        var sut = StreamFactory.Create((writer, value) => writer.Write($"arg:{value}"), 42);

        Assert.Equal("arg:42", ReadAsString(sut));
    }

    [Fact]
    public void Create_ShouldWriteContent_WhenUsingActionOfStreamWriterAndTwoArguments()
    {
        var sut = StreamFactory.Create((writer, prefix, value) => writer.Write($"{prefix}:{value}"), "arg", 42);

        Assert.Equal("arg:42", ReadAsString(sut));
    }

    [Fact]
    public void Create_ShouldWriteContent_WhenUsingActionOfStreamWriterAndThreeArguments()
    {
        var sut = StreamFactory.Create((writer, a, b, c) => writer.Write($"{a}:{b}:{c}"), "a", "b", "c");

        Assert.Equal("a:b:c", ReadAsString(sut));
    }

    [Fact]
    public void Create_ShouldWriteContent_WhenUsingActionOfStreamWriterAndFourArguments()
    {
        var sut = StreamFactory.Create((writer, a, b, c, d) => writer.Write($"{a}:{b}:{c}:{d}"), "a", "b", "c", "d");

        Assert.Equal("a:b:c:d", ReadAsString(sut));
    }

    [Fact]
    public void Create_ShouldWriteContent_WhenUsingActionOfStreamWriterAndFiveArguments()
    {
        var sut = StreamFactory.Create((writer, a, b, c, d, e) => writer.Write($"{a}:{b}:{c}:{d}:{e}"), "a", "b", "c", "d", "e");

        Assert.Equal("a:b:c:d:e", ReadAsString(sut));
    }

    [Fact]
    public void Create_ShouldRemovePreamble_WhenConfiguredForStreamWriter()
    {
        var encoding = Encoding.Unicode;
        var sut = StreamFactory.Create(writer => writer.Write("hello world"), o =>
        {
            o.Encoding = encoding;
            o.Preamble = PreambleSequence.Remove;
        });
        var bytes = Decorator.Enclose(sut).ToByteArray(o => o.LeaveOpen = true);
        var preamble = encoding.GetPreamble();

        Assert.Equal("hello world", ReadAsString(sut, encoding));
        Assert.False(bytes.Take(preamble.Length).SequenceEqual(preamble));
    }

    [Fact]
    public void Create_ShouldWrapExceptionsFromWriterDelegate()
    {
        var ex = Assert.Throws<InvalidOperationException>(() => StreamFactory.Create((StreamWriter writer) => throw new FormatException("boom")));

        Assert.Equal("There is an error in the Stream being written.", ex.Message);
        Assert.IsType<FormatException>(ex.InnerException);
        Assert.Equal("boom", ex.InnerException.Message);
    }

#if NET10_0_OR_GREATER

    [Fact]
    public void Create_ShouldWriteContent_WhenUsingActionOfBufferWriter()
    {
        var encoding = new UTF8Encoding(true);
        var sut = StreamFactory.Create(writer => WriteBuffer(writer, encoding, "arg:zero", true), o =>
        {
            o.BufferSize = 8;
            o.Encoding = encoding;
            o.Preamble = PreambleSequence.Remove;
        });
        var bytes = Decorator.Enclose(sut).ToByteArray(o => o.LeaveOpen = true);
        var preamble = encoding.GetPreamble();

        Assert.Equal("arg:zero", ReadAsString(sut, encoding));
        Assert.False(bytes.Take(preamble.Length).SequenceEqual(preamble));
    }

    [Fact]
    public void Create_ShouldWriteContent_WhenUsingActionOfBufferWriterAndOneArgument()
    {
        var sut = StreamFactory.Create((writer, value) => WriteBuffer(writer, Encoding.UTF8, $"arg:{value}"), 42);

        Assert.Equal("arg:42", ReadAsString(sut));
    }

    [Fact]
    public void Create_ShouldWriteContent_WhenUsingActionOfBufferWriterAndTwoArguments()
    {
        var sut = StreamFactory.Create((writer, prefix, value) => WriteBuffer(writer, Encoding.UTF8, $"{prefix}:{value}"), "arg", 42);

        Assert.Equal("arg:42", ReadAsString(sut));
    }

    [Fact]
    public void Create_ShouldWriteContent_WhenUsingActionOfBufferWriterAndThreeArguments()
    {
        var sut = StreamFactory.Create((writer, a, b, c) => WriteBuffer(writer, Encoding.UTF8, $"{a}:{b}:{c}"), "a", "b", "c");

        Assert.Equal("a:b:c", ReadAsString(sut));
    }

    [Fact]
    public void Create_ShouldWriteContent_WhenUsingActionOfBufferWriterAndFourArguments()
    {
        var sut = StreamFactory.Create((writer, a, b, c, d) => WriteBuffer(writer, Encoding.UTF8, $"{a}:{b}:{c}:{d}"), "a", "b", "c", "d");

        Assert.Equal("a:b:c:d", ReadAsString(sut));
    }

    [Fact]
    public void Create_ShouldWriteContent_WhenUsingActionOfBufferWriterAndFiveArguments()
    {
        var sut = StreamFactory.Create((writer, a, b, c, d, e) => WriteBuffer(writer, Encoding.UTF8, $"{a}:{b}:{c}:{d}:{e}"), "a", "b", "c", "d", "e");

        Assert.Equal("a:b:c:d:e", ReadAsString(sut));
    }

    private static void WriteBuffer(IBufferWriter<byte> writer, Encoding encoding, string value, bool includePreamble = false)
    {
        if (includePreamble)
        {
            WriteBytes(writer, encoding.GetPreamble());
        }

        WriteBytes(writer, encoding.GetBytes(value));
    }

    private static void WriteBytes(IBufferWriter<byte> writer, byte[] bytes)
    {
        var span = writer.GetSpan(bytes.Length);
        bytes.AsSpan().CopyTo(span);
        writer.Advance(bytes.Length);
    }

#endif

    private static string ReadAsString(Stream stream, Encoding encoding = null)
    {
        return Decorator.Enclose(stream).ToEncodedString(o =>
        {
            o.Encoding = encoding ?? Encoding.UTF8;
            o.LeaveOpen = true;
            o.Preamble = PreambleSequence.Remove;
        });
    }
}
