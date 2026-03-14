using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cuemon.IO;
using Cuemon.Text;

namespace Cuemon.Extensions.IO
{
    /// <summary>
    /// Extension methods for the <see cref="Stream"/> class.
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>
        /// Combines two streams, <paramref name="first"/> and <paramref name="second"/>, into one stream.
        /// </summary>
        /// <param name="first">The <see cref="Stream"/> to extend.</param>
        /// <param name="second">The <see cref="Stream"/> to concat with <paramref name="first"/>.</param>
        /// <param name="setup">The <see cref="DisposableOptions"/> which may be configured.</param>
        /// <returns>The concatenated <see cref="Stream"/> representations of values <paramref name="first"/> and <paramref name="second"/>.</returns>
        public static Stream Concat(this Stream first, Stream second, Action<DisposableOptions> setup = null)
        {
            Validator.ThrowIfNull(first);
            Validator.ThrowIfNull(second);
            var options = Patterns.Configure(setup);
            var result = new MemoryStream();
            try
            {
                first.CopyTo(result);
                second.CopyTo(result);
            }
            finally
            {
                if (!options.LeaveOpen)
                {
                    first.Dispose();
                    second.Dispose();
                }
            }
            result.Flush();
            result.Position = 0;
            return result;
        }

        /// <summary>
        /// Converts the specified <paramref name="input"/> to its equivalent <see cref="T:char[]"/> representation.
        /// </summary>
        /// <param name="input">The <see cref="Stream"/> to be extended.</param>
        /// <param name="setup">The <see cref="EncodingOptions"/> which may be configured.</param>
        /// <returns>A <see cref="T:char[]"/> that is equivalent to <paramref name="input"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="input"/> cannot be read from.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="input" /> length is greater than <see cref="int.MaxValue"/>.
        /// </exception>
        /// <exception cref="InvalidEnumArgumentException">
        /// <paramref name="setup"/> was initialized with an invalid <see cref="EncodingOptions.Preamble"/>.
        /// </exception>
        /// <remarks><see cref="EncodingOptions"/> will be initialized with <see cref="EncodingOptions.DefaultPreambleSequence"/> and <see cref="EncodingOptions.DefaultEncoding"/>.</remarks>
        public static char[] ToCharArray(this Stream input, Action<EncodingOptions> setup = null)
        {
            Validator.ThrowIfNull(input);
            var options = Patterns.Configure(setup);
            if (options.Encoding.Equals(EncodingOptions.DefaultEncoding)) { options.Encoding = ByteOrderMark.DetectEncodingOrDefault(input, options.Encoding); }
            var valueInBytes = Decorator.Enclose(input).ToByteArray();
            switch (options.Preamble)
            {
                case PreambleSequence.Keep:
                    break;
                case PreambleSequence.Remove:
                    valueInBytes = ByteOrderMark.Remove(valueInBytes, options.Encoding);
                    break;
                default:
                    throw new InvalidEnumArgumentException(nameof(setup), (int)options.Preamble, typeof(PreambleSequence));
            }
            return options.Encoding.GetChars(valueInBytes);
        }

        /// <summary>
        /// Converts the specified <paramref name="input"/> to its equivalent <see cref="T:byte[]"/> representation.
        /// </summary>
        /// <param name="input">The <see cref="Stream"/> to extend.</param>
        /// <param name="setup">The <see cref="StreamCopyOptions"/> which may be configured.</param>
        /// <returns>A <see cref="T:byte[]"/> that is equivalent to <paramref name="input"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="input"/> cannot be read from.
        /// </exception>
        public static byte[] ToByteArray(this Stream input, Action<StreamCopyOptions> setup = null)
        {
            Validator.ThrowIfNull(input);
            Validator.ThrowIfFalse(input.CanRead, nameof(input), "Stream cannot be read from.");
            return Decorator.Enclose(input).ToByteArray(setup);
        }


        /// <summary>
        /// Converts the specified <paramref name="input"/> to its equivalent <see cref="T:byte[]"/> representation.
        /// </summary>
        /// <param name="input">The <see cref="Stream"/> to extend.</param>
        /// <param name="setup">The <see cref="AsyncStreamCopyOptions"/> which may be configured.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:byte[]"/> that is equivalent to <paramref name="input"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="input"/> cannot be read from.
        /// </exception>
        public static Task<byte[]> ToByteArrayAsync(this Stream input, Action<AsyncStreamCopyOptions> setup = null)
        {
            Validator.ThrowIfNull(input);
            Validator.ThrowIfFalse(input.CanRead, nameof(input), "Stream cannot be read from.");
            return Decorator.Enclose(input).ToByteArrayAsync(setup);
        }

