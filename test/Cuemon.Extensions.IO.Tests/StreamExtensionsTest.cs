using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Cuemon.Text;
using Xunit;

namespace Cuemon.Extensions.IO;
public class StreamExtensionsTest : Test
{
    public StreamExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Concat_ShouldConcatenateASetOfStreamsIntoOneStream()
    {
        var sut1 = "C".ToStream();
        var sut2 = "u".ToStream();
        var sut3 = "e".ToStream();
        var sut4 = "m".ToStream();
        var sut5 = "o".ToStream();
        var sut6 = "n".ToStream();
        var sut7 = sut1.Concat(sut2).Concat(sut3).Concat(sut4).Concat(sut5).Concat(sut6);
        var sut8 = sut7.ToEncodedString();

        Assert.Equal("Cuemon", sut8);
        Assert.Throws<ObjectDisposedException>(() => sut1.Position);
        Assert.Throws<ObjectDisposedException>(() => sut2.Position);
        Assert.Throws<ObjectDisposedException>(() => sut3.Position);
        Assert.Throws<ObjectDisposedException>(() => sut4.Position);
        Assert.Throws<ObjectDisposedException>(() => sut5.Position);
        Assert.Throws<ObjectDisposedException>(() => sut6.Position);
        Assert.Throws<ObjectDisposedException>(() => sut7.Position);
    }

    [Fact]
    public void ToCharArray_ShouldConvertStreamToCharArray()
    {
        var sut1 = $"This is a string that will be converted back and forth. Lets add some foreign characters: æøå and some punctuations as well: {Alphanumeric.PunctuationMarks}.";
        var sut2 = sut1.ToStream();
        var sut3 = sut2.ToCharArray();
        var sut4 = new string(sut3);

        Assert.Equal(sut1, sut4);
        Assert.Throws<ObjectDisposedException>(() => sut2.Position);
    }

    [Fact]
    public void ToByteArray_ShouldConvertStreamToByteArray()
    {
        var sut1 = $"This is a string that will be converted back and forth. Lets add some foreign characters: æøå and some punctuations as well: {Alphanumeric.PunctuationMarks}.";
        var sut2 = sut1.ToStream();
        var sut3 = sut2.ToByteArray();
        var sut4 = sut3.ToEncodedString();
        var sut5 = sut4.ToByteArray();

        Assert.Equal(sut1, sut4);
        Assert.Equal(sut3, sut5);
        Assert.Throws<ObjectDisposedException>(() => sut2.Position);
    }

    [Fact]
    public async Task ToByteArrayAsync_ShouldConvertStreamToByteArrayAsync()
    {
        var sut1 = $"This is a string that will be converted back and forth. Lets add some foreign characters: æøå and some punctuations as well: {Alphanumeric.PunctuationMarks}.";
        var sut2 = await sut1.ToStreamAsync();
        var sut3 = await sut2.ToByteArrayAsync();
        var sut4 = sut3.ToEncodedString();
        var sut5 = sut4.ToByteArray();

        Assert.Equal(sut1, sut4);
        Assert.Equal(sut3, sut5);
        Assert.Throws<ObjectDisposedException>(() => sut2.Position);
    }

    [Fact]
    public async Task WriteAllAsync_ShouldWriteByteArrayToStreamAsync()
    {
        var sut1 = $"This is a string that will be converted back and forth. Lets add some foreign characters: æøå and some punctuations as well: {Alphanumeric.PunctuationMarks}.";
        var sut2 = sut1.ToByteArray();
        var sut3 = new MemoryStream();
        await sut3.WriteAllAsync(sut2);
        var sut4 = sut3.ToByteArray();
        var sut5 = sut4.ToEncodedString();

        Assert.Equal(sut1, sut5);
        Assert.Equal(sut2, sut4);
        Assert.Throws<ObjectDisposedException>(() => sut3.Position);
    }

