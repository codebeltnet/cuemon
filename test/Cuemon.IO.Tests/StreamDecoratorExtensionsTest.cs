using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Cuemon.Extensions.IO;
using Xunit;

namespace Cuemon.IO
{
    public class StreamDecoratorExtensionsTest : Test
    {
        public StreamDecoratorExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

#if NET10_0_OR_GREATER

        [Fact]
        public void CompressBrotli_ShouldCompressAndDecompress()
        {
            var size = 1024 * 1024;
            var fs = Generate.RandomString(size);
            var os = Decorator.Enclose(fs).ToStream();
            var cos = Decorator.Enclose(os).CompressBrotli();
            var dos = Decorator.Enclose(cos).DecompressBrotli();
            var osResult = Decorator.Enclose(os).ToEncodedString(o => o.LeaveOpen = true);
            var cosResult = Decorator.Enclose(cos).ToEncodedString(o => o.LeaveOpen = true);
            var dosResult = Decorator.Enclose(dos).ToEncodedString(o => o.LeaveOpen = true);

            Assert.Equal(size, os.Length);
            Assert.NotEqual(os.Length, cos.Length);
            Assert.True(os.Length > cos.Length);
            Assert.Equal(os.Length, dos.Length);
            Assert.Equal(osResult, dosResult);
            Assert.NotEqual(osResult, cosResult);

            TestOutput.WriteLine($"Original ({os.Length}): {osResult.Substring(0, 50)} ...");
            TestOutput.WriteLine($"Compressed ({cos.Length}): {cosResult.Substring(0, 50)} ...");
            TestOutput.WriteLine($"Decompressed ({dos.Length}): {dosResult.Substring(0, 50)} ...");
        }

        [Fact]
        public async Task CompressBrotliAsync_ShouldCompressAndDecompress()
        {
            var size = 1024 * 1024;
            var fs = Generate.RandomString(size);
            var os = await fs.ToStreamAsync();
            var cos = await Decorator.Enclose(os).CompressBrotliAsync();
            var dos = await Decorator.Enclose(cos).DecompressBrotliAsync();
            var osResult = await Decorator.Enclose(os).ToEncodedStringAsync(o => o.LeaveOpen = true);
            var cosResult = await Decorator.Enclose(cos).ToEncodedStringAsync(o => o.LeaveOpen = true);
            var dosResult = await Decorator.Enclose(dos).ToEncodedStringAsync(o => o.LeaveOpen = true);

            Assert.Equal(size, os.Length);
            Assert.NotEqual(os.Length, cos.Length);
            Assert.True(os.Length > cos.Length);
            Assert.Equal(os.Length, dos.Length);
            Assert.Equal(osResult, dosResult);
            Assert.NotEqual(osResult, cosResult);

            TestOutput.WriteLine($"Original ({os.Length}): {osResult.Substring(0, 50)} ...");
            TestOutput.WriteLine($"Compressed ({cos.Length}): {cosResult.Substring(0, 50)} ...");
            TestOutput.WriteLine($"Decompressed ({dos.Length}): {dosResult.Substring(0, 50)} ...");
        }

        [Fact]
        public async Task CompressBrotliAsync_ShouldThrowTaskCanceledException()
        {
            var ctsShouldFail = new CancellationTokenSource(TimeSpan.FromMilliseconds(5));
            var size = 1024 * 1024;
            var fs = Generate.RandomString(size);
            var os = await fs.ToStreamAsync();
            await Assert.ThrowsAsync<TaskCanceledException>(async () =>
            {
                await Task.Delay(TimeSpan.FromMilliseconds(100), ctsShouldFail.Token);
                await Decorator.Enclose(os).CompressBrotliAsync(o => o.CancellationToken = ctsShouldFail.Token);
            });
        }

#endif

