using System;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using Cuemon.Text;
using Cuemon.Threading;

namespace Cuemon.IO;
/// <summary>
/// Extension methods for the <see cref="Stream"/> class hidden behind the <see cref="IDecorator{T}"/> interface.
/// </summary>
/// <seealso cref="IDecorator{T}"/>
/// <seealso cref="Decorator{T}"/>
public static class StreamDecoratorExtensions
{
    /// <summary>
    /// Asynchronously reads the bytes from the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/> and writes them to the <paramref name="destination"/>.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{Stream}"/> to extend.</param>
    /// <param name="destination">The <see cref="Stream"/> to which the contents of the current stream will be copied.</param>
    /// <param name="bufferSize">The size of the buffer. This value must be greater than zero. The default size is 81920.</param>
    /// <param name="changePosition">if <c>true</c>, the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/> will temporarily have its position changed to 0; otherwise the position is left untouched.</param>
    /// <param name="ct">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static async Task CopyStreamAsync(this IDecorator<Stream> decorator, Stream destination, int bufferSize = 81920, bool changePosition = true, CancellationToken ct = default)
    {
        Validator.ThrowIfNull(decorator);
        var source = decorator.Inner;
        long lastPosition = 0;
        if (changePosition && source.CanSeek)
        {
            lastPosition = source.Position;
            if (source.CanSeek) { source.Position = 0; }
        }

        await source.CopyToAsync(destination, bufferSize, ct).ConfigureAwait(false);
        await destination.FlushAsync(ct).ConfigureAwait(false);

        if (changePosition && source.CanSeek) { source.Position = lastPosition; }
        if (changePosition && destination.CanSeek) { destination.Position = 0; }
    }

    /// <summary>
    /// Converts the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/> to its equivalent <see cref="byte"/> array representation.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{Stream}"/> to extend.</param>
    /// <param name="setup">The <see cref="StreamCopyOptions"/> which may be configured.</param>
    /// <returns>A <see cref="byte"/> array that is equivalent to the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// The enclosed <see cref="Stream"/> of <paramref name="decorator"/> cannot be read from.
    /// </exception>
    public static byte[] ToByteArray(this IDecorator<Stream> decorator, Action<StreamCopyOptions> setup = null)
    {
        Validator.ThrowIfNull(decorator);
        Validator.ThrowIfFalse(decorator.Inner.CanRead, nameof(decorator.Inner), "Stream cannot be read from.");
        var options = Patterns.Configure(setup);
        return decorator.InvokeToByteArray(options.BufferSize, options.LeaveOpen);
    }

    /// <summary>
    /// Converts the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/> to its equivalent <see cref="byte"/> array representation.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{Stream}"/> to extend.</param>
    /// <param name="setup">The <see cref="AsyncStreamCopyOptions"/> which may be configured.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="byte"/> array that is equivalent to the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// The enclosed <see cref="Stream"/> of <paramref name="decorator"/> cannot be read from.
    /// </exception>
    public static Task<byte[]> ToByteArrayAsync(this IDecorator<Stream> decorator, Action<AsyncStreamCopyOptions> setup = null)
    {
        Validator.ThrowIfNull(decorator);
        Validator.ThrowIfFalse(decorator.Inner.CanRead, nameof(decorator.Inner), "Stream cannot be read from.");
        return ToByteArrayAsyncCore(decorator, Patterns.Configure(setup));
    }

    private static async Task<byte[]> ToByteArrayAsyncCore(this IDecorator<Stream> decorator, AsyncStreamCopyOptions options)
    {
        try
        {
            if (decorator.Inner is MemoryStream s)
            {
                return s.ToArray();
            }

            using (var memoryStream = new MemoryStream(new byte[decorator.Inner.Length]))
            {
                var oldPosition = decorator.Inner.Position;
                if (decorator.Inner.CanSeek)
                {
                    decorator.Inner.Position = 0;
                }

                await decorator.Inner.CopyToAsync(memoryStream, options.BufferSize, options.CancellationToken).ConfigureAwait(false);
                if (decorator.Inner.CanSeek)
                {
                    decorator.Inner.Position = oldPosition;
                }

                return memoryStream.ToArray();
            }
        }
        finally
        {
            if (!options.LeaveOpen)
            {
                decorator.Inner.Dispose();
            }
        }
    }

