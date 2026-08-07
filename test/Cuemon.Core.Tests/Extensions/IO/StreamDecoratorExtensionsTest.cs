using System;
using System.IO;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.IO;
/// <summary>
/// Tests for the <see cref="StreamDecoratorExtensions"/> class.
/// </summary>
public class StreamDecoratorExtensionsTest : Test
{
    public StreamDecoratorExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void CopyStream_ShouldThrowArgumentNullException_WhenDecoratorIsNull()
    {
        IDecorator<Stream> decorator = null;
        Assert.Throws<ArgumentNullException>(() => decorator.CopyStream(Stream.Null));
    }

    [Fact]
    public void CopyStream_ShouldCopyContentToDestination()
    {
        var data = "Hello, World!"u8.ToArray();
        using var source = new MemoryStream(data);
        using var destination = new MemoryStream();

        Decorator.Enclose(source).CopyStream(destination);

        Assert.Equal(data.Length, destination.Length);
        Assert.Equal(data, destination.ToArray());
    }

    [Fact]
    public void CopyStream_ShouldResetSourcePositionAfterCopy_WhenChangePositionIsTrue()
    {
        var data = "Some test data"u8.ToArray();
        using var source = new MemoryStream(data);
        source.Position = 5;

        using var destination = new MemoryStream();

        Decorator.Enclose(source).CopyStream(destination, changePosition: true);

        Assert.Equal(5, source.Position);
        Assert.Equal(0, destination.Position);
        Assert.Equal(data.Length, destination.Length);
    }

    [Fact]
    public void CopyStream_ShouldNotResetSourcePosition_WhenChangePositionIsFalse()
    {
        var data = "Some test data"u8.ToArray();
        using var source = new MemoryStream(data);
        source.Position = 5;

        using var destination = new MemoryStream();

        Decorator.Enclose(source).CopyStream(destination, changePosition: false);

        Assert.Equal(data.Length, source.Position);
        Assert.Equal(data.Length - 5, destination.Length);
    }

    [Fact]
    public void CopyStream_ShouldCopyEntireStream_WhenSourcePositionIsAtEnd()
    {
        var data = "End of stream"u8.ToArray();
        using var source = new MemoryStream(data);
        source.Position = source.Length;

        using var destination = new MemoryStream();

        Decorator.Enclose(source).CopyStream(destination, changePosition: true);

        Assert.Equal(data.Length, destination.Length);
        Assert.Equal(data, destination.ToArray());
    }

    [Fact]
    public void CopyStream_ShouldSetDestinationPositionToZero_WhenChangePositionIsTrue()
    {
        var data = "Position test"u8.ToArray();
        using var source = new MemoryStream(data);
        using var destination = new MemoryStream();

        Decorator.Enclose(source).CopyStream(destination, changePosition: true);

        Assert.Equal(0, destination.Position);
    }

    [Fact]
    public void CopyStream_ShouldRespectCustomBufferSize()
    {
        var data = "Buffer size test data with more content to exceed small buffer"u8.ToArray();
        using var source = new MemoryStream(data);
        using var destination = new MemoryStream();

        Decorator.Enclose(source).CopyStream(destination, bufferSize: 4);

        Assert.Equal(data.Length, destination.Length);
        Assert.Equal(data, destination.ToArray());
    }

    [Fact]
    public void InvokeToByteArray_ShouldThrowArgumentNullException_WhenDecoratorIsNull()
    {
        IDecorator<Stream> decorator = null;
        Assert.Throws<ArgumentNullException>(() => decorator.InvokeToByteArray());
    }

    [Fact]
    public void InvokeToByteArray_ShouldConvertMemoryStreamToByteArray()
    {
        var data = "MemoryStream test"u8.ToArray();
        using var stream = new MemoryStream(data);

        var result = Decorator.Enclose(stream).InvokeToByteArray(leaveOpen: true);

        Assert.Equal(data, result);
    }

    [Fact]
    public void InvokeToByteArray_ShouldDisposeStream_WhenLeaveOpenIsFalse()
    {
        var data = "Dispose test"u8.ToArray();
        var stream = new MemoryStream(data);

        Decorator.Enclose(stream).InvokeToByteArray(leaveOpen: false);

        Assert.Throws<ObjectDisposedException>(() => stream.ReadByte());
    }

    [Fact]
    public void InvokeToByteArray_ShouldNotDisposeStream_WhenLeaveOpenIsTrue()
    {
        var data = "Leave open test"u8.ToArray();
        using var stream = new MemoryStream(data);

        Decorator.Enclose(stream).InvokeToByteArray(leaveOpen: true);

        stream.Position = 0;
        Assert.Equal(data[0], stream.ReadByte());
    }

    [Fact]
    public void InvokeToByteArray_ShouldConvertNonMemoryStreamToByteArray()
    {
        var data = "Non-MemoryStream data"u8.ToArray();
        using var fileStream = new BufferedStream(new MemoryStream(data));

        var result = Decorator.Enclose(fileStream).InvokeToByteArray(leaveOpen: true);

        Assert.Equal(data, result);
    }

    [Fact]
    public void InvokeToByteArray_ShouldPreservePosition_ForNonMemoryStream()
    {
        var data = "Position preservation test"u8.ToArray();
        using var inner = new MemoryStream(data);
        using var stream = new BufferedStream(inner);
        stream.Position = 5;

        var result = Decorator.Enclose(stream).InvokeToByteArray(leaveOpen: true);

        Assert.Equal(5, stream.Position);
        Assert.Equal(data, result);
    }

    [Fact]
    public void InvokeToByteArray_ShouldHandleEmptyStream()
    {
        using var stream = new MemoryStream();

        var result = Decorator.Enclose(stream).InvokeToByteArray(leaveOpen: true);

        Assert.Empty(result);
    }

    [Fact]
    public void InvokeToByteArray_ShouldRespectCustomBufferSize()
    {
        var data = "Buffer size test with enough data"u8.ToArray();
        using var stream = new BufferedStream(new MemoryStream(data));

        var result = Decorator.Enclose(stream).InvokeToByteArray(bufferSize: 8, leaveOpen: true);

        Assert.Equal(data, result);
    }
}