    [Fact]
    public void TryDetectUnicodeEncoding_ShouldDetectUnicodeEncodings()
    {
        var sut1 = $"This is a string that will be converted back and forth. Lets add some foreign characters: æøå and some punctuations as well: {Alphanumeric.PunctuationMarks}.";
        var sut2 = sut1.ToStream(o => o.Preamble = PreambleSequence.Keep);
        sut2.TryDetectUnicodeEncoding(out var sut3);
        var sut4 = sut1.ToStream(o =>
        {
            o.Encoding = Encoding.Unicode;
            o.Preamble = PreambleSequence.Keep;
        });
        sut4.TryDetectUnicodeEncoding(out var sut5);
        var sut6 = sut1.ToStream(o =>
        {
            o.Encoding = Encoding.BigEndianUnicode;
            o.Preamble = PreambleSequence.Keep;
        });
        sut6.TryDetectUnicodeEncoding(out var sut7);
        var sut8 = sut1.ToStream(o =>
        {
            o.Encoding = Encoding.UTF32;
            o.Preamble = PreambleSequence.Keep;
        });
        sut8.TryDetectUnicodeEncoding(out var sut9);
        var sut10 = sut1.ToStream(o =>
        {
            o.Encoding = Encoding.GetEncoding("UTF-32BE");
            o.Preamble = PreambleSequence.Keep;
        });
        sut10.TryDetectUnicodeEncoding(out var sut11);

        Assert.Equal(Encoding.UTF8, sut3);
        Assert.Equal(Encoding.Unicode, sut5);
        Assert.Equal(Encoding.BigEndianUnicode, sut7);
        Assert.Equal(Encoding.UTF32, sut9);
        Assert.Equal(Encoding.GetEncoding("UTF-32BE"), sut11);
    }

    [Fact]
    public void ToEncodedString_ShouldConvertStreamToUnicodeEncodedString()
    {
        var sut1 = $"This is a string that will be converted back and forth. Lets add some foreign characters: æøå and some punctuations as well: {Alphanumeric.PunctuationMarks}.";
        var sut2 = sut1.ToStream(o => o.Preamble = PreambleSequence.Keep);
        var sut3 = sut1.ToStream(o =>
        {
            o.Encoding = Encoding.Unicode;
            o.Preamble = PreambleSequence.Keep;
        });
        var sut4 = sut1.ToStream(o =>
        {
            o.Encoding = Encoding.BigEndianUnicode;
            o.Preamble = PreambleSequence.Keep;
        });
        var sut5 = sut1.ToStream(o =>
        {
            o.Encoding = Encoding.UTF32;
            o.Preamble = PreambleSequence.Keep;
        });
        var sut6 = sut1.ToStream(o =>
        {
            o.Encoding = Encoding.GetEncoding("UTF-32BE");
            o.Preamble = PreambleSequence.Keep;
        });
        var sut7 = sut2.ToEncodedString(o => o.LeaveOpen = true);
        var sut8 = sut3.ToEncodedString(o => o.LeaveOpen = true);
        var sut9 = sut4.ToEncodedString(o => o.LeaveOpen = true);
        var sut10 = sut5.ToEncodedString(o => o.LeaveOpen = true);
        var sut11 = sut6.ToEncodedString(o => o.LeaveOpen = true);

        Assert.Equal(sut1, sut7);
        Assert.Equal(sut1, sut8);
        Assert.Equal(sut1, sut9);
        Assert.Equal(sut1, sut10);
        Assert.Equal(sut1, sut11);

        Assert.Equal(164, sut2.Length);
        Assert.Equal(318, sut3.Length);
        Assert.Equal(318, sut4.Length);
        Assert.Equal(636, sut5.Length);
        Assert.Equal(636, sut6.Length);
    }