        [Fact]
        public void CompressGZip_ShouldCompressAndDecompress()
        {
            var size = 1024 * 1024;
            var fs = Generate.RandomString(size);
            var os = Decorator.Enclose(fs).ToStream();
            var cos = Decorator.Enclose(os).CompressGZip();
            var dos = Decorator.Enclose(cos).DecompressGZip();
            var osResult = Decorator.Enclose(os).ToEncodedString(o => o.LeaveOpen = true);
            var cosResult = Decorator.Enclose(cos).ToEncodedString(o => o.LeaveOpen = true);
            var dosResult = Decorator.Enclose(dos).ToEncodedString(o => o.LeaveOpen = true);

            Assert.Equal(size, os.Length);
            Assert.NotEqual(os.Length, cos.Length);
            Assert.True(os.Length > cos.Length);
            Assert.Equal(os.Length, dos.Length);
            Assert.Equal(osResult, dosResult);
            Assert.NotEqual(osResult, cosResult);

            TestOutput.WriteLine($"Original ({os.Length}): {osResult.Substring(0, 50)} ...");
            TestOutput.WriteLine($"Compressed ({cos.Length}): {cosResult.Substring(0, 50)} ...");
            TestOutput.WriteLine($"Decompressed ({dos.Length}): {dosResult.Substring(0, 50)} ...");
        }

        [Fact]
        public async Task CompressGZipAsync_ShouldCompressAndDecompress()
        {
            var size = 1024 * 1024;
            var fs = Generate.RandomString(size);
            var os = await fs.ToStreamAsync();
            var cos = await Decorator.Enclose(os).CompressGZipAsync();
            var dos = await Decorator.Enclose(cos).DecompressGZipAsync();
            var osResult = await Decorator.Enclose(os).ToEncodedStringAsync(o => o.LeaveOpen = true);
            var cosResult = await Decorator.Enclose(cos).ToEncodedStringAsync(o => o.LeaveOpen = true);
            var dosResult = await Decorator.Enclose(dos).ToEncodedStringAsync(o => o.LeaveOpen = true);

            Assert.Equal(size, os.Length);
            Assert.NotEqual(os.Length, cos.Length);
            Assert.True(os.Length > cos.Length);
            Assert.Equal(os.Length, dos.Length);
            Assert.Equal(osResult, dosResult);
            Assert.NotEqual(osResult, cosResult);

            TestOutput.WriteLine($"Original ({os.Length}): {osResult.Substring(0, 50)} ...");
            TestOutput.WriteLine($"Compressed ({cos.Length}): {cosResult.Substring(0, 50)} ...");
            TestOutput.WriteLine($"Decompressed ({dos.Length}): {dosResult.Substring(0, 50)} ...");
        }

        [Fact]
        public async Task CompressGZipAsync_ShouldThrowTaskCanceledException()
        {
            var ctsShouldFail = new CancellationTokenSource(TimeSpan.FromMilliseconds(5));
            var size = 1024 * 1024;
            var fs = Generate.RandomString(size);
            var os = await fs.ToStreamAsync();
            await Assert.ThrowsAsync<TaskCanceledException>(async () =>
            {
                await Task.Delay(TimeSpan.FromMilliseconds(100), ctsShouldFail.Token);
                await Decorator.Enclose(os).CompressGZipAsync(o => o.CancellationToken = ctsShouldFail.Token);
            });
        }

        [Fact]
        public void CompressDeflate_ShouldCompressAndDecompress()
        {
            var size = 1024 * 1024;
            var fs = Generate.RandomString(size);
            var os = Decorator.Enclose(fs).ToStream();
            var cos = Decorator.Enclose(os).CompressDeflate();
            var dos = Decorator.Enclose(cos).DecompressDeflate();
            var osResult = Decorator.Enclose(os).ToEncodedString(o => o.LeaveOpen = true);
            var cosResult = Decorator.Enclose(cos).ToEncodedString(o => o.LeaveOpen = true);
            var dosResult = Decorator.Enclose(dos).ToEncodedString(o => o.LeaveOpen = true);

            Assert.Equal(size, os.Length);
            Assert.NotEqual(os.Length, cos.Length);
            Assert.True(os.Length > cos.Length);
            Assert.Equal(os.Length, dos.Length);
            Assert.Equal(osResult, dosResult);
            Assert.NotEqual(osResult, cosResult);

            TestOutput.WriteLine($"Original ({os.Length}): {osResult.Substring(0, 50)} ...");
            TestOutput.WriteLine($"Compressed ({cos.Length}): {cosResult.Substring(0, 50)} ...");
            TestOutput.WriteLine($"Decompressed ({dos.Length}): {dosResult.Substring(0, 50)} ...");
        }