        /// <summary>
        /// Asynchronously writes a sequence of bytes to the current stream with the entire size of the <paramref name="buffer"/> starting from position 0.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to extend.</param>
        /// <param name="buffer">The buffer to write data from.</param>
        /// <param name="ct">The token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous write operation.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="stream"/> is null -or-
        /// <paramref name="buffer">buffer</paramref> is null.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// The stream does not support writing.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The stream has been disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The stream is currently in use by a previous write operation.
        /// </exception>
        public static Task WriteAllAsync(this Stream stream, byte[] buffer, CancellationToken ct = default)
        {
            Validator.ThrowIfNull(stream);
            return Decorator.Enclose(stream).WriteAllAsync(buffer, ct);
        }

        /// <summary>
        /// Tries to resolve the Unicode <see cref="Encoding"/> object from the specified <see cref="Stream"/> object.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> to extend.</param>
        /// <param name="result">When this method returns, it contains the Unicode <see cref="Encoding"/> value equivalent to the encoding contained in <paramref name="value"/>, if the conversion succeeded, or a null reference (Nothing in Visual Basic) if the conversion failed. The conversion fails if the <paramref name="value"/> parameter is null, or does not contain a Unicode representation of an <see cref="Encoding"/>.</param>
        /// <returns><c>true</c> if the <paramref name="value"/> parameter was converted successfully; otherwise, <c>false</c>.</returns>
        public static bool TryDetectUnicodeEncoding(this Stream value, out Encoding result)
        {
            return ByteOrderMark.TryDetectEncoding(value, out result);
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <see cref="string"/>.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> to extend.</param>
        /// <param name="setup">The <see cref="StreamReaderOptions"/> which may be configured.</param>
        /// <returns>A <see cref="string"/> containing the result of the specified <paramref name="value"/>.</returns>
        /// <remarks><see cref="IEncodingOptions"/> will be initialized with <see cref="EncodingOptions.DefaultPreambleSequence"/> and <see cref="EncodingOptions.DefaultEncoding"/>.</remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> cannot be null.
        /// </exception>
        /// <exception cref="InvalidEnumArgumentException">
        /// <paramref name="setup"/> was initialized with an invalid <see cref="StreamEncodingOptions.Preamble"/>.
        /// </exception>
        public static string ToEncodedString(this Stream value, Action<StreamEncodingOptions> setup = null)
        {
            Validator.ThrowIfNull(value);
            return Decorator.Enclose(value).ToEncodedString(setup);
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <see cref="string"/>.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> to extend.</param>
        /// <param name="setup">The <see cref="AsyncStreamEncodingOptions"/> which may be configured.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="string"/> containing the result of the specified <paramref name="value"/>.</returns>
        /// <remarks><see cref="IEncodingOptions"/> will be initialized with <see cref="EncodingOptions.DefaultPreambleSequence"/> and <see cref="EncodingOptions.DefaultEncoding"/>.</remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> cannot be null.
        /// </exception>
        /// <exception cref="InvalidEnumArgumentException">
        /// <paramref name="setup"/> was initialized with an invalid <see cref="StreamEncodingOptions.Preamble"/>.
        /// </exception>
        public static Task<string> ToEncodedStringAsync(this Stream value, Action<AsyncStreamEncodingOptions> setup = null)
        {
            Validator.ThrowIfNull(value);
            return Decorator.Enclose(value).ToEncodedStringAsync(setup);
        }

#if NETSTANDARD2_1_OR_GREATER || NET10_0_OR_GREATER

        /// <summary>
        /// Compresses the <paramref name="value"/> using the BROTLI algorithm.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> to extend.</param>
        /// <param name="setup">The <see cref="StreamCompressionOptions"/> which may be configured.</param>
        /// <returns>A BROTLI compressed <see cref="Stream"/> of the <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> does not support write operations such as compression.
        /// </exception>
        public static Stream CompressBrotli(this Stream value, Action<StreamCompressionOptions> setup = null)
        {
            Validator.ThrowIfNull(value);
            return Decorator.Enclose(value).CompressBrotli(setup);
        }

        /// <summary>
        /// Compresses the <paramref name="value"/> using the BROTLI algorithm.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> to extend.</param>
        /// <param name="setup">The <see cref="AsyncStreamCompressionOptions"/> which may be configured.</param>
        /// <returns>A DEFLATE compressed <see cref="Stream"/> of the <paramref name="value"/>.</returns>
        /// <returns>A task that represents the asynchronous operation. The task result contains a BROTLI compressed <see cref="Stream"/> of the specified <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> does not support write operations such as compression.
        /// </exception>
        public static Task<Stream> CompressBrotliAsync(this Stream value, Action<AsyncStreamCompressionOptions> setup = null)
        {
            Validator.ThrowIfNull(value);
            return Decorator.Enclose(value).CompressBrotliAsync(setup);
        }

#endif

        /// <summary>
        /// Compresses the <paramref name="value"/> using the DEFLATE algorithm.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> to extend.</param>
        /// <param name="setup">The <see cref="StreamCompressionOptions"/> which may be configured.</param>
        /// <returns>A DEFLATE compressed <see cref="Stream"/> of the <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> does not support write operations such as compression.
        /// </exception>
        public static Stream CompressDeflate(this Stream value, Action<StreamCompressionOptions> setup = null)
        {
            Validator.ThrowIfNull(value);
            return Decorator.Enclose(value).CompressDeflate(setup);
        }

        /// <summary>
        /// Compresses the <paramref name="value"/> using the DEFLATE algorithm.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> to extend.</param>
        /// <param name="setup">The <see cref="AsyncStreamCompressionOptions"/> which may be configured.</param>
        /// <returns>A DEFLATE compressed <see cref="Stream"/> of the <paramref name="value"/>.</returns>
        /// <returns>A task that represents the asynchronous operation. The task result contains a DEFLATE compressed <see cref="Stream"/> of the specified <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> does not support write operations such as compression.
        /// </exception>
        public static Task<Stream> CompressDeflateAsync(this Stream value, Action<AsyncStreamCompressionOptions> setup = null)
        {
            Validator.ThrowIfNull(value);
            return Decorator.Enclose(value).CompressDeflateAsync(setup);
        }

        /// <summary>
        /// Compresses the <paramref name="value"/> using the GZIP algorithm.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> to extend.</param>
        /// <param name="setup">The <see cref="StreamCompressionOptions"/> which may be configured.</param>
        /// <returns>A GZIP compressed <see cref="Stream"/> of the <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> does not support write operations such as compression.
        /// </exception>
        public static Stream CompressGZip(this Stream value, Action<StreamCompressionOptions> setup = null)
        {
            Validator.ThrowIfNull(value);
            return Decorator.Enclose(value).CompressGZip(setup);
        }

        /// <summary>
        /// Compresses the <paramref name="value"/> using the GZIP algorithm.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> to extend.</param>
        /// <param name="setup">The <see cref="AsyncStreamCompressionOptions"/> which may be configured.</param>
        /// <returns>A DEFLATE compressed <see cref="Stream"/> of the <paramref name="value"/>.</returns>
        /// <returns>A task that represents the asynchronous operation. The task result contains a GZIP compressed <see cref="Stream"/> of the specified <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> does not support write operations such as compression.
        /// </exception>
        public static Task<Stream> CompressGZipAsync(this Stream value, Action<AsyncStreamCompressionOptions> setup = null)
        {
            Validator.ThrowIfNull(value);
            return Decorator.Enclose(value).CompressGZipAsync(setup);
        }

#if NETSTANDARD2_1_OR_GREATER || NET10_0_OR_GREATER

        /// <summary>
        /// Decompresses the <paramref name="value"/> using the BROTLI data format specification.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> to extend.</param>
        /// <param name="setup">The <see cref="StreamCopyOptions"/> which may be configured.</param>
        /// <returns>A decompressed <see cref="Stream"/> of the <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> does not support write operations such as compression.
        /// </exception>
        /// <exception cref="InvalidDataException">
        /// <paramref name="value"/> was compressed using an unsupported compression method.
        /// </exception>
        public static Stream DecompressBrotli(this Stream value, Action<StreamCopyOptions> setup = null)
        {
            Validator.ThrowIfNull(value);
            return Decorator.Enclose(value).DecompressBrotli(setup);
        }

        /// <summary>
        /// Decompresses the <paramref name="value"/> using the BROTLI data format specification.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> to extend.</param>
        /// <param name="setup">The <see cref="AsyncStreamCopyOptions"/> which may be configured.</param>
        /// <returns>A decompressed <see cref="Stream"/> of the <paramref name="value"/>.</returns>
        /// <returns>A task that represents the asynchronous operation. The task result contains a decompressed <see cref="Stream"/> of the specified <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> does not support write operations such as compression.
        /// </exception>
        /// <exception cref="InvalidDataException">
        /// <paramref name="value"/> was compressed using an unsupported compression method.
        /// </exception>
        public static Task<Stream> DecompressBrotliAsync(this Stream value, Action<AsyncStreamCopyOptions> setup = null)
        {
            Validator.ThrowIfNull(value);
            return Decorator.Enclose(value).DecompressBrotliAsync(setup);
        }

#endif

        /// <summary>
        /// Decompresses the <paramref name="value"/> using the DEFLATE data format specification.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> to extend.</param>
        /// <param name="setup">The <see cref="StreamCopyOptions"/> which may be configured.</param>
        /// <returns>A decompressed <see cref="Stream"/> of the <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> does not support write operations such as compression.
        /// </exception>
        /// <exception cref="InvalidDataException">
        /// <paramref name="value"/> was compressed using an unsupported compression method.
        /// </exception>
        public static Stream DecompressDeflate(this Stream value, Action<StreamCopyOptions> setup = null)
        {
            Validator.ThrowIfNull(value);
            return Decorator.Enclose(value).DecompressDeflate(setup);
        }

        /// <summary>
        /// Decompresses the <paramref name="value"/> using the DEFLATE data format specification.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> to extend.</param>
        /// <param name="setup">The <see cref="AsyncStreamCopyOptions"/> which may be configured.</param>
        /// <returns>A decompressed <see cref="Stream"/> of the <paramref name="value"/>.</returns>
        /// <returns>A task that represents the asynchronous operation. The task result contains a decompressed <see cref="Stream"/> of the specified <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> does not support write operations such as compression.
        /// </exception>
        /// <exception cref="InvalidDataException">
        /// <paramref name="value"/> was compressed using an unsupported compression method.
        /// </exception>
        public static Task<Stream> DecompressDeflateAsync(this Stream value, Action<AsyncStreamCopyOptions> setup = null)
        {
            Validator.ThrowIfNull(value);
            return Decorator.Enclose(value).DecompressDeflateAsync(setup);
        }

        /// <summary>
        /// Decompresses the <paramref name="value"/> using the GZIP data format specification.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> to extend.</param>
        /// <param name="setup">The <see cref="StreamCopyOptions"/> which may be configured.</param>
        /// <returns>A decompressed <see cref="Stream"/> of the <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> does not support write operations such as compression.
        /// </exception>
        /// <exception cref="InvalidDataException">
        /// <paramref name="value"/> was compressed using an unsupported compression method.
        /// </exception>
        public static Stream DecompressGZip(this Stream value, Action<StreamCopyOptions> setup = null)
        {
            Validator.ThrowIfNull(value);
            return Decorator.Enclose(value).DecompressGZip(setup);
        }

        /// <summary>
        /// Decompresses the <paramref name="value"/> using the GZIP data format specification.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> to extend.</param>
        /// <param name="setup">The <see cref="AsyncStreamCopyOptions"/> which may be configured.</param>
        /// <returns>A decompressed <see cref="Stream"/> of the <paramref name="value"/>.</returns>
        /// <returns>A task that represents the asynchronous operation. The task result contains a decompressed <see cref="Stream"/> of the specified <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> does not support write operations such as compression.
        /// </exception>
        /// <exception cref="InvalidDataException">
        /// <paramref name="value"/> was compressed using an unsupported compression method.
        /// </exception>
        public static Task<Stream> DecompressGZipAsync(this Stream value, Action<AsyncStreamCopyOptions> setup = null)
        {
            Validator.ThrowIfNull(value);
            return Decorator.Enclose(value).DecompressGZipAsync(setup);
        }
    }
}
