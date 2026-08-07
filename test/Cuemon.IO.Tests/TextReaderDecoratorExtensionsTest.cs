using System;
using System.IO;
using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.IO;
public class TextReaderDecoratorExtensionsTest : Test
{
    public TextReaderDecoratorExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task CopyToAsync_ShouldCopyTextToWriter()
    {
        using (var reader = new StringReader("Alpha Beta Gamma"))
        using (var writer = new StringWriter())
        {
            await Decorator.Enclose<TextReader>(reader).CopyToAsync(writer, 4);

            Assert.Equal("Alpha Beta Gamma", writer.ToString());
        }
    }

    [Fact]
    public async Task CopyToAsync_ShouldThrowArgumentNullException_WhenDecoratorIsNull()
    {
        IDecorator<TextReader> sut = null;

        await Assert.ThrowsAsync<ArgumentNullException>(() => sut.CopyToAsync(new StringWriter()));
    }

    [Fact]
    public async Task CopyToAsync_ShouldThrowArgumentNullException_WhenWriterIsNull()
    {
        using (var reader = new StringReader("Alpha Beta Gamma"))
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => Decorator.Enclose<TextReader>(reader).CopyToAsync(null));
        }
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task CopyToAsync_ShouldThrowArgumentOutOfRangeException_WhenBufferSizeIsInvalid(int bufferSize)
    {
        using (var reader = new StringReader("Alpha Beta Gamma"))
        using (var writer = new StringWriter())
        {
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => Decorator.Enclose<TextReader>(reader).CopyToAsync(writer, bufferSize));
        }
    }
}