    [Fact]
    public async Task ToEncodedStringAsync_ShouldConvertStreamToUnicodeEncodedStringAsync()
    {
        var sut1 = $"This is a string that will be converted back and forth. Lets add some foreign characters: æøå and some punctuations as well: {Alphanumeric.PunctuationMarks}.";
        var sut2 = await sut1.ToStreamAsync(o => o.Preamble = PreambleSequence.Keep);
        var sut3 = await sut1.ToStreamAsync(o =>
        {
            o.Encoding = Encoding.Unicode;
            o.Preamble = PreambleSequence.Keep;
        });
        var sut4 = await sut1.ToStreamAsync(o =>
        {
            o.Encoding = Encoding.BigEndianUnicode;
            o.Preamble = PreambleSequence.Keep;
        });
        var sut5 = await sut1.ToStreamAsync(o =>
        {
            o.Encoding = Encoding.UTF32;
            o.Preamble = PreambleSequence.Keep;
        });
        var sut6 = await sut1.ToStreamAsync(o =>
        {
            o.Encoding = Encoding.GetEncoding("UTF-32BE");
            o.Preamble = PreambleSequence.Keep;
        });
        var sut7 = await sut2.ToEncodedStringAsync(o => o.LeaveOpen = true);
        var sut8 = await sut3.ToEncodedStringAsync(o => o.LeaveOpen = true);
        var sut9 = await sut4.ToEncodedStringAsync(o => o.LeaveOpen = true);
        var sut10 = await sut5.ToEncodedStringAsync(o => o.LeaveOpen = true);
        var sut11 = await sut6.ToEncodedStringAsync(o => o.LeaveOpen = true);

        Assert.Equal(sut1, sut7);
        Assert.Equal(sut1, sut8);
        Assert.Equal(sut1, sut9);
        Assert.Equal(sut1, sut10);
        Assert.Equal(sut1, sut11);

        Assert.Equal(164, sut2.Length);
        Assert.Equal(318, sut3.Length);
        Assert.Equal(318, sut4.Length);
        Assert.Equal(636, sut5.Length);
        Assert.Equal(636, sut6.Length);
    }

#if NET9_0_OR_GREATER

    [Fact]
    public void CompressBrotli_ShouldCompressAndDecompress()
    {
        var size = 1024 * 1024;
        var sut1 = Generate.RandomString(size);
        var sut2 = sut1.ToStream();
        var sut3 = sut2.CompressBrotli();
        var sut4 = sut3.DecompressBrotli();
        var sut5 = sut2.ToEncodedString(o => o.LeaveOpen = true);
        var sut6 = sut3.ToEncodedString(o => o.LeaveOpen = true);
        var sut7 = sut4.ToEncodedString(o => o.LeaveOpen = true);

        Assert.Equal(size, sut2.Length);
        Assert.NotEqual(sut2.Length, sut3.Length);
        Assert.True(sut2.Length > sut3.Length);
        Assert.Equal(sut2.Length, sut4.Length);
        Assert.Equal(sut5, sut7);
        Assert.NotEqual(sut5, sut6);

        TestOutput.WriteLine($"Original ({sut2.Length}): {sut5.Substring(0, 50)} ...");
        TestOutput.WriteLine($"Compressed ({sut3.Length}): {sut6.Substring(0, 50)} ...");
        TestOutput.WriteLine($"Decompressed ({sut4.Length}): {sut7.Substring(0, 50)} ...");
    }

    [Fact]
    public async Task CompressBrotliAsync_ShouldCompressAndDecompress()
    {
        var size = 1024 * 1024;
        var sut1 = Generate.RandomString(size);
        var sut2 = await sut1.ToStreamAsync();
        var sut3 = await sut2.CompressBrotliAsync();
        var sut4 = await sut3.DecompressBrotliAsync();
        var sut5 = await sut2.ToEncodedStringAsync(o => o.LeaveOpen = true);
        var sut6 = await sut3.ToEncodedStringAsync(o => o.LeaveOpen = true);
        var sut7 = await sut4.ToEncodedStringAsync(o => o.LeaveOpen = true);

        Assert.Equal(size, sut2.Length);
        Assert.NotEqual(sut2.Length, sut3.Length);
        Assert.True(sut2.Length > sut3.Length);
        Assert.Equal(sut2.Length, sut4.Length);
        Assert.Equal(sut5, sut7);
        Assert.NotEqual(sut5, sut6);

        TestOutput.WriteLine($"Original ({sut2.Length}): {sut5.Substring(0, 50)} ...");
        TestOutput.WriteLine($"Compressed ({sut3.Length}): {sut6.Substring(0, 50)} ...");
        TestOutput.WriteLine($"Decompressed ({sut4.Length}): {sut7.Substring(0, 50)} ...");
    }