        [Fact]
        public async Task CompressDeflateAsync_ShouldCompressAndDecompress()
        {
            var size = 1024 * 1024;
            var fs = Generate.RandomString(size);
            var os = await fs.ToStreamAsync();
            var cos = await Decorator.Enclose(os).CompressDeflateAsync();
            var dos = await Decorator.Enclose(cos).DecompressDeflateAsync();
            var osResult = await Decorator.Enclose(os).ToEncodedStringAsync(o => o.LeaveOpen = true);
            var cosResult = await Decorator.Enclose(cos).ToEncodedStringAsync(o => o.LeaveOpen = true);
            var dosResult = await Decorator.Enclose(dos).ToEncodedStringAsync(o => o.LeaveOpen = true);

            Assert.Equal(size, os.Length);
            Assert.NotEqual(os.Length, cos.Length);
            Assert.True(os.Length > cos.Length);
            Assert.Equal(os.Length, dos.Length);
            Assert.Equal(osResult, dosResult);
            Assert.NotEqual(osResult, cosResult);

            TestOutput.WriteLine($"Original ({os.Length}): {osResult.Substring(0, 50)} ...");
            TestOutput.WriteLine($"Compressed ({cos.Length}): {cosResult.Substring(0, 50)} ...");
            TestOutput.WriteLine($"Decompressed ({dos.Length}): {dosResult.Substring(0, 50)} ...");
        }

        [Fact]
        public async Task CompressDeflateAsync_ShouldThrowTaskCanceledException()
        {
            var ctsShouldFail = new CancellationTokenSource(TimeSpan.FromMilliseconds(5));
            var size = 1024 * 1024;
            var fs = Generate.RandomString(size);
            var os = await fs.ToStreamAsync();
            await Assert.ThrowsAsync<TaskCanceledException>(async () =>
            {
                await Task.Delay(TimeSpan.FromMilliseconds(100), ctsShouldFail.Token);
                await Decorator.Enclose(os).CompressDeflateAsync(o => o.CancellationToken = ctsShouldFail.Token);
            });
        }

        [Fact]
        public void ToByteArray_ShouldConvertStreamToByteArrayWithDefaultOptions()
        {
            var size = 1024 * 1024;
            var fs = Generate.FixedString('*', size);
            var fsBytes = Convertible.GetBytes(fs);
            var s = new MemoryStream(fsBytes);
            var sb = Decorator.Enclose(s).ToByteArray();

            Assert.Throws<ObjectDisposedException>(() => s.Capacity);
            Assert.Equal(fsBytes, sb);
            Assert.Equal(size, sb.Length);
            Assert.True(sb.All(b => b == '*'), "Expected all elements to have the value of '*'.");
        }

        [Fact]
        public async Task ToByteArrayAsync_ShouldConvertStreamToByteArrayWithDefaultOptions()
        {
            var size = 1024 * 1024;
            var fs = Generate.FixedString('*', size);
            var fsBytes = Convertible.GetBytes(fs);
            var s = new MemoryStream(fsBytes);
            var sb = await Decorator.Enclose(s).ToByteArrayAsync();

            Assert.Throws<ObjectDisposedException>(() => s.Capacity);
            Assert.Equal(fsBytes, sb);
            Assert.Equal(size, sb.Length);
            Assert.True(sb.All(b => b == '*'), "Expected all elements to have the value of '*'.");
        }

