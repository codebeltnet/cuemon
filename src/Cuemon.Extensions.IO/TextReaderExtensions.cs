using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Cuemon.IO;

namespace Cuemon.Extensions.IO
{
    /// <summary>
    /// Extension methods for the <see cref="TextReader"/> class.
    /// </summary>
    public static class TextReaderExtensions
    {
        /// <summary>
        /// Asynchronously reads the bytes from the <paramref name="reader"/> and writes them to the <paramref name="writer"/>.
        /// </summary>
        /// <param name="reader">The <see cref="TextReader"/> to extend.</param>
        /// <param name="writer">The <see cref="TextWriter"/> to asynchronously write bytes to.</param>
        /// <param name="bufferSize">The size, in bytes, of the buffer. This value must be greater than zero. The default size is 81920.</param>
        /// <returns>A task that represents the asynchronous copy operation.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="reader"/> cannot be null -or-
        /// <paramref name="writer"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="bufferSize" /> is lower than or equal to 0.
        /// </exception>
        public static Task CopyToAsync(this TextReader reader, TextWriter writer, int bufferSize = 81920)
        {
            Validator.ThrowIfNull(reader);
            return Decorator.Enclose(reader).CopyToAsync(writer, bufferSize);
        }

        /// <summary>
        /// Reads all lines of characters from the <paramref name="reader"/> and returns the data as a sequence of strings.
        /// </summary>
        /// <param name="reader">The <see cref="TextReader"/> to extend.</param>
        /// <returns>An <see cref="IEnumerable{String}"/> that contains all lines of characters from the <paramref name="reader"/>.</returns>
        public static IEnumerable<string> ReadAllLines(this TextReader reader)
        {
            Validator.ThrowIfNull(reader);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                yield return line;
            }
        }

        /// <summary>
        /// Asynchronously reads all lines of characters from the <paramref name="reader"/> and returns the data as a sequence of strings.
        /// </summary>
        /// <param name="reader">The <see cref="TextReader"/> to extend.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="IReadOnlyList{String}"/> that contains all lines of characters from the <paramref name="reader"/> that contains elements from the input sequence.</returns>
        public static async Task<IReadOnlyList<string>> ReadAllLinesAsync(this TextReader reader)
        {
            Validator.ThrowIfNull(reader);
            var lines = new List<string>();
            string line;
            while ((line = await reader.ReadLineAsync().ConfigureAwait(false)) != null)
            {
                lines.Add(line);
            }
            return lines;
        }
    }
}