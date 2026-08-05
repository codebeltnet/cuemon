using System;
using Cuemon.Text;
using Cuemon.Net;

namespace Cuemon.Extensions.Net
{
    /// <summary>
    /// Extension methods for the <see cref="byte"/> array.
    /// </summary>
    public static class ByteArrayExtensions
    {
        /// <summary>
        /// Converts the specified <paramref name="bytes"/> into a URL-encoded array of bytes, starting at the specified <paramref name="position"/> in the array and continuing for the specified number of <paramref name="bytesToRead"/>.
        /// </summary>
        /// <param name="bytes">The <see cref="byte"/> array to extend.</param>
        /// <param name="position">The position in the byte array at which to begin encoding.</param>
        /// <param name="bytesToRead">The number of bytes to encode.</param>
        /// <param name="setup">The <see cref="EncodingOptions"/> which may be configured.</param>
        /// <returns>An encoded <see cref="byte"/> array.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="bytes"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="position" /> is lower than 0 - or -
        /// <paramref name="bytesToRead" /> is lower than 0 - or -
        /// <paramref name="position" /> is greater than or equal to the length of <paramref name="bytes"/> - or -
        /// <paramref name="bytesToRead" /> is greater than (the length of <paramref name="bytes"/> minus <paramref name="position"/>).
        /// </exception>
        public static byte[] UrlEncode(this byte[] bytes, int position = 0, int bytesToRead = -1, Action<EncodingOptions> setup = null)
        {
            Validator.ThrowIfNull(bytes);
            return Decorator.Enclose(bytes).UrlEncode(position, bytesToRead, setup);
        }
    }
}