        [Fact]
        public void ToEncodedString_ShouldConvertStreamToString()
        {
            var size = 1024;
            var enc = Encoding.GetEncoding("iso-8859-1");
            var fs = Generate.RandomString(size, "æøåÆØÅ");
            var fsBytesUnicode = Convertible.GetBytes(fs);
            var fsBytesIso88591 = Convertible.GetBytes(fs, o => o.Encoding = enc);
            var sUnicode = Decorator.Enclose(fsBytesUnicode).ToStream();
            var sUnicodeLength = sUnicode.Length;
            var sIso88591 = Decorator.Enclose(fsBytesIso88591).ToStream();
            var sIso88591Length = sIso88591.Length;
            var resultUnicode = Decorator.Enclose(sUnicode).ToEncodedString(o => o.LeaveOpen = true);
            var resultIso88591 = Decorator.Enclose(sIso88591).ToEncodedString(o =>
            {
                o.Encoding = enc;
                o.LeaveOpen = true;
            });
            var wrongDecodedUnicodeResult = Decorator.Enclose(sUnicode).ToEncodedString(o => o.Encoding = Encoding.ASCII);
            var wrongDecodedIso88591Result = Decorator.Enclose(sIso88591).ToEncodedString(o => o.Encoding = Encoding.ASCII);

            Assert.Throws<ObjectDisposedException>(() => sIso88591.Length);
            Assert.Throws<ObjectDisposedException>(() => sUnicode.Length);
            Assert.Equal(fsBytesIso88591.Length, sIso88591Length);
            Assert.Equal(fsBytesUnicode.Length, sUnicodeLength);
            Assert.NotEqual(fsBytesIso88591.Length, fsBytesUnicode.Length);
            Assert.NotEqual(sIso88591Length, sUnicodeLength);
            Assert.Equal(fs, resultIso88591);
            Assert.Equal(fs, resultUnicode);
            Assert.NotEqual(fs, wrongDecodedIso88591Result);
            Assert.NotEqual(fs, wrongDecodedUnicodeResult);
        }

        [Fact]
        public async Task ToEncodedStringAsync_ShouldConvertStreamToString()
        {
            var size = 1024;
            var enc = Encoding.GetEncoding("iso-8859-1");
            var fs = Generate.RandomString(size, "æøåÆØÅ");
            var fsBytesUnicode = Convertible.GetBytes(fs);
            var fsBytesIso88591 = Convertible.GetBytes(fs, o => o.Encoding = enc);
            var sUnicode = await fsBytesUnicode.ToStreamAsync();
            var sUnicodeLength = sUnicode.Length;
            var sIso88591 = await fsBytesIso88591.ToStreamAsync();
            var sIso88591Length = sIso88591.Length;
            var resultUnicode = await Decorator.Enclose(sUnicode).ToEncodedStringAsync(o => o.LeaveOpen = true);
            var resultIso88591 = await Decorator.Enclose(sIso88591).ToEncodedStringAsync(o =>
            {
                o.Encoding = enc;
                o.LeaveOpen = true;
            });
            var wrongDecodedUnicodeResult = await Decorator.Enclose(sUnicode).ToEncodedStringAsync(o => o.Encoding = Encoding.ASCII);
            var wrongDecodedIso88591Result = await Decorator.Enclose(sIso88591).ToEncodedStringAsync(o => o.Encoding = Encoding.ASCII);

            Assert.Throws<ObjectDisposedException>(() => sIso88591.Length);
            Assert.Throws<ObjectDisposedException>(() => sUnicode.Length);
            Assert.Equal(fsBytesIso88591.Length, sIso88591Length);
            Assert.Equal(fsBytesUnicode.Length, sUnicodeLength);
            Assert.NotEqual(fsBytesIso88591.Length, fsBytesUnicode.Length);
            Assert.NotEqual(sIso88591Length, sUnicodeLength);
            Assert.Equal(fs, resultIso88591);
            Assert.Equal(fs, resultUnicode);
            Assert.NotEqual(fs, wrongDecodedIso88591Result);
            Assert.NotEqual(fs, wrongDecodedUnicodeResult);
        }
    }
}