    /// <summary>
    /// Converts the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/> to a <see cref="string"/>.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{Stream}"/> to extend.</param>
    /// <param name="setup">The <see cref="StreamReaderOptions"/> which may be configured.</param>
    /// <returns>A <see cref="string"/> containing the result of the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/>.</returns>
    /// <remarks><see cref="IEncodingOptions"/> will be initialized with <see cref="EncodingOptions.DefaultPreambleSequence"/> and <see cref="EncodingOptions.DefaultEncoding"/>.</remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    /// <exception cref="InvalidEnumArgumentException">
    /// <paramref name="setup"/> was initialized with an invalid <see cref="StreamEncodingOptions.Preamble"/>.
    /// </exception>
    public static string ToEncodedString(this IDecorator<Stream> decorator, Action<StreamReaderOptions> setup = null)
    {
        Validator.ThrowIfNull(decorator);
        var options = Patterns.Configure(setup);
        if (options.Encoding.Equals(EncodingOptions.DefaultEncoding))
        {
            options.Encoding = ByteOrderMark.DetectEncodingOrDefault(decorator.Inner, options.Encoding);
        }

        if (options.Preamble < PreambleSequence.Keep || options.Preamble > PreambleSequence.Remove)
        {
            throw new InvalidEnumArgumentException(nameof(setup), (int)options.Preamble, typeof(PreambleSequence));
        }

        var bytes = Decorator.Enclose(decorator.Inner).ToByteArray(o =>
        {
            o.BufferSize = options.BufferSize;
            o.LeaveOpen = options.LeaveOpen;
        });
        return Convertible.ToString(bytes, o =>
        {
            o.Encoding = options.Encoding;
            o.Preamble = options.Preamble;
        });
    }

    /// <summary>
    /// Converts the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/> to a <see cref="string"/>.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{Stream}"/> to extend.</param>
    /// <param name="setup">The <see cref="AsyncStreamReaderOptions"/> which may be configured.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="string"/> containing the result of the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/>.</returns>
    /// <remarks><see cref="IEncodingOptions"/> will be initialized with <see cref="EncodingOptions.DefaultPreambleSequence"/> and <see cref="EncodingOptions.DefaultEncoding"/>.</remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    /// <exception cref="InvalidEnumArgumentException">
    /// <paramref name="setup"/> was initialized with an invalid <see cref="StreamEncodingOptions.Preamble"/>.
    /// </exception>
    public static Task<string> ToEncodedStringAsync(this IDecorator<Stream> decorator, Action<AsyncStreamReaderOptions> setup = null)
    {
        Validator.ThrowIfNull(decorator);
        var options = Patterns.Configure(setup);
        if (options.Encoding.Equals(EncodingOptions.DefaultEncoding))
        {
            options.Encoding = ByteOrderMark.DetectEncodingOrDefault(decorator.Inner, options.Encoding);
        }

        if (options.Preamble < PreambleSequence.Keep || options.Preamble > PreambleSequence.Remove)
        {
            throw new InvalidEnumArgumentException(nameof(setup), (int)options.Preamble, typeof(PreambleSequence));
        }

        return ToEncodedStringAsyncCore(decorator, options);
    }

    private static async Task<string> ToEncodedStringAsyncCore(this IDecorator<Stream> decorator, AsyncStreamReaderOptions options)
    {
        var bytes = await Decorator.Enclose(decorator.Inner).ToByteArrayAsync(o =>
        {
            o.BufferSize = options.BufferSize;
            o.LeaveOpen = options.LeaveOpen;
            o.CancellationToken = options.CancellationToken;
            o.CancellationTokenProvider = options.CancellationTokenProvider;
        }).ConfigureAwait(false);
        return Convertible.ToString(bytes, o =>
        {
            o.Encoding = options.Encoding;
            o.Preamble = options.Preamble;
        });
    }

#if NETSTANDARD2_1_OR_GREATER || NET10_0_OR_GREATER

    /// <summary>
    /// Compress the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/> using the <c>Brotli</c> algorithm.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{Stream}"/> to extend.</param>
    /// <param name="setup">The <see cref="StreamCompressionOptions"/> which may be configured.</param>
    /// <returns>A compressed version of the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// The enclosed <see cref="Stream"/> of <paramref name="decorator"/> does not support write operations such as compression.
    /// </exception>
    public static Stream CompressBrotli(this IDecorator<Stream> decorator, Action<StreamCompressionOptions> setup = null)
    {
        Validator.ThrowIfNull(decorator);
        return Compress(decorator, Patterns.Configure(setup), (stream, level, leaveOpen) => new BrotliStream(stream, level, leaveOpen));
    }