    [Fact]
    public async Task CompressBrotliAsync_ShouldThrowTaskCanceledException()
    {
        var sut1 = new CancellationTokenSource(TimeSpan.FromMilliseconds(1));
        var size = 1024 * 1024;
        var sut2 = Generate.RandomString(size);
        var sut3 = await sut2.ToStreamAsync();
        await Assert.ThrowsAsync<TaskCanceledException>(async () =>
        {
            await Task.Delay(TimeSpan.FromMilliseconds(100), sut1.Token);
            await sut3.CompressBrotliAsync(o => o.CancellationToken = sut1.Token);
        });
    }

#endif

    [Fact]
    public void CompressGZip_ShouldCompressAndDecompress()
    {
        var size = 1024 * 1024;
        var sut1 = Generate.RandomString(size);
        var sut2 = sut1.ToStream();
        var sut3 = sut2.CompressGZip();
        var sut4 = sut3.DecompressGZip();
        var sut5 = sut2.ToEncodedString(o => o.LeaveOpen = true);
        var sut6 = sut3.ToEncodedString(o => o.LeaveOpen = true);
        var sut7 = sut4.ToEncodedString(o => o.LeaveOpen = true);

        Assert.Equal(size, sut2.Length);
        Assert.NotEqual(sut2.Length, sut3.Length);
        Assert.True(sut2.Length > sut3.Length);
        Assert.Equal(sut2.Length, sut4.Length);
        Assert.Equal(sut5, sut7);
        Assert.NotEqual(sut5, sut6);

        TestOutput.WriteLine($"Original ({sut2.Length}): {sut5.Substring(0, 50)} ...");
        TestOutput.WriteLine($"Compressed ({sut3.Length}): {sut6.Substring(0, 50)} ...");
        TestOutput.WriteLine($"Decompressed ({sut4.Length}): {sut7.Substring(0, 50)} ...");
    }

    [Fact]
    public async Task CompressGZipAsync_ShouldCompressAndDecompress()
    {
        var size = 1024 * 1024;
        var sut1 = Generate.RandomString(size);
        var sut2 = await sut1.ToStreamAsync();
        var sut3 = await sut2.CompressGZipAsync();
        var sut4 = await sut3.DecompressGZipAsync();
        var sut5 = await sut2.ToEncodedStringAsync(o => o.LeaveOpen = true);
        var sut6 = await sut3.ToEncodedStringAsync(o => o.LeaveOpen = true);
        var sut7 = await sut4.ToEncodedStringAsync(o => o.LeaveOpen = true);

        Assert.Equal(size, sut2.Length);
        Assert.NotEqual(sut2.Length, sut3.Length);
        Assert.True(sut2.Length > sut3.Length);
        Assert.Equal(sut2.Length, sut4.Length);
        Assert.Equal(sut5, sut7);
        Assert.NotEqual(sut5, sut6);

        TestOutput.WriteLine($"Original ({sut2.Length}): {sut5.Substring(0, 50)} ...");
        TestOutput.WriteLine($"Compressed ({sut3.Length}): {sut6.Substring(0, 50)} ...");
        TestOutput.WriteLine($"Decompressed ({sut4.Length}): {sut7.Substring(0, 50)} ...");
    }

    [Fact]
    public async Task CompressGZipAsync_ShouldThrowTaskCanceledException()
    {
        var sut1 = new CancellationTokenSource(TimeSpan.FromMilliseconds(1));
        var size = 1024 * 1024;
        var sut2 = Generate.RandomString(size);
        var sut3 = await sut2.ToStreamAsync();
        await Assert.ThrowsAsync<TaskCanceledException>(async () =>
        {
            await Task.Delay(TimeSpan.FromMilliseconds(100), sut1.Token);
            await sut3.CompressGZipAsync(o => o.CancellationToken = sut1.Token);
        });
    }

