using System;
using System.IO;
using System.Text;
using Cuemon.IO;

namespace Cuemon.Text
{
    /// <summary>
    /// Provides static helper methods for detecting, decoding, and removing Unicode byte order marks (BOMs).
    /// </summary>
    public static class ByteOrderMark
    {
        /// <summary>
        /// Decodes the byte order mark (BOM) in the specified byte array to its corresponding <see cref="Encoding"/>.
        /// </summary>
        /// <param name="bytes">The byte array that contains the BOM to decode.</param>
        /// <returns>The <see cref="Encoding"/> represented by the BOM in <paramref name="bytes"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="bytes"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="bytes"/> does not contain a recognizable byte order mark.
        /// </exception>
        public static Encoding Decode(byte[] bytes)
        {
            Validator.ThrowIfNull(bytes);
            if (BomIsUtf8(bytes)) { return Encoding.GetEncoding("UTF-8"); }
            if (BomIsUtf32BigEndian(bytes)) { return Encoding.GetEncoding("UTF-32BE"); }
            if (BomIsUtf32(bytes)) { return Encoding.GetEncoding("UTF-32"); }
            if (BomIsUtf16BigEndian(bytes)) { return Encoding.GetEncoding("UNICODEFFFE"); }
            if (BomIsUtf16(bytes)) { return Encoding.GetEncoding("UTF-16"); }
            throw new ArgumentException("Unable to locate and decode BOM.", nameof(bytes));
        }

        private static bool BomIsUtf8(byte[] bytes)
        {
            return bytes.Length >= 3 &&
                   bytes[0] == 0xEF &&
                   bytes[1] == 0xBB &&
                   bytes[2] == 0xBF;
        }

        private static bool BomIsUtf32BigEndian(byte[] bytes)
        {
            return bytes.Length >= 4 &&
                   bytes[0] == 0x00 &&
                   bytes[1] == 0x00 &&
                   bytes[2] == 0xFE &&
                   bytes[3] == 0xFF;
        }

        private static bool BomIsUtf32(byte[] bytes)
        {
            return bytes.Length >= 4 &&
                   bytes[0] == 0xFF &&
                   bytes[1] == 0xFE &&
                   bytes[2] == 0x00 &&
                   bytes[3] == 0x00;
        }

        private static bool BomIsUtf16BigEndian(byte[] bytes)
        {
            return bytes.Length >= 2 &&
                   bytes[0] == 0xFE &&
                   bytes[1] == 0xFF;
        }

        private static bool BomIsUtf16(byte[] bytes)
        {
            return bytes.Length >= 2 &&
                   bytes[0] == 0xFF &&
                   bytes[1] == 0xFE;
        }

        /// <summary>
        /// Detects the encoding of the specified byte array, or returns a fallback encoding if detection fails.
        /// </summary>
        /// <param name="input">The byte array to inspect.</param>
        /// <param name="fallbackEncoding">The encoding to return when detection fails.</param>
        /// <returns>
        /// The detected encoding of <paramref name="input"/>, or <paramref name="fallbackEncoding"/> if detection fails.
        /// If <paramref name="fallbackEncoding"/> is <see langword="null"/>, <see cref="EncodingOptions.DefaultEncoding"/> is returned.
        /// </returns>
        public static Encoding DetectEncodingOrDefault(byte[] input, Encoding fallbackEncoding)
        {
            if (TryDetectEncoding(input, out var result))
            {
                return result;
            }
            return fallbackEncoding ?? EncodingOptions.DefaultEncoding;
        }

        /// <summary>
        /// Detects the encoding of the specified stream, or returns a fallback encoding if detection fails.
        /// </summary>
        /// <param name="value">The stream to inspect.</param>
        /// <param name="fallbackEncoding">The encoding to return when detection fails.</param>
        /// <returns>
        /// The detected encoding of <paramref name="value"/>, or <paramref name="fallbackEncoding"/> if detection fails.
        /// </returns>
        public static Encoding DetectEncodingOrDefault(Stream value, Encoding fallbackEncoding)
        {
            if (TryDetectEncoding(value, out var result))
            {
                return result;
            }
            return fallbackEncoding;
        }