    /// <summary>
    /// Compress the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/> using the <c>Brotli</c> algorithm.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{Stream}"/> to extend.</param>
    /// <param name="setup">The <see cref="AsyncStreamCompressionOptions"/> which may be configured.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a compressed version of the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// The enclosed <see cref="Stream"/> of <paramref name="decorator"/> does not support write operations such as compression.
    /// </exception>
    public static Task<Stream> CompressBrotliAsync(this IDecorator<Stream> decorator, Action<AsyncStreamCompressionOptions> setup = null)
    {
        Validator.ThrowIfNull(decorator);
        return CompressAsync(decorator, Patterns.Configure(setup), (stream, level, leaveOpen) => new BrotliStream(stream, level, leaveOpen));
    }

    /// <summary>
    /// Decompress the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/> using <c>Brotli</c> data format specification.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{Stream}"/> to extend.</param>
    /// <param name="setup">The <see cref="StreamCopyOptions"/> which may be configured.</param>
    /// <returns>A decompressed version of the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// The enclosed <see cref="Stream"/> of <paramref name="decorator"/> does not support write operations such as compression.
    /// </exception>
    /// <exception cref="InvalidDataException">
    /// The enclosed <see cref="Stream"/> of <paramref name="decorator"/> was compressed using an unsupported compression method.
    /// </exception>
    public static Stream DecompressBrotli(this IDecorator<Stream> decorator, Action<StreamCopyOptions> setup = null)
    {
        Validator.ThrowIfNull(decorator);
        return Decompress(decorator, Patterns.Configure(setup), (stream, mode, leaveOpen) => new BrotliStream(stream, mode, leaveOpen));
    }

    /// <summary>
    /// Decompress the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/> using <c>Brotli</c> data format specification.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{Stream}"/> to extend.</param>
    /// <param name="setup">The <see cref="AsyncStreamCopyOptions"/> which may be configured.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a decompressed version of the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// The enclosed <see cref="Stream"/> of <paramref name="decorator"/> does not support write operations such as compression.
    /// </exception>
    /// <exception cref="InvalidDataException">
    /// The enclosed <see cref="Stream"/> of <paramref name="decorator"/> was compressed using an unsupported compression method.
    /// </exception>
    public static Task<Stream> DecompressBrotliAsync(this IDecorator<Stream> decorator, Action<AsyncStreamCopyOptions> setup = null)
    {
        Validator.ThrowIfNull(decorator);
        return DecompressAsync(decorator, Patterns.Configure(setup), (stream, mode, leaveOpen) => new BrotliStream(stream, mode, leaveOpen));
    }

#endif

    /// <summary>
    /// Compress the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/> using the <c>GZip</c> algorithm.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{Stream}"/> to extend.</param>
    /// <param name="setup">The <see cref="StreamCompressionOptions"/> which may be configured.</param>
    /// <returns>A compressed version of the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// The enclosed <see cref="Stream"/> of <paramref name="decorator"/> does not support write operations such as compression.
    /// </exception>
    public static Stream CompressGZip(this IDecorator<Stream> decorator, Action<StreamCompressionOptions> setup = null)
    {
        Validator.ThrowIfNull(decorator);
        return Compress(decorator, Patterns.Configure(setup), (stream, level, leaveOpen) => new GZipStream(stream, level, leaveOpen));
    }

    /// <summary>
    /// Compress the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/> using the <c>GZip</c> algorithm.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{Stream}"/> to extend.</param>
    /// <param name="setup">The <see cref="AsyncStreamCompressionOptions"/> which may be configured.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a compressed version of the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// The enclosed <see cref="Stream"/> of <paramref name="decorator"/> does not support write operations such as compression.
    /// </exception>
    public static Task<Stream> CompressGZipAsync(this IDecorator<Stream> decorator, Action<AsyncStreamCompressionOptions> setup = null)
    {
        Validator.ThrowIfNull(decorator);
        return CompressAsync(decorator, Patterns.Configure(setup), (stream, level, leaveOpen) => new GZipStream(stream, level, leaveOpen));
    }

