using System;

namespace Cuemon
{
    /// <summary>
    /// Provides a set of static methods for eradicating different types of values or sequences of values.
    /// </summary>
    /// <seealso cref="Generate"/>
    public static class Eradicate
    {
        /// <summary>
        /// Eradicates trailing zero information (if any) from the specified set of <paramref name="bytes"/>.
        /// </summary>
        /// <param name="bytes">The <see cref="byte"/> array to process.</param>
        /// <returns>A <see cref="byte"/> array without trailing zeros.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="bytes"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="bytes" /> must have a length larger than 1.
        /// </exception>
        public static byte[] TrailingZeros(byte[] bytes)
        {
            return TrailingBytes(bytes, new byte[] { 0 });
        }

        /// <summary>
        /// Eradicates trailing byte information (if any) from the specified set of <paramref name="bytes"/>.
        /// </summary>
        /// <param name="bytes">The <see cref="byte"/> array to process.</param>
        /// <param name="trailingBytes">The <see cref="byte"/> array to form the trailing bytes.</param>
        /// <returns>A <see cref="byte"/> array without <paramref name="trailingBytes"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="bytes"/> cannot be null - or -
        /// <paramref name="trailingBytes"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="bytes" /> must have a length larger than 1.
        /// </exception>
        public static byte[] TrailingBytes(byte[] bytes, byte[] trailingBytes)
        {
            Validator.ThrowIfNull(bytes);
            Validator.ThrowIfNull(trailingBytes);
            Validator.ThrowIfLowerThanOrEqual(bytes.Length, 1, nameof(bytes), "The byte array must have a length larger than 1.");

            Array.Reverse(trailingBytes);
            var hasTrailingBytes = false;
            var marker = bytes.Length - 1;
            var size = trailingBytes.Length - 1;
            while (marker > size && HasMatch(marker, bytes, trailingBytes))
            {
                if (!hasTrailingBytes) { hasTrailingBytes = true; }
                marker -= trailingBytes.Length;
            }
            if (!hasTrailingBytes) { return bytes; }
            marker++;
            var output = new byte[marker];
            Array.Copy(bytes, output, marker);
            return output;
        }

        private static bool HasMatch(int index, byte[] bytes, byte[] trailingBytes)
        {
            var match = true;
            for (var i = 0; i < trailingBytes.Length; i++)
            {
                match &= trailingBytes[i] == bytes[index - i];
            }
            return match;
        }
    }
}
