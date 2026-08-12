using System;
using System.IO;

namespace Cuemon.IO;
/// <summary>
/// Extension methods for the <see cref="Stream"/> class hidden behind the <see cref="IDecorator{T}"/> interface.
/// This API supports the product infrastructure and is not intended to be used directly from application code.
/// </summary>
/// <seealso cref="IDecorator{T}"/>
/// <seealso cref="Decorator{T}"/>
public static class StreamDecoratorExtensions
{
    /// <summary>
    /// Copies the contents of the enclosed <see cref="Stream"/> to the specified <paramref name="destination"/>.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{Stream}"/> that wraps the source stream.</param>
    /// <param name="destination">The destination stream to which the contents of the source stream are copied.</param>
    /// <param name="bufferSize">The size of the buffer, in bytes. The value must be greater than zero. The default is 81920.</param>
    /// <param name="changePosition">
    /// <see langword="true"/> to temporarily reset the position of the enclosed stream to the beginning before copying;
    /// otherwise, <see langword="false"/> to preserve the current position.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> is <see langword="null"/>.
    /// </exception>
    public static void CopyStream(this IDecorator<Stream> decorator, Stream destination, int bufferSize = 81920, bool changePosition = true)
    {
        Validator.ThrowIfNull(decorator);
        var source = decorator.Inner;
        long lastPosition = 0;
        var canSeekSource = source.CanSeek;
        if (changePosition && canSeekSource)
        {
            lastPosition = source.Position;
            source.Position = 0;
        }

        source.CopyTo(destination, bufferSize);
        destination.Flush();

        if (changePosition && canSeekSource) { source.Position = lastPosition; }
        if (changePosition && destination.CanSeek) { destination.Position = 0; }
    }

    /// <summary>
    /// Converts the enclosed <see cref="Stream"/> to its byte array representation.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{Stream}"/> that wraps the source stream.</param>
    /// <param name="bufferSize">The size of the buffer, in bytes. The value must be greater than zero. The default is 81920.</param>
    /// <param name="leaveOpen"><see langword="true"/> to leave the enclosed stream open; otherwise, <see langword="false"/>.</param>
    /// <returns>A byte array containing the contents of the enclosed <see cref="Stream"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// The enclosed <see cref="Stream"/> does not support reading.
    /// </exception>
    /// <remarks>
    /// This API supports the product infrastructure and is not intended to be used directly from application code.
    /// </remarks>
    public static byte[] InvokeToByteArray(this IDecorator<Stream> decorator, int bufferSize = 81920, bool leaveOpen = false)
    {
        Validator.ThrowIfNull(decorator);
        Validator.ThrowIfFalse(decorator.Inner.CanRead, nameof(decorator.Inner), "Stream cannot be read from.");
        try
        {
            if (decorator.Inner is MemoryStream s)
            {
                return s.ToArray();
            }

            var source = decorator.Inner;
            var canSeek = source.CanSeek;
            var oldPosition = 0L;

            if (canSeek)
            {
                oldPosition = source.Position;
                source.Position = 0;
            }

            var length = canSeek ? source.Length : 0L;

            if (canSeek && length == 0)
            {
                source.Position = oldPosition;
                return Array.Empty<byte>();
            }

            var memoryStream = length > 0 && length <= int.MaxValue
                ? new MemoryStream((int)length)
                : new MemoryStream();

            using (memoryStream)
            {
                source.CopyTo(memoryStream, bufferSize);
                if (canSeek)
                {
                    source.Position = oldPosition;
                }

                if (memoryStream.TryGetBuffer(out var segment) &&
                    segment.Offset == 0 &&
                    segment.Count == segment.Array.Length)
                {
                    return segment.Array;
                }

                return memoryStream.ToArray();
            }
        }
        finally
        {
            if (!leaveOpen)
            {
                decorator.Inner.Dispose();
            }
        }
    }
}