    /// <summary>
    /// Decompress the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/> using <c>GZip</c> data format specification.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{Stream}"/> to extend.</param>
    /// <param name="setup">The <see cref="StreamCopyOptions"/> which may be configured.</param>
    /// <returns>A decompressed version of the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// The enclosed <see cref="Stream"/> of <paramref name="decorator"/> does not support write operations such as compression.
    /// </exception>
    /// <exception cref="InvalidDataException">
    /// The enclosed <see cref="Stream"/> of <paramref name="decorator"/> was compressed using an unsupported compression method.
    /// </exception>
    public static Stream DecompressGZip(this IDecorator<Stream> decorator, Action<StreamCopyOptions> setup = null)
    {
        Validator.ThrowIfNull(decorator);
        return Decompress(decorator, Patterns.Configure(setup), (stream, mode, leaveOpen) => new GZipStream(stream, mode, leaveOpen));
    }

    /// <summary>
    /// Decompress the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/> using <c>GZip</c> data format specification.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{Stream}"/> to extend.</param>
    /// <param name="setup">The <see cref="AsyncStreamCopyOptions"/> which may be configured.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a decompressed version of the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// The enclosed <see cref="Stream"/> of <paramref name="decorator"/> does not support write operations such as compression.
    /// </exception>
    /// <exception cref="InvalidDataException">
    /// The enclosed <see cref="Stream"/> of <paramref name="decorator"/> was compressed using an unsupported compression method.
    /// </exception>
    public static Task<Stream> DecompressGZipAsync(this IDecorator<Stream> decorator, Action<AsyncStreamCopyOptions> setup = null)
    {
        Validator.ThrowIfNull(decorator);
        return DecompressAsync(decorator, Patterns.Configure(setup), (stream, mode, leaveOpen) => new GZipStream(stream, mode, leaveOpen));
    }

    /// <summary>
    /// Compress the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/> using the <c>Deflate</c> algorithm.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{Stream}"/> to extend.</param>
    /// <param name="setup">The <see cref="StreamCompressionOptions"/> which may be configured.</param>
    /// <returns>A compressed version of the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// The enclosed <see cref="Stream"/> of <paramref name="decorator"/> does not support write operations such as compression.
    /// </exception>
    public static Stream CompressDeflate(this IDecorator<Stream> decorator, Action<StreamCompressionOptions> setup = null)
    {
        Validator.ThrowIfNull(decorator);
        return Compress(decorator, Patterns.Configure(setup), (stream, level, leaveOpen) => new DeflateStream(stream, level, leaveOpen));
    }

    /// <summary>
    /// Compress the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/> using the <c>Deflate</c> algorithm.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{Stream}"/> to extend.</param>
    /// <param name="setup">The <see cref="AsyncStreamCompressionOptions"/> which may be configured.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a compressed version of the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// The enclosed <see cref="Stream"/> of <paramref name="decorator"/> does not support write operations such as compression.
    /// </exception>
    public static Task<Stream> CompressDeflateAsync(this IDecorator<Stream> decorator, Action<AsyncStreamCompressionOptions> setup = null)
    {
        Validator.ThrowIfNull(decorator);
        return CompressAsync(decorator, Patterns.Configure(setup), (stream, level, leaveOpen) => new DeflateStream(stream, level, leaveOpen));
    }

    /// <summary>
    /// Decompress the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/> using <c>Deflate</c> data format specification.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{Stream}"/> to extend.</param>
    /// <param name="setup">The <see cref="StreamCopyOptions"/> which may be configured.</param>
    /// <returns>A decompressed version of the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// The enclosed <see cref="Stream"/> of <paramref name="decorator"/> does not support write operations such as compression.
    /// </exception>
    /// <exception cref="InvalidDataException">
    /// The enclosed <see cref="Stream"/> of <paramref name="decorator"/> was compressed using an unsupported compression method.
    /// </exception>
    public static Stream DecompressDeflate(this IDecorator<Stream> decorator, Action<StreamCopyOptions> setup = null)
    {
        Validator.ThrowIfNull(decorator);
        return Decompress(decorator, Patterns.Configure(setup), (stream, mode, leaveOpen) => new DeflateStream(stream, mode, leaveOpen));
    }

    /// <summary>
    /// Decompress the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/> using <c>Deflate</c> data format specification.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{Stream}"/> to extend.</param>
    /// <param name="setup">The <see cref="AsyncStreamCopyOptions"/> which may be configured.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a decompressed version of the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// The enclosed <see cref="Stream"/> of <paramref name="decorator"/> does not support write operations such as compression.
    /// </exception>
    /// <exception cref="InvalidDataException">
    /// The enclosed <see cref="Stream"/> of <paramref name="decorator"/> was compressed using an unsupported compression method.
    /// </exception>
    public static Task<Stream> DecompressDeflateAsync(this IDecorator<Stream> decorator, Action<AsyncStreamCopyOptions> setup = null)
    {
        Validator.ThrowIfNull(decorator);
        return DecompressAsync(decorator, Patterns.Configure(setup), (stream, mode, leaveOpen) => new DeflateStream(stream, mode, leaveOpen));
    }

