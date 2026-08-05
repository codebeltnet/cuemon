using System;
using System.IO;
using Cuemon.IO;
using Cuemon.Text;

namespace Cuemon.Net
{
    /// <summary>
    /// Extension methods for the <see cref="byte"/> array hidden behind the <see cref="IDecorator{T}"/> interface.
    /// </summary>
    /// <seealso cref="IDecorator{T}"/>
    /// <seealso cref="Decorator{T}"/>
    public static class ByteArrayDecoratorExtensions
    {
        private static readonly char[] HexadecimalCharactersLowerCase = Alphanumeric.Hexadecimal.ToLowerInvariant().ToCharArray();

        /// <summary>
        /// Converts the enclosed <see cref="byte"/> array of the specified <paramref name="decorator"/> into a URL-encoded array of bytes, starting at the specified <paramref name="position"/> in the array and continuing for the specified number of <paramref name="bytesToRead"/>.
        /// </summary>
        /// <param name="decorator">The decorator that wraps the <see cref="byte"/> array to extend.</param>
        /// <param name="position">The position in the byte array at which to begin encoding.</param>
        /// <param name="bytesToRead">The number of bytes to encode.</param>
        /// <param name="setup">The <see cref="EncodingOptions"/> which may be configured.</param>
        /// <returns>An encoded <see cref="byte"/> array.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="position" /> is lower than 0 - or -
        /// <paramref name="bytesToRead" /> is lower than 0 - or -
        /// <paramref name="position" /> is greater than or equal to the length of the enclosed <see cref="byte"/> array of the specified <paramref name="decorator"/> - or -
        /// <paramref name="bytesToRead" /> is greater than (the length of the enclosed <see cref="byte"/> array of the specified <paramref name="decorator"/> minus <paramref name="position"/>).
        /// </exception>
        public static byte[] UrlEncode(this IDecorator<byte[]> decorator, int position = 0, int bytesToRead = -1, Action<EncodingOptions> setup = null)
        {
            Validator.ThrowIfNull(decorator);
            var bytes = decorator.Inner;
            if (bytes.Length == 0) { return Array.Empty<byte>(); }

            Validator.ThrowIfLowerThan(position, 0, nameof(position));
            Validator.ThrowIfGreaterThanOrEqual(position, bytes.Length, nameof(position));
            Validator.ThrowIfLowerThan(bytesToRead, 0, nameof(bytesToRead));
            Validator.ThrowIfGreaterThan(bytesToRead, bytes.Length - position, nameof(bytesToRead));

            var options = Patterns.Configure(setup);
            using (var result = StreamFactory.Create(UrlEncodeCharWriter, bytes, position, bytesToRead, HexadecimalCharactersLowerCase, o =>
            {
                o.Encoding = options.Encoding;
                o.Preamble = options.Preamble;
            }))
            {
                return Decorator.Enclose(result).ToByteArray();
            }
        }

        private static void UrlEncodeCharWriter(StreamWriter writer, byte[] bytes, int offset, int count, char[] hexadecimalCharacters)
        {
            var end = offset + count;
            for (var i = offset; i < end; i++)
            {
                var c = (char)bytes[i];
                if (c > 255)
                {
                    int idx;
                    int j = c;

                    writer.Write('%');
                    writer.Write('u');
                    idx = j >> 12;
                    writer.Write(hexadecimalCharacters[idx]);
                    idx = (j >> 8) & 0x0F;
                    writer.Write(hexadecimalCharacters[idx]);
                    idx = (j >> 4) & 0x0F;
                    writer.Write(hexadecimalCharacters[idx]);
                    idx = j & 0x0F;
                    writer.Write(hexadecimalCharacters[idx]);
                    continue;
                }

                if (c > ' ' && Infrastructure.NotEncoded(c))
                {
                    writer.Write(c);
                    continue;
                }
                if (c == ' ')
                {
                    writer.Write('+');
                    continue;
                }
                if ((c < '0') ||
                    (c < 'A' && c > '9') ||
                    (c > 'Z' && c < 'a') ||
                    (c > 'z'))
                {
                    writer.Write('%');
                    var idx = ((int)c) >> 4;
                    writer.Write(hexadecimalCharacters[idx]);
                    idx = ((int)c) & 0x0F;
                    writer.Write(hexadecimalCharacters[idx]);
                }
                else
                {
                    writer.Write(c);
                }
            }
        }
    }
}