        /// <summary>
        /// Tries to detect the encoding represented by the byte order mark in the specified byte array.
        /// </summary>
        /// <param name="input">The byte array to inspect.</param>
        /// <param name="result">
        /// When this method returns, contains the detected <see cref="Encoding"/> if detection succeeds;
        /// otherwise, <see langword="null"/>.
        /// </param>
        /// <returns><see langword="true"/> if an encoding was detected; otherwise, <see langword="false"/>.</returns>
        public static bool TryDetectEncoding(byte[] input, out Encoding result)
        {
            return Patterns.TryInvoke(() => Decode(input), out result);
        }

        /// <summary>
        /// Tries to detect the encoding represented by the byte order mark in the specified stream.
        /// </summary>
        /// <param name="value">The stream to inspect.</param>
        /// <param name="result">
        /// When this method returns, contains the detected <see cref="Encoding"/> if detection succeeds;
        /// otherwise, <see langword="null"/>.
        /// </param>
        /// <returns><see langword="true"/> if an encoding was detected; otherwise, <see langword="false"/>.</returns>
        /// <remarks>
        /// This method reads up to the first four bytes of <paramref name="value"/> and restores the original stream position
        /// before returning. The stream must support seeking.
        /// </remarks>
        public static bool TryDetectEncoding(Stream value, out Encoding result)
        {
            if (value == null || !value.CanSeek)
            {
                result = null;
                return false;
            }

            byte[] byteOrderMarks = { 0, 0, 0, 0 };
            var startingPosition = value.Position;
            value.Position = 0;
            var bytesRead = value.Read(byteOrderMarks, 0, 4); // only read the first 4 bytes
            value.Seek(startingPosition, SeekOrigin.Begin); // reset to original position

            if (bytesRead < byteOrderMarks.Length)
            {
                var resizedByteOrderMarks = new byte[bytesRead];
                Array.Copy(byteOrderMarks, resizedByteOrderMarks, bytesRead);
                byteOrderMarks = resizedByteOrderMarks;
            }

            return TryDetectEncoding(byteOrderMarks, out result);
        }

        /// <summary>
        /// Removes the preamble, if present, from the specified stream.
        /// </summary>
        /// <param name="value">The stream to process.</param>
        /// <param name="encoding">The encoding used to determine which preamble to remove.</param>
        /// <param name="setup">The delegate that configures disposable behavior.</param>
        /// <returns>A stream whose content does not include the detected preamble.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> or <paramref name="encoding"/> is <see langword="null"/>.
        /// </exception>
        public static Stream Remove(Stream value, Encoding encoding, Action<DisposableOptions> setup = null)
        {
            Validator.ThrowIfNull(value);
            Validator.ThrowIfNull(encoding);

            var option = Patterns.Configure(setup);
            var bytes = Decorator.Enclose(value).InvokeToByteArray(leaveOpen: option.LeaveOpen);
            bytes = Remove(bytes, encoding);
            return Patterns.SafeInvoke(() => new MemoryStream(bytes.Length), ms =>
            {
                ms.Write(bytes, 0, bytes.Length);
                ms.Position = 0;
                return ms;
            });
        }

        /// <summary>
        /// Removes the preamble, if present, from the specified byte array.
        /// </summary>
        /// <param name="bytes">The byte array to process.</param>
        /// <param name="encoding">The encoding used to determine which preamble to remove.</param>
        /// <returns>A byte array whose content does not include the detected preamble.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="bytes"/> or <paramref name="encoding"/> is <see langword="null"/>.
        /// </exception>
        public static byte[] Remove(byte[] bytes, Encoding encoding)
        {
            Validator.ThrowIfNull(bytes);
            Validator.ThrowIfNull(encoding);
            if (bytes.Length <= 1) { return bytes; }
            var preamble = encoding.GetPreamble();
            if (preamble.Length == 0 || bytes.Length < preamble.Length) { return bytes; }
            for (var i = 0; i < preamble.Length; i++)
            {
                if (preamble[i] != bytes[i]) { return bytes; }
            }
            var bytesToRead = bytes.Length - preamble.Length;
            var bytesWithNoPreamble = new byte[bytesToRead];
            Array.Copy(bytes, preamble.Length, bytesWithNoPreamble, 0, bytesToRead);
            return bytesWithNoPreamble;
        }
    }
}