    private static Stream Compress<T>(IDecorator<Stream> decorator, StreamCompressionOptions options, Func<Stream, CompressionLevel, bool, T> decompressor) where T : Stream
    {
        return Patterns.SafeInvoke(() => new MemoryStream(), target =>
        {
            using (var compressed = decompressor(target, options.Level, true))
            {
                Decorator.Enclose(decorator.Inner).CopyStream(compressed, options.BufferSize);
            }

            target.Flush();
            target.Position = 0;
            return target;
        });
    }

    private static Task<Stream> CompressAsync<T>(IDecorator<Stream> decorator, AsyncStreamCompressionOptions options, Func<Stream, CompressionLevel, bool, T> decompressor) where T : Stream
    {
        return AsyncPatterns.SafeInvokeAsync<Stream>(() => new MemoryStream(), async (target, ct) =>
        {
#if NETSTANDARD2_1_OR_GREATER || NET10_0_OR_GREATER
            await using (var compressed = decompressor(target, options.Level, true))
            {
                await Decorator.Enclose(decorator.Inner).CopyStreamAsync(compressed, options.BufferSize, ct: ct).ConfigureAwait(false);
            }
#else
            using (var compressed = decompressor(target, options.Level, true))
            {
                await Decorator.Enclose(decorator.Inner).CopyStreamAsync(compressed, options.BufferSize, ct: ct).ConfigureAwait(false);
            }
#endif
            await target.FlushAsync(ct).ConfigureAwait(false);
            target.Position = 0;
            return target;
        }, ct: options.CancellationToken);
    }

    private static Stream Decompress<T>(IDecorator<Stream> decorator, StreamCopyOptions options, Func<Stream, CompressionMode, bool, T> compressor) where T : Stream
    {
        return Patterns.SafeInvoke(() => new MemoryStream(), target =>
        {
            using (var uncompressed = compressor(decorator.Inner, CompressionMode.Decompress, true))
            {
                Decorator.Enclose(uncompressed).CopyStream(target, options.BufferSize);
            }

            target.Flush();
            target.Position = 0;
            return target;
        });
    }

    private static Task<Stream> DecompressAsync<T>(IDecorator<Stream> decorator, AsyncStreamCopyOptions options, Func<Stream, CompressionMode, bool, T> compressor) where T : Stream
    {
        return AsyncPatterns.SafeInvokeAsync<Stream>(() => new MemoryStream(), async (target, ct) =>
        {
#if NETSTANDARD2_1_OR_GREATER || NET10_0_OR_GREATER
            await using (var uncompressed = compressor(decorator.Inner, CompressionMode.Decompress, true))
            {
                await Decorator.Enclose(uncompressed).CopyStreamAsync(target, options.BufferSize, ct: ct).ConfigureAwait(false);
            }
#else
            using (var uncompressed = compressor(decorator.Inner, CompressionMode.Decompress, true))
            {
                await Decorator.Enclose(uncompressed).CopyStreamAsync(target, options.BufferSize, ct: ct).ConfigureAwait(false);
            }
#endif
            await target.FlushAsync(ct).ConfigureAwait(false);
            target.Position = 0;
            return target;
        }, ct: options.CancellationToken);
    }

    /// <summary>
    /// Asynchronously writes a sequence of bytes to the enclosed <see cref="Stream"/> of the <paramref name="decorator"/> with the entire size of the <paramref name="buffer"/> starting from position 0.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <param name="buffer">The buffer to write data from.</param>
    /// <param name="ct">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> is null -or-
    /// <paramref name="buffer"/> is null.
    /// </exception>
    /// <exception cref="NotSupportedException">
    /// The enclosed <see cref="Stream"/> of <paramref name="decorator"/> does not support writing.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    /// The enclosed <see cref="Stream"/> of <paramref name="decorator"/> has been disposed.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The enclosed <see cref="Stream"/> of <paramref name="decorator"/> is currently in use by a previous write operation.
    /// </exception>
    public static Task WriteAllAsync(this IDecorator<Stream> decorator, byte[] buffer, CancellationToken ct = default)
    {
        Validator.ThrowIfNull(decorator);
        return decorator.Inner.WriteAsync(buffer, 0, buffer.Length, ct);
    }
}