    [Fact]
    public void CompressDeflate_ShouldCompressAndDecompress()
    {
        var size = 1024 * 1024;
        var sut1 = Generate.RandomString(size);
        var sut2 = sut1.ToStream();
        var sut3 = sut2.CompressDeflate();
        var sut4 = sut3.DecompressDeflate();
        var sut5 = sut2.ToEncodedString(o => o.LeaveOpen = true);
        var sut6 = sut3.ToEncodedString(o => o.LeaveOpen = true);
        var sut7 = sut4.ToEncodedString(o => o.LeaveOpen = true);

        Assert.Equal(size, sut2.Length);
        Assert.NotEqual(sut2.Length, sut3.Length);
        Assert.True(sut2.Length > sut3.Length);
        Assert.Equal(sut2.Length, sut4.Length);
        Assert.Equal(sut5, sut7);
        Assert.NotEqual(sut5, sut6);

        TestOutput.WriteLine($"Original ({sut2.Length}): {sut5.Substring(0, 50)} ...");
        TestOutput.WriteLine($"Compressed ({sut3.Length}): {sut6.Substring(0, 50)} ...");
        TestOutput.WriteLine($"Decompressed ({sut4.Length}): {sut7.Substring(0, 50)} ...");
    }

    [Fact]
    public async Task CompressDeflateAsync_ShouldCompressAndDecompress()
    {
        var size = 1024 * 1024;
        var sut1 = Generate.RandomString(size);
        var sut2 = await sut1.ToStreamAsync();
        var sut3 = await sut2.CompressDeflateAsync();
        var sut4 = await sut3.DecompressDeflateAsync();
        var sut5 = await sut2.ToEncodedStringAsync(o => o.LeaveOpen = true);
        var sut6 = await sut3.ToEncodedStringAsync(o => o.LeaveOpen = true);
        var sut7 = await sut4.ToEncodedStringAsync(o => o.LeaveOpen = true);

        Assert.Equal(size, sut2.Length);
        Assert.NotEqual(sut2.Length, sut3.Length);
        Assert.True(sut2.Length > sut3.Length);
        Assert.Equal(sut2.Length, sut4.Length);
        Assert.Equal(sut5, sut7);
        Assert.NotEqual(sut5, sut6);

        TestOutput.WriteLine($"Original ({sut2.Length}): {sut5.Substring(0, 50)} ...");
        TestOutput.WriteLine($"Compressed ({sut3.Length}): {sut6.Substring(0, 50)} ...");
        TestOutput.WriteLine($"Decompressed ({sut4.Length}): {sut7.Substring(0, 50)} ...");
    }

    [Fact]
    public async Task CompressDeflateAsync_ShouldThrowTaskCanceledException()
    {
        var sut1 = new CancellationTokenSource(TimeSpan.FromMilliseconds(1));
        var size = 1024 * 1024;
        var sut2 = Generate.RandomString(size);
        var sut3 = await sut2.ToStreamAsync();
        await Assert.ThrowsAsync<TaskCanceledException>(async () =>
        {
            await Task.Delay(TimeSpan.FromMilliseconds(100), sut1.Token);
            await sut3.CompressDeflateAsync(o => o.CancellationToken = sut1.Token);
        });
    }

    [Fact]
    public async Task ToStreamAsync_ShouldConvertStringToStream()
    {
        var size = 2048;
        var rs = Generate.RandomString(size);
        var s = await rs.ToStreamAsync();
        using (var sr = new StreamReader(s))
        {
            var result = await sr.ReadToEndAsync();
            Assert.Equal(size, s.Length);
            Assert.Equal(rs, result);
        }
    }

    [Fact]
    public async Task ToStreamAsync_ShouldConvertByteArrayToStream()
    {
        var size = 1024 * 1024;
        var fs = Generate.FixedString('*', size);
        var fsBytes = Convertible.GetBytes(fs);
        var s = await fsBytes.ToStreamAsync();
        using (var sr = new StreamReader(s))
        {
            var result = await sr.ReadToEndAsync();
            Assert.Equal(size, s.Length);
            Assert.All(result, c => Assert.Equal('*', c));
        }
    }